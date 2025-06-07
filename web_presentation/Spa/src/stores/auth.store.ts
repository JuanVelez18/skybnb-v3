import type { Tokens } from "@/models/auth";
import { TOKENS_KEY } from "@/utils/tokens";
import { create } from "zustand";

type State = {
  isAuthenticated: boolean;
  accessToken: string | null;
  refreshToken: string | null;
};

type Action = {
  authenticate: (tokens: Tokens) => void;
  clearAuthentication: () => void;
};

export const useAuthStore = create<State & Action>((set) => ({
  isAuthenticated: false,
  accessToken: null,
  refreshToken: null,

  authenticate(tokens) {
    localStorage.setItem(TOKENS_KEY, JSON.stringify(tokens));

    set(() => ({
      isAuthenticated: true,
      accessToken: tokens.accessToken,
      refreshToken: tokens.refreshToken,
    }));
  },

  clearAuthentication() {
    localStorage.removeItem(TOKENS_KEY);

    set(() => ({
      isAuthenticated: false,
      accessToken: null,
      refreshToken: null,
    }));
  },
}));
