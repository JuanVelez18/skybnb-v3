import { useState } from "react";
import { toast } from "sonner";
import { useQueryClient } from "@tanstack/react-query";

import type { BookingFormData } from "@/components/BookingSheet";
import type { CreationBooking } from "@/models/bookings";
import { BookingService } from "@/services/booking.service";
import { ApiError } from "@/core/httpClient";
import { BookingsQueryKeys } from "@/queries/bookings.queries";

export const useBooking = (onSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const [isLoading, setIsLoading] = useState(false);

  const submitBooking = async (
    propertyId: string,
    formData: BookingFormData
  ) => {
    setIsLoading(true);

    try {
      // Validate form data
      if (!formData.checkIn || !formData.checkOut) {
        throw new Error("Please select check-in and check-out dates");
      }

      const checkInDate = new Date(formData.checkIn);
      const checkOutDate = new Date(formData.checkOut);

      if (checkInDate >= checkOutDate) {
        throw new Error("Check-out date must be after check-in date");
      }

      if (checkInDate < new Date()) {
        throw new Error("Check-in date cannot be in the past");
      }

      const booking: CreationBooking = {
        property: propertyId,
        checkIn: checkInDate,
        checkOut: checkOutDate,
        guests: parseInt(formData.guests),
        comment: formData.comment || null,
      };
      await BookingService.createBooking(booking);

      queryClient.invalidateQueries({
        queryKey: BookingsQueryKeys.search(),
      });

      // Show success message
      toast.success("Booking request sent successfully!", {
        description:
          "The host will review your request and respond within 24 hours.",
      });

      // Call success callback if provided
      onSuccess?.();

      return true;
    } catch (error) {
      let message = "Failed to submit booking request";
      if (error instanceof ApiError && error.status && error.status < 500) {
        message =
          error.message || "An error occurred while processing your request";
      } else if (error instanceof Error) {
        message = error.message;
      }

      toast.error(message);
      throw error;
    } finally {
      setIsLoading(false);
    }
  };

  return {
    isLoading,
    submitBooking,
  };
};
