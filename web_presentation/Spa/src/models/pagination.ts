export type Page<T> = {
  results: T[];
  total: number;
  page: number;
  totalPages: number;
};

export type PaginationOptions = {
  page?: number;
  pageSize?: number;
};

export type PageDto<T> = {
  results: T[];
  page: number;
  totalPages: number;
  totalCount: number;
};

export function dtoToPage<T>(dto: PageDto<T>): Page<T> {
  return {
    results: dto.results,
    total: dto.totalCount,
    page: dto.page,
    totalPages: dto.totalPages,
  };
}
