import { useState, forwardRef, useImperativeHandle } from "react";
import { CalendarIcon, Shield, Star } from "lucide-react";

import { Button } from "@/components/ui/button";
import { Calendar } from "@/components/ui/calendar";
import { Card, CardContent } from "@/components/ui/card";
import {
  Sheet,
  SheetContent,
  SheetDescription,
  SheetHeader,
  SheetTitle,
  SheetTrigger,
} from "@/components/ui/sheet";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Label } from "@/components/ui/label";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import { Separator } from "@/components/ui/separator";
import { Textarea } from "@/components/ui/textarea";
import { cn } from "@/lib/utils";
import { dateToLocaleString, isPastDate } from "@/utils/dates";

export type BookingFormData = {
  checkIn?: Date;
  checkOut?: Date;
  guests: string;
  comment: string;
};

export type PropertySummary = {
  id: string;
  title: string;
  location: string;
  rating: number;
  price: number;
  maxGuests: number;
  image?: string;
};

// Ref methods for external control
export type BookingSheetRef = {
  closeSheet: () => void;
  resetForm: () => void;
  openSheet: () => void;
};

type Props = {
  property: PropertySummary;
  onSubmit: (data: BookingFormData) => void;
  isLoading?: boolean;
  children: React.ReactNode;
  open?: boolean;
  onOpenChange?: (open: boolean) => void;
};

const SERVICE_FEE_PERCENTAGE = 0.1;

const BookingSheet = forwardRef<BookingSheetRef, Props>(
  (
    { property, onSubmit, isLoading = false, children, open, onOpenChange },
    ref
  ) => {
    const [internalOpen, setInternalOpen] = useState(false);
    const [formData, setFormData] = useState<BookingFormData>({
      checkIn: undefined,
      checkOut: undefined,
      guests: "1",
      comment: "",
    });

    // Use controlled state if provided, otherwise use internal state
    const isOpen = open !== undefined ? open : internalOpen;
    const setIsOpen = onOpenChange || setInternalOpen;

    // Reset form to initial state
    const resetForm = () => {
      setFormData({
        checkIn: undefined,
        checkOut: undefined,
        guests: "1",
        comment: "",
      });
    };

    // Expose methods for external control
    useImperativeHandle(
      ref,
      () => ({
        closeSheet: () => setIsOpen(false),
        resetForm,
        openSheet: () => setIsOpen(true),
      }),
      [setIsOpen]
    );

    const handleSubmit = () => {
      onSubmit(formData);
    };

    const calculateNights = () => {
      if (!formData.checkIn || !formData.checkOut) return 0;
      const checkInDate = new Date(formData.checkIn);
      const checkOutDate = new Date(formData.checkOut);
      const diffTime = Math.abs(checkOutDate.getTime() - checkInDate.getTime());
      return Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    };

    const nights = calculateNights();
    const subtotal = property.price * nights;
    const serviceFee = subtotal * SERVICE_FEE_PERCENTAGE;
    const total = subtotal + serviceFee;

    const handleOpenChange = (open: boolean) => {
      setIsOpen(open);
      if (!open) {
        resetForm();
      }
    };

    return (
      <Sheet open={isOpen} onOpenChange={handleOpenChange}>
        <SheetTrigger asChild>{children}</SheetTrigger>
        <SheetContent className="w-[400px] sm:w-[540px]">
          <SheetHeader>
            <SheetTitle className="text-lg font-semibold">
              Reserve Your Stay
            </SheetTitle>
            <SheetDescription>
              Complete the form below to send a booking request to the host.
            </SheetDescription>
          </SheetHeader>

          <div className="px-4 space-y-6">
            {/* Property Summary Card */}
            <PropertySummaryCard property={property} />

            {/* Booking Form */}
            <BookingForm
              formData={formData}
              setFormData={setFormData}
              maxGuests={property.maxGuests}
            />

            {/* Price Breakdown */}
            <PriceBreakdown
              price={property.price}
              nights={nights}
              subtotal={subtotal}
              serviceFee={serviceFee}
              total={total}
            />

            {/* Disclaimer */}
            <BookingDisclaimer />

            {/* Submit Button */}
            <Button
              onClick={handleSubmit}
              className="w-full"
              size="lg"
              disabled={isLoading || !formData.checkIn || !formData.checkOut}
            >
              {isLoading ? "Sending..." : "Send Booking Request"}
            </Button>
          </div>
        </SheetContent>
      </Sheet>
    );
  }
);

// Sub-components for better organization
const PropertySummaryCard = ({ property }: { property: PropertySummary }) => (
  <Card className="py-0">
    <CardContent className="p-4">
      <div className="flex items-center gap-3">
        <img
          src={property.image || "/placeholder.svg"}
          alt={property.title}
          className="w-16 h-16 rounded-lg object-cover"
        />
        <div>
          <h3 className="font-semibold line-clamp-1">{property.title}</h3>
          <p className="text-sm text-muted-foreground">{property.location}</p>
          <div className="flex items-center gap-1 mt-1">
            <Star className="h-3 w-3 fill-yellow-400 text-yellow-400" />
            <span className="text-sm">{property.rating}</span>
          </div>
        </div>
      </div>
    </CardContent>
  </Card>
);

