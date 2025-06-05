import { CreditCard, DollarSign, Shield } from "lucide-react";
import { useState } from "react";

import { Button } from "@/components/ui/button";
import { Card, CardContent } from "@/components/ui/card";
import {
  Dialog,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogDescription,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import type { Booking } from "@/models/bookings";
import { Separator } from "@/components/ui/separator";
import { useCreatePayment } from "@/queries/payments.queries";
import type { PaymentCreation } from "@/models/payments";

type Props = {
  isOpen: boolean;
  booking?: Booking | null;
  onClose: () => void;
};

const PaymentForm = ({ isOpen, booking, onClose }: Props) => {
  const { createPayment, isPaymentLoading } = useCreatePayment();

  const [paymentMethod, setPaymentMethod] = useState("credit-card");
  const [cardNumber, setCardNumber] = useState("");
  const [expiryDate, setExpiryDate] = useState("");
  const [cvv, setCvv] = useState("");
  const [cardName, setCardName] = useState("");

  const resetForm = () => {
    setPaymentMethod("credit-card");
    setCardNumber("");
    setExpiryDate("");
    setCvv("");
    setCardName("");
  };

  const processPayment = () => {
    if (!booking) return;

    const paymentData: PaymentCreation = {
      bookingId: booking.id,
      method: paymentMethod,
    };

    createPayment(paymentData, {
      onSuccess: () => {
        resetForm();
        onClose();
      },
    });
  };

  return (
    <Dialog open={isOpen} onOpenChange={onClose}>
      <DialogContent className="sm:max-w-md">
        <DialogHeader>
          <DialogTitle>Complete Payment</DialogTitle>
          <DialogDescription>
            {booking && (
              <>Complete your payment for booking {booking.property.title}</>
            )}
          </DialogDescription>
        </DialogHeader>

        <div className="space-y-6">
          {/* Booking Summary */}
          {booking && (
            <Card>
              <CardContent className="p-4">
                <div className="space-y-2">
                  <div className="flex justify-between">
                    <span className="text-sm">Property:</span>
                    <span className="text-sm font-medium">
                      {booking.property.title}
                    </span>
                  </div>
                  <div className="flex justify-between">
                    <span className="text-sm">Dates:</span>
                    <span className="text-sm font-medium">
                      {new Date(booking.checkIn).toLocaleDateString()} -{" "}
                      {new Date(booking.checkOut).toLocaleDateString()}
                    </span>
                  </div>
                  <div className="flex justify-between">
                    <span className="text-sm">Guests:</span>
                    <span className="text-sm font-medium">
                      {booking.guests}
                    </span>
                  </div>
                  <Separator />
                  <div className="flex justify-between font-semibold">
                    <span>Total Amount:</span>
                    <span>${booking.totalAmount}</span>
                  </div>
                </div>
              </CardContent>
            </Card>
          )}

          {/* Payment Method Selection */}
          <div className="space-y-3">
            <Label>Payment Method</Label>
            <div className="grid grid-cols-2 gap-3">
              <Button
                variant={
                  paymentMethod === "credit-card" ? "default" : "outline"
                }
                onClick={() => setPaymentMethod("credit-card")}
                className="h-12"
              >
                <CreditCard className="h-4 w-4 mr-2" />
                Credit Card
              </Button>
              <Button
                variant={paymentMethod === "debit-card" ? "default" : "outline"}
                onClick={() => setPaymentMethod("debit-card")}
                className="h-12"
              >
                <CreditCard className="h-4 w-4 mr-2" />
                Debit Card
              </Button>
            </div>
          </div>

          {/* Card Details Form */}
          <div className="space-y-4">
            <div className="space-y-2">
              <Label htmlFor="cardNumber">Card Number</Label>
              <Input
                id="cardNumber"
                placeholder="1234 5678 9012 3456"
                value={cardNumber}
                onChange={(e) => {
                  // Format card number with spaces
                  const value = e.target.value
                    .replace(/\s/g, "")
                    .replace(/(.{4})/g, "$1 ")
                    .trim();
                  if (value.length <= 19) setCardNumber(value);
                }}
                maxLength={19}
              />
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="expiryDate">Expiry Date</Label>
                <Input
                  id="expiryDate"
                  placeholder="MM/YY"
                  value={expiryDate}
                  onChange={(e) => {
                    // Format expiry date
                    const value = e.target.value
                      .replace(/\D/g, "")
                      .replace(/(\d{2})(\d)/, "$1/$2");
                    if (value.length <= 5) setExpiryDate(value);
                  }}
                  maxLength={5}
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="cvv">CVV</Label>
                <Input
                  id="cvv"
                  placeholder="123"
                  value={cvv}
                  onChange={(e) => {
                    const value = e.target.value.replace(/\D/g, "");
                    if (value.length <= 3) setCvv(value);
                  }}
                  maxLength={3}
                />
              </div>
            </div>

            <div className="space-y-2">
              <Label htmlFor="cardName">Cardholder Name</Label>
              <Input
                id="cardName"
                placeholder="John Doe"
                value={cardName}
                onChange={(e) => setCardName(e.target.value)}
              />
            </div>
          </div>

          {/* Security Notice */}
          <div className="bg-muted/50 p-3 rounded-lg">
            <div className="flex items-start gap-2">
              <Shield className="h-4 w-4 text-primary mt-0.5" />
              <div className="text-xs text-muted-foreground">
                <p className="font-medium">Secure Payment</p>
                <p>
                  Your payment information is encrypted and secure. This is a
                  demo environment.
                </p>
              </div>
            </div>
          </div>
        </div>

        <DialogFooter className="flex gap-2">
          <Button
            variant="outline"
            onClick={onClose}
            disabled={isPaymentLoading}
          >
            Cancel
          </Button>
          <Button
            onClick={processPayment}
            disabled={
              !cardNumber ||
              !expiryDate ||
              !cvv ||
              !cardName ||
              isPaymentLoading
            }
            className="min-w-[120px]"
          >
            {isPaymentLoading ? (
              <>
                <div className="animate-spin rounded-full h-4 w-4 border-b-2 border-white mr-2"></div>
                Processing...
              </>
            ) : (
              <>
                <DollarSign className="h-4 w-4 mr-2" />
                Pay Now
              </>
            )}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};

export default PaymentForm;
