export type Tokens = {
  accessToken: string;
  refreshToken: string;
};

export type TokensDto = {
  AccessToken: string;
  RefreshToken: string;
};

export const dtoToTokens = (dto: TokensDto): Tokens => {
  return {
    accessToken: dto.AccessToken,
    refreshToken: dto.RefreshToken,
  };
};
