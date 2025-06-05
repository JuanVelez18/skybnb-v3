export type PaymentCreation = {
  bookingId: string;
  method: string;
};

export type PaymentCreationDto = {
  bookingId: string;
  paymentMethod: string;
};

export const paymentCreationToDto = (
  payment: PaymentCreation
): PaymentCreationDto => {
  return {
    bookingId: payment.bookingId,
    paymentMethod: payment.method,
  };
};
