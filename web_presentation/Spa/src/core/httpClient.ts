import axios from "axios";
import type {
  AxiosInstance,
  AxiosRequestConfig,
  AxiosResponse,
  InternalAxiosRequestConfig,
} from "axios";
import { type Tokens } from "../models/auth";
import { useAuthStore } from "@/stores/auth.store";

// Base HTTP client configuration
const API_BASE_URL =
  import.meta.env.VITE_API_URL || "http://localhost:5299/api";

export interface ApiResponse<T = unknown> {
  data: T;
  status: number;
  statusText: string;
}

export class ApiError {
  message: string;
  status?: number;
  details?: unknown;

  constructor(message: string, status?: number, details?: unknown) {
    this.message = message;
    this.status = status;
    this.details = details;
  }
}

interface FailedRequest {
  resolve: (value: string | null) => void;
  reject: (error: Error) => void;
}

class HttpClient {
  private client: AxiosInstance;
  private isRefreshing = false;
  private failedQueue: FailedRequest[] = [];

  constructor() {
    this.client = axios.create({
      baseURL: API_BASE_URL,
      timeout: 10000,
      headers: {
        "Content-Type": "application/json",
      },
    });

    this.setupInterceptors();
  }
  private setupInterceptors() {
    // Request interceptor to add authorization token
    this.client.interceptors.request.use(
      (config: InternalAxiosRequestConfig) => {
        const accessToken = useAuthStore.getState().accessToken;
        if (accessToken) {
          config.headers.Authorization = `Bearer ${accessToken}`;
        }
        return config;
      },
      (error: Error) => {
        return Promise.reject(error);
      }
    );

    // Response interceptor to handle token refresh
    this.client.interceptors.response.use(
      (response: AxiosResponse) => response,
      async (error: unknown) => {
        const originalRequest = error as AxiosResponse & {
          config: InternalAxiosRequestConfig & { _retry?: boolean };
        };

        if (
          axios.isAxiosError(error) &&
          error.response?.status === 401 &&
          error.response.data?.detail.includes("token expired") &&
          !originalRequest.config._retry
        ) {
          if (this.isRefreshing) {
            // If token is already being refreshed, add to queue
            return new Promise((resolve, reject) => {
              this.failedQueue.push({ resolve, reject });
            })
              .then((token) => {
                originalRequest.config.headers.Authorization = `Bearer ${token}`;
                return this.client(originalRequest.config);
              })
              .catch((err) => {
                return Promise.reject(err);
              });
          }

          originalRequest.config._retry = true;
          this.isRefreshing = true;

          try {
            const newTokens = await this.refreshToken();
            if (newTokens) {
              // Process the queue of failed requests
              this.processQueue(null, newTokens.accessToken);

              // Retry the original request
              originalRequest.config.headers.Authorization = `Bearer ${newTokens.accessToken}`;
              return this.client(originalRequest.config);
            }
          } catch (refreshError) {
            this.processQueue(refreshError as Error, null);
            const logout = await import("@/utils/auth").then(
              (module) => module.logout
            );
            logout();

            return Promise.reject(refreshError);
          } finally {
            this.isRefreshing = false;
          }
        }

        return Promise.reject(error);
      }
    );
  }

  private processQueue(error: Error | null, token: string | null) {
    this.failedQueue.forEach(({ resolve, reject }) => {
      if (error) {
        reject(error);
      } else {
        resolve(token);
      }
    });

    this.failedQueue = [];
  }

  private async refreshToken(): Promise<Tokens | null> {
    try {
      const refreshToken = useAuthStore.getState().refreshToken;
      if (!refreshToken) {
        throw new Error("No refresh token available");
      }

      const response = await axios.post<Tokens>(
        `${API_BASE_URL}/auth/refresh`,
        { RefreshToken: refreshToken },
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );

      const newTokens = response.data;

      useAuthStore.getState().authenticate(newTokens);
      return newTokens;
    } catch (error) {
      console.error("Error refreshing token:", error);
      return null;
    }
  }

