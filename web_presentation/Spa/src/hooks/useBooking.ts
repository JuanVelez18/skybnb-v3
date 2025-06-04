import { useState } from "react";
import { toast } from "sonner";

import type { BookingFormData } from "@/components/BookingSheet";

const DAY_IN_MS = 1000 * 60 * 60 * 24;

export const useBooking = (onSuccess?: () => void) => {
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

      // Calculate nights and pricing
      const nights = Math.ceil(
        (checkOutDate.getTime() - checkInDate.getTime()) / DAY_IN_MS
      );

      // Here you would typically call your booking API
      const bookingData = {
        propertyId,
        checkInDate: formData.checkIn,
        checkOutDate: formData.checkOut,
        numGuests: parseInt(formData.guests),
        comment: formData.comment,
        nights,
      };

      // TODO: Replace with actual API call
      await new Promise((resolve) => setTimeout(resolve, 2000));
      console.log("Booking request submitted:", bookingData);

      // Show success message
      toast.success("Booking request sent successfully!", {
        description:
          "The host will review your request and respond within 24 hours.",
      });

      // Call success callback if provided
      onSuccess?.();

      return true;
    } catch (error) {
      const message =
        error instanceof Error
          ? error.message
          : "Failed to submit booking request";
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
