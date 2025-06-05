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
