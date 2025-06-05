import { dateToDateOnlyString } from "@/utils/dates";

export type CreationBooking = {
  property: string;
  checkIn: Date;
  checkOut: Date;
  guests: number;
  comment: string | null;
};

export type CreationBookingDto = {
  propertyId: string;
  checkInDate: string; // ISO date string
  checkOutDate: string; // ISO date string
  numGuests: number;
  comment?: string; // Optional field for comments
};

export type BookingProperty = {
  title: string;
  photoUrl: string | null;
  city: string;
  country: string;
};

export type BookingHost = {
  id: string;
  fullName: string;
};

export type BookingGuest = BookingHost & {
  email: string;
};

export type BookingStatus =
  | "pending"
  | "approved"
  | "confirmed"
  | "cancelled"
  | "completed";

export type Booking = {
  id: string;
  property: BookingProperty;
  host: BookingHost;
  guest: BookingGuest;
  checkIn: Date;
  checkOut: Date;
  guests: number;
  totalAmount: number;
  message: string | null;
  status: BookingStatus;
  rating: number | null;
};

export type BookingRoleFilter = "guest" | "host";

export type BookingSortBy = "newest" | "oldest" | "checkin" | "amount";

export type BookingFilters = {
  role?: BookingRoleFilter;
  status?: BookingStatus;
  sortBy: BookingSortBy;
};

export const creationBookingToDto = (
  booking: CreationBooking
): CreationBookingDto => {
  const dto: CreationBookingDto = {
    propertyId: booking.property,
    checkInDate: dateToDateOnlyString(booking.checkIn),
    checkOutDate: dateToDateOnlyString(booking.checkOut),
    numGuests: booking.guests,
  };
  if (booking.comment) dto.comment = booking.comment;

  return dto;
};
