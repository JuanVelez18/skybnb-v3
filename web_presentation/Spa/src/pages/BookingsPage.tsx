import { useEffect, useRef, useState } from "react";
import { Calendar } from "lucide-react";

import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import type {
  Booking,
  BookingFilters,
  BookingRoleFilter,
  BookingSortBy,
  BookingStatus,
} from "@/models/bookings";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Tabs, TabsList, TabsTrigger } from "@/components/ui/tabs";
import BookingCard from "@/components/cards/BookingCard";
import { useGetUserSummary } from "@/queries/users.queries";
import {
  useApproveBooking,
  useCancelBooking,
  useGetInfiniteBookings,
} from "@/queries/bookings.queries";
import { Skeleton } from "@/components/ui/skeleton";
import { Link } from "react-router-dom";
import { RouteNames } from "@/router/routes";

const BookingsPage = () => {
  const { user } = useGetUserSummary();

  const [bookingId, setBookingId] = useState<string | null>(null);
  const [filters, setFilters] = useState<BookingFilters>({
    role: undefined,
    status: undefined,
    sortBy: "newest",
  });

  const {
    bookings,
    areBookingsLoading,
    isLoadingNextBookingsPage,
    hasMoreBookings,
    fetchNextBookingsPage,
  } = useGetInfiniteBookings(filters);
  const { approveBooking } = useApproveBooking();
  const { cancelBooking } = useCancelBooking();

  const loadMoreRef = useRef<HTMLDivElement | null>(null);
  const showSkeleton =
    areBookingsLoading || isLoadingNextBookingsPage || hasMoreBookings;

  const handleTabChange = (value: string) => {
    setFilters((prev) => ({
      ...prev,
      role: (value as BookingRoleFilter) || undefined,
    }));
  };

  const handleStatusChange = (value: string) => {
    setFilters((prev) => ({
      ...prev,
      status: (value as BookingStatus) || undefined,
    }));
  };

  const handleSortChange = (value: string) => {
    setFilters((prev) => ({
      ...prev,
      sortBy: (value as BookingSortBy) || "newest",
    }));
  };

  const handleApprove = (booking: Booking) => {
    setBookingId(booking.id);

    approveBooking(booking.id, {
      onSettled: () => {
        setBookingId(null);
      },
    });
  };

  const handlePayment = (booking: Booking) => {
    // TODO: Implement payment processing logic
    console.log(`Processing payment for booking ${booking.id}`);
  };

  const handleCancel = (booking: Booking) => {
    setBookingId(booking.id);

    cancelBooking(booking.id, {
      onSettled: () => {
        setBookingId(null);
      },
    });
  };

  useEffect(() => {
    const target = loadMoreRef.current;

    if (!target || !hasMoreBookings) return;

    const observer = new IntersectionObserver(
      (entries) => {
        if (
          entries[0].isIntersecting &&
          !areBookingsLoading &&
          !isLoadingNextBookingsPage
        ) {
          fetchNextBookingsPage();
        }
      },
      {
        root: null,
        rootMargin: "30px 0px",
      }
    );

    observer.observe(target);

    return () => observer.unobserve(target);
  }, [
    areBookingsLoading,
    hasMoreBookings,
    isLoadingNextBookingsPage,
    fetchNextBookingsPage,
  ]);

  return (
    <div className="max-w-6xl mx-auto space-y-6">
      <div className="space-y-4">
        <Tabs value={filters.role ?? "all"} onValueChange={handleTabChange}>
          <TabsList className="grid w-full grid-cols-3">
            <TabsTrigger value="all">All Bookings</TabsTrigger>
            <TabsTrigger value="guest">As Guest</TabsTrigger>
            <TabsTrigger value="host">As Host</TabsTrigger>
          </TabsList>
        </Tabs>

        <div className="flex gap-4">
          <Select
            value={filters.status ?? "all"}
            onValueChange={handleStatusChange}
          >
            <SelectTrigger className="w-48">
              <SelectValue placeholder="Filter by status" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="all">All Statuses</SelectItem>
              <SelectItem value="pending">Pending</SelectItem>
              <SelectItem value="approved">Approved</SelectItem>
              <SelectItem value="confirmed">Confirmed</SelectItem>
              <SelectItem value="cancelled">Cancelled</SelectItem>
              <SelectItem value="completed">Completed</SelectItem>
            </SelectContent>
          </Select>

          <Select value={filters.sortBy} onValueChange={handleSortChange}>
            <SelectTrigger className="w-48">
              <SelectValue placeholder="Sort by" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="newest">Newest First</SelectItem>
              <SelectItem value="oldest">Oldest First</SelectItem>
              <SelectItem value="checkin">Check-in Date</SelectItem>
              <SelectItem value="amount">Total Amount</SelectItem>
            </SelectContent>
          </Select>
        </div>
      </div>

      <div className="space-y-4">
        {!areBookingsLoading && (
          <>
            {bookings.length > 0 ? (
              bookings.map((booking) => (
                <BookingCard
                  key={booking.id}
                  booking={booking}
                  userId={user?.id ?? ""}
                  isLoading={bookingId === booking.id}
                  onApprove={() => handleApprove(booking)}
                  onPayment={() => handlePayment(booking)}
                  onCancel={() => handleCancel(booking)}
                />
              ))
            ) : (
              <Card>
                <CardContent className="flex flex-col items-center justify-center py-12 text-center">
                  <Calendar className="h-12 w-12 text-muted-foreground mb-4" />
                  <h3 className="text-lg font-semibold mb-2">
                    No bookings found
                  </h3>
                  <p className="text-muted-foreground mb-4">
                    {filters.role === "guest"
                      ? "You haven't made any bookings yet."
                      : filters.role === "host"
                      ? "You haven't received any bookings yet."
                      : "No bookings match your current filters."}
                  </p>
                  <Button asChild>
                    <Link
                      to={
                        filters.role === "guest"
                          ? RouteNames.HOME
                          : RouteNames.CREATE_PROPERTY
                      }
                    >
                      {filters.role === "guest"
                        ? "Start Exploring"
                        : "List Your Property"}
                    </Link>
                  </Button>
                </CardContent>
              </Card>
            )}
          </>
        )}

        {showSkeleton && <Skeleton className="h-72 w-full" ref={loadMoreRef} />}
      </div>
    </div>
  );
};

export default BookingsPage;
