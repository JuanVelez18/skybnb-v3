import { dtoToTokens, type Tokens, type RazorTokensDto } from "../models/auth";

const TOKENS_KEY = "tokens";
const LOGOUT_URL = "/Logout";

export const getTokensFromStorage = (): Tokens | null => {
  const tokens = localStorage.getItem(TOKENS_KEY);
  if (!tokens) return null;

  try {
    return JSON.parse(tokens) as Tokens;
  } catch (error) {
    console.error("Failed to parse tokens from localStorage", error);
    return null;
  }
};

const getTokensFromRazor = (): Tokens | null => {
  const tokensTag = document.getElementById(TOKENS_KEY);
  if (!tokensTag) return null;

  tokensTag.remove();

  const tokensDto = JSON.parse(tokensTag.textContent!.trim()) as RazorTokensDto;
  const tokens = dtoToTokens(tokensDto);

  return tokens;
};

export const saveTokens = (tokens: Tokens) => {
  localStorage.setItem(TOKENS_KEY, JSON.stringify(tokens));
};

export const redirectToLogout = () => {
  const anchor = document.createElement("a");
  anchor.href = LOGOUT_URL;
  anchor.click();
};

export const initializeSession = async () => {
  const localStorageTokens = getTokensFromStorage();
  if (localStorageTokens) return;

  const razorTokens = getTokensFromRazor();
  if (razorTokens) {
    saveTokens(razorTokens);
    return;
  }

  redirectToLogout();
};

export const logout = () => {
  localStorage.removeItem(TOKENS_KEY);
  redirectToLogout();
};