  // Public methods for making HTTP requests
  async get<T = unknown>(
    url: string,
    config?: AxiosRequestConfig
  ): Promise<ApiResponse<T>> {
    try {
      const response = await this.client.get<T>(url, config);
      return {
        data: response.data,
        status: response.status,
        statusText: response.statusText,
      };
    } catch (error) {
      throw this.handleError(error);
    }
  }

  async post<T = unknown>(
    url: string,
    data?: unknown,
    config?: AxiosRequestConfig
  ): Promise<ApiResponse<T>> {
    try {
      const response = await this.client.post<T>(url, data, config);
      return {
        data: response.data,
        status: response.status,
        statusText: response.statusText,
      };
    } catch (error) {
      throw this.handleError(error);
    }
  }

  async put<T = unknown>(
    url: string,
    data?: unknown,
    config?: AxiosRequestConfig
  ): Promise<ApiResponse<T>> {
    try {
      const response = await this.client.put<T>(url, data, config);
      return {
        data: response.data,
        status: response.status,
        statusText: response.statusText,
      };
    } catch (error) {
      throw this.handleError(error);
    }
  }

  async patch<T = unknown>(
    url: string,
    data?: unknown,
    config?: AxiosRequestConfig
  ): Promise<ApiResponse<T>> {
    try {
      const response = await this.client.patch<T>(url, data, config);
      return {
        data: response.data,
        status: response.status,
        statusText: response.statusText,
      };
    } catch (error) {
      throw this.handleError(error);
    }
  }

  async delete<T = unknown>(
    url: string,
    config?: AxiosRequestConfig
  ): Promise<ApiResponse<T>> {
    try {
      const response = await this.client.delete<T>(url, config);
      return {
        data: response.data,
        status: response.status,
        statusText: response.statusText,
      };
    } catch (error) {
      throw this.handleError(error);
    }
  }

  // Method for making requests without authentication (useful for login/register)
  async publicRequest<T = unknown>(
    method: "GET" | "POST" | "PUT" | "DELETE" | "PATCH",
    url: string,
    data?: unknown,
    config?: AxiosRequestConfig
  ): Promise<ApiResponse<T>> {
    try {
      const fullConfig = {
        ...config,
        headers: {
          "Content-Type": "application/json",
          ...config?.headers,
        },
      };

      let response: AxiosResponse<T>;
      switch (method) {
        case "GET":
          response = await axios.get<T>(`${API_BASE_URL}${url}`, fullConfig);
          break;
        case "POST":
          response = await axios.post<T>(
            `${API_BASE_URL}${url}`,
            data,
            fullConfig
          );
          break;
        case "PUT":
          response = await axios.put<T>(
            `${API_BASE_URL}${url}`,
            data,
            fullConfig
          );
          break;
        case "PATCH":
          response = await axios.patch<T>(
            `${API_BASE_URL}${url}`,
            data,
            fullConfig
          );
          break;
        case "DELETE":
          response = await axios.delete<T>(`${API_BASE_URL}${url}`, fullConfig);
          break;
        default:
          throw new Error(`Unsupported HTTP method: ${method}`);
      }

      return {
        data: response.data,
        status: response.status,
        statusText: response.statusText,
      };
    } catch (error) {
      throw this.handleError(error);
    }
  }

  private handleError(error: unknown): ApiError {
    if (axios.isAxiosError(error)) {
      const message =
        error.response?.data?.message ||
        error.response?.data?.detail ||
        error.message ||
        "An error occurred in the request";

      return new ApiError(
        message,
        error.response?.status,
        error.response?.data
      );
    }

    if (error instanceof Error) return new ApiError(error.message);

    return new ApiError("An unknown error occurred");
  }

  // Method to get the axios instance (useful for special cases)
  getAxiosInstance(): AxiosInstance {
    return this.client;
  }

  // Method to update the base URL at runtime
  setBaseURL(baseURL: string): void {
    this.client.defaults.baseURL = baseURL;
  }

  // Method to configure global headers
  setGlobalHeader(key: string, value: string): void {
    this.client.defaults.headers.common[key] = value;
  }

  // Method to remove global headers
  removeGlobalHeader(key: string): void {
    delete this.client.defaults.headers.common[key];
  }
}

// Export a singleton instance
const httpClient = new HttpClient();
export default httpClient;

// Also export the class for cases where multiple instances are needed
export { HttpClient };
