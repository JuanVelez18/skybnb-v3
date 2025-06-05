import httpClient from "@/core/httpClient";
import {
  creationBookingToDto,
  dtoToBooking,
  type Booking,
  type BookingDto,
  type BookingFilters,
  type CreationBooking,
  type CreationBookingDto,
} from "@/models/bookings";
import type { Page, PageDto, PaginationOptions } from "@/models/pagination";

export class BookingService {
  public static async createBooking(booking: CreationBooking): Promise<void> {
    const bookingDto: CreationBookingDto = creationBookingToDto(booking);
    await httpClient.post("/bookings", bookingDto);
  }

  public static async getBookings(
    pagination: PaginationOptions,
    filters?: BookingFilters
  ): Promise<Page<Booking>> {
    const params = {
      ...pagination,
      ...(filters ?? {}),
    };

    const response = await httpClient.get<PageDto<BookingDto>>("/bookings", {
      params,
    });

    return {
      page: response.data.page,
      total: response.data.totalCount,
      totalPages: response.data.totalPages,
      results: response.data.results.map(dtoToBooking),
    };
  }
}
