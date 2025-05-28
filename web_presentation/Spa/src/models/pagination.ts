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
