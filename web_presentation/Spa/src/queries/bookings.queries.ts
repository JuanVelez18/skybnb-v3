import type { BookingFilters } from "@/models/bookings";
import type { PaginationOptions } from "@/models/pagination";
import { BookingService } from "@/services/booking.service";
import { useInfiniteQuery } from "@tanstack/react-query";
import { useMemo } from "react";

export const BookingsQueryKeys = {
  all: ["bookings"] as const,
  search: () => [...BookingsQueryKeys.all, "search"] as const,
  filteredSearch: (filters: BookingFilters) =>
    [...BookingsQueryKeys.search(), filters] as const,
};

export const useGetInfiniteBookings = (filters: BookingFilters) => {
  const PAGE_SIZE = 10;

  const {
    data,
    isLoading,
    isError,
    hasNextPage,
    isFetchingNextPage,
    fetchNextPage,
  } = useInfiniteQuery({
    queryKey: BookingsQueryKeys.filteredSearch(filters),
    queryFn: ({ pageParam }) => BookingService.getBookings(pageParam, filters),
    initialPageParam: { page: 1, pageSize: PAGE_SIZE } as PaginationOptions,
    getNextPageParam({ page, totalPages }): PaginationOptions | undefined {
      const nextPage = page + 1;
      if (nextPage > totalPages) return undefined;

      return { page: nextPage, pageSize: PAGE_SIZE };
    },
  });

  const bookings = useMemo(() => {
    return data?.pages.flatMap((page) => page.results) ?? [];
  }, [data]);

  return {
    bookings,
    total: data?.pages[0].total ?? 0,
    areBookingsLoading: isLoading,
    isLoadingNextBookingsPage: isFetchingNextPage,
    isBookingsError: isError,
    canLoadMoreBookings: hasNextPage && !isFetchingNextPage,
    hasMoreBookings: hasNextPage,
    fetchNextBookingsPage: fetchNextPage,
  };
};
