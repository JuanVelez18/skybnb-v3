import httpClient from "../core/httpClient";
import type { UserSummary } from "../models/users";

export class UserService {
  public static async getUserSummary(): Promise<UserSummary> {
    const response = await httpClient.get<UserSummary>("/users/me");
    return response.data;
  }
}
