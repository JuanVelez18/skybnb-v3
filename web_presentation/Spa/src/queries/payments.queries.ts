import { useMutation, useQueryClient } from "@tanstack/react-query";

import { PaymentService } from "@/services/payment.service";
import { BookingsQueryKeys } from "./bookings.queries";
import { toast } from "sonner";

export const useCreatePayment = () => {
  const queryClient = useQueryClient();
  const { mutate, isPending } = useMutation({
    mutationFn: PaymentService.createPayment,
    onSuccess() {
      queryClient.invalidateQueries({
        queryKey: BookingsQueryKeys.search(),
      });

      toast.success("Payment created successfully");
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
    createPayment: mutate,
    isPaymentLoading: isPending,
  };
};
