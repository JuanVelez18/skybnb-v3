import httpClient from "@/core/httpClient";
import {
  creationBookingToDto,
  type CreationBooking,
  type CreationBookingDto,
} from "@/models/bookings";

export class BookingService {
  public static async createBooking(booking: CreationBooking): Promise<void> {
    const bookingDto: CreationBookingDto = creationBookingToDto(booking);
    await httpClient.post("/bookings", bookingDto);
  }
}