const BookingForm = ({
  formData,
  setFormData,
  maxGuests,
}: {
  formData: BookingFormData;
  setFormData: React.Dispatch<React.SetStateAction<BookingFormData>>;
  maxGuests: number;
}) => (
  <div className="space-y-4">
    <div className="grid grid-cols-2 gap-4">
      <div className="space-y-2">
        <Label>Check-in</Label>
        <Popover>
          <PopoverTrigger asChild>
            <Button
              variant={"outline"}
              className={cn(
                "w-full pl-3 text-left font-normal",
                !formData.checkIn && "text-muted-foreground"
              )}
            >
              {formData.checkIn ? (
                dateToLocaleString(formData.checkIn)
              ) : (
                <span>mm/dd/yyyy</span>
              )}
              <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
            </Button>
          </PopoverTrigger>
          <PopoverContent className="w-auto p-0" align="start">
            <Calendar
              mode="single"
              selected={formData.checkIn}
              onSelect={(date) =>
                setFormData((prev) => ({
                  ...prev,
                  checkIn: date,
                }))
              }
              disabled={isPastDate}
            />
          </PopoverContent>
        </Popover>
      </div>
      <div className="space-y-2">
        <Label>Check-out</Label>
        <Popover>
          <PopoverTrigger asChild>
            <Button
              variant={"outline"}
              className={cn(
                "w-full pl-3 text-left font-normal",
                !formData.checkOut && "text-muted-foreground"
              )}
            >
              {formData.checkOut ? (
                dateToLocaleString(formData.checkOut)
              ) : (
                <span>mm/dd/yyyy</span>
              )}
              <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
            </Button>
          </PopoverTrigger>
          <PopoverContent className="w-auto p-0" align="start">
            <Calendar
              mode="single"
              selected={formData.checkOut}
              onSelect={(date) =>
                setFormData((prev) => ({
                  ...prev,
                  checkOut: date,
                }))
              }
              disabled={(date) =>
                isPastDate(date) ||
                !formData?.checkIn ||
                date < formData.checkIn
              }
            />
          </PopoverContent>
        </Popover>
      </div>
    </div>

    <div className="space-y-2">
      <Label htmlFor="guests">Guests</Label>
      <Select
        value={formData.guests}
        onValueChange={(value) =>
          setFormData((prev) => ({ ...prev, guests: value }))
        }
      >
        <SelectTrigger className="w-full">
          <SelectValue />
        </SelectTrigger>
        <SelectContent>
          {Array.from({ length: maxGuests }, (_, i) => i + 1).map((num) => (
            <SelectItem key={num} value={num.toString()}>
              {num} {num === 1 ? "guest" : "guests"}
            </SelectItem>
          ))}
        </SelectContent>
      </Select>
    </div>

    <div className="space-y-2">
      <Label htmlFor="comment">Message to host (Optional)</Label>
      <Textarea
        id="comment"
        placeholder="Tell the host about your trip, who's coming, or anything else you'd like them to know..."
        value={formData.comment}
        onChange={(e) =>
          setFormData((prev) => ({ ...prev, comment: e.target.value }))
        }
        className="min-h-[100px]"
      />
    </div>
  </div>
);

const PriceBreakdown = ({
  price,
  nights,
  subtotal,
  serviceFee,
  total,
}: {
  price: number;
  nights: number;
  subtotal: number;
  serviceFee: number;
  total: number;
}) => (
  <Card>
    <CardContent className="px-4 space-y-2">
      {nights > 0 && (
        <>
          <div className="flex justify-between">
            <span>
              ${price} × {nights} night{nights !== 1 ? "s" : ""}
            </span>
            <span>${subtotal}</span>
          </div>
          <div className="flex justify-between">
            <span>Service fee ({SERVICE_FEE_PERCENTAGE * 100}%)</span>
            <span>${serviceFee}</span>
          </div>
          <Separator />
          <div className="flex justify-between font-semibold">
            <span>Total</span>
            <span>${total}</span>
          </div>
        </>
      )}
      {nights === 0 && (
        <div className="text-center text-muted-foreground">
          Select dates to see pricing
        </div>
      )}
    </CardContent>
  </Card>
);

const BookingDisclaimer = () => (
  <div className="bg-muted/50 p-4 rounded-lg">
    <div className="flex items-start gap-2">
      <Shield className="h-5 w-5 text-primary mt-0.5" />
      <div className="space-y-1">
        <p className="text-sm font-medium">Booking Request</p>
        <p className="text-xs text-muted-foreground">
          This is a booking request. The host will review your request and
          respond within 24 hours. You won't be charged until your request is
          approved.
        </p>
      </div>
    </div>
  </div>
);

// Add display name for better debugging
BookingSheet.displayName = "BookingSheet";

export default BookingSheet;
