export type Tokens = {
  accessToken: string;
  refreshToken: string;
};

export type RazorTokensDto = {
  AccessToken: string;
  RefreshToken: string;
};

export const dtoToTokens = (dto: RazorTokensDto): Tokens => {
  return {
    accessToken: dto.AccessToken,
    refreshToken: dto.RefreshToken,
  };
};
