import {
  useInfiniteQuery,
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";
import { useMemo } from "react";
import { toast } from "sonner";

import { ApiError } from "@/core/httpClient";
import type { BookingFilters } from "@/models/bookings";
import type { PaginationOptions } from "@/models/pagination";
import { BookingService } from "@/services/booking.service";

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

export const useApproveBooking = () => {
  const queryClient = useQueryClient();

  const { mutate, isPending } = useMutation({
    mutationFn: BookingService.aproveBooking,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: BookingsQueryKeys.search() });
      toast.success("Booking approved successfully!");
    },
    onError(error) {
      let meessage = "An error occurred while approving the booking.";
      // @ts-expect-error The error type is not always ApiError
      if (error instanceof ApiError && error.status && error.status < 500) {
        meessage =
          error.message || "An error occurred while processing your request";
      }

      toast.error(meessage);
    },
  });

  return {
    approveBooking: mutate,
    isApprovingBooking: isPending,
  };
};

export const useCancelBooking = () => {
  const queryClient = useQueryClient();

  const { mutate, isPending } = useMutation({
    mutationFn: BookingService.cancelBooking,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: BookingsQueryKeys.search() });
      toast.success("Booking cancelled successfully!");
    },
    onError(error) {
      let message = "An error occurred while cancelling the booking.";
      // @ts-expect-error The error type is not always ApiError
      if (error instanceof ApiError && error.status && error.status < 500) {
        message =
          error.message || "An error occurred while processing your request";
      }

      toast.error(message);
    },
  });

  return {
    cancelBooking: mutate,
    isCancellingBooking: isPending,
  };
};
