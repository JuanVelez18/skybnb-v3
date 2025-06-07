import httpClient from "@/core/httpClient";
import {
  paymentCreationToDto,
  type PaymentCreation,
  type PaymentCreationDto,
} from "@/models/payments";

export class PaymentService {
  public static async createPayment(payment: PaymentCreation): Promise<void> {
    const dto: PaymentCreationDto = paymentCreationToDto(payment);
    await httpClient.post("/payments", dto);
  }
}
