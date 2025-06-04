import { dtoToTokens, type Tokens, type RazorTokensDto } from "../models/auth";

export const TOKENS_KEY = "tokens";

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

export const getTokensFromRazor = (): Tokens | null => {
  const tokensTag = document.getElementById(TOKENS_KEY);
  if (!tokensTag) return null;

  tokensTag.remove();

  const tokensDto = JSON.parse(tokensTag.textContent!.trim()) as RazorTokensDto;
  const tokens = dtoToTokens(tokensDto);

  return tokens;
};
