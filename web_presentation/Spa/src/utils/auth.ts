import httpClient from "@/core/httpClient";
import { useAuthStore } from "@/stores/auth.store";
import { getTokensFromRazor, getTokensFromStorage } from "./tokens";
import type { UserSummary } from "@/models/users";
import { queryClient } from "@/core/queryClient";

export const initializeSession = () => {
  const tokens = getTokensFromStorage() ?? getTokensFromRazor();
  if (!tokens) return;

  useAuthStore.getState().authenticate(tokens);
};

export const logout = async () => {
  await httpClient.post("/auth/logout", {
    RefreshToken: useAuthStore.getState().refreshToken,
  });

  useAuthStore.getState().clearAuthentication();
  queryClient.clear();
};

export const hasPermission = (
  user: UserSummary,
  permission: string
): boolean => {
  return user.permissions.some(
    (userPermission) => userPermission === permission
  );
};
