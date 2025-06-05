import {
  AlertCircle,
  CheckCircle,
  DollarSign,
  MapPin,
  MoreHorizontal,
  Star,
  XCircle,
} from "lucide-react";

import {
  AlertDialog,
  AlertDialogContent,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogDescription,
  AlertDialogTrigger,
  AlertDialogAction,
  AlertDialogCancel,
} from "@/components/ui/alert-dialog";
import { Avatar, AvatarImage, AvatarFallback } from "@/components/ui/avatar";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Card, CardContent } from "@/components/ui/card";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import type { Booking, BookingStatus } from "@/models/bookings";
import { bookingStatuses } from "@/utils/bookings";

type Props = {
  booking: Booking;
  userId: string;
  onApprove: () => void;
  onPayment: () => void;
  onCancel: () => void;
};

const BookingCard = ({
  booking,
  userId,
  onApprove,
  onPayment,
  onCancel,
}: Props) => {
  const isGuest = booking.guest.id === userId;

  const canApprove = !isGuest && booking.status === "pending";
  const canPay = isGuest && booking.status === "approved";
  const canCancel =
    booking.status !== "completed" && booking.status !== "cancelled";

  return (
    <Card className="hover:shadow-md transition-shadow">
      <CardContent className="p-6">
        <div className="flex gap-4">
          {/* Property Image */}
          <div className="flex-shrink-0">
            <img
              src={booking.property.photoUrl || "/placeholder.svg"}
              alt={booking.property.title}
              className="w-24 h-24 rounded-lg object-cover"
            />
          </div>

          {/* Booking Details */}
          <div className="flex-1 space-y-3">
            <div className="flex justify-between items-start">
              <div>
                <h3 className="font-semibold text-lg">
                  {booking.property.title}
                </h3>
                <p className="text-sm text-muted-foreground flex items-center gap-1">
                  <MapPin className="h-3 w-3" />
                  {booking.property.city}, {booking.property.country}
                </p>
              </div>
              <div className="flex items-center gap-2">
                <StatusBadge
                  status={booking.status as keyof typeof bookingStatuses}
                />
                {canCancel && (
                  <DropdownMenu>
                    <DropdownMenuTrigger asChild>
                      <Button variant="ghost" size="icon" className="h-8 w-8">
                        <MoreHorizontal className="h-4 w-4" />
                      </Button>
                    </DropdownMenuTrigger>
                    <DropdownMenuContent align="end">
                      <AlertDialog>
                        <AlertDialogTrigger asChild>
                          <DropdownMenuItem
                            onSelect={(e) => e.preventDefault()}
                          >
                            <XCircle className="h-4 w-4 mr-2" />
                            Cancel Booking
                          </DropdownMenuItem>
                        </AlertDialogTrigger>
                        <AlertDialogContent>
                          <AlertDialogHeader>
                            <AlertDialogTitle>Cancel Booking</AlertDialogTitle>
                            <AlertDialogDescription>
                              Are you sure you want to cancel this booking? This
                              action cannot be undone.
                            </AlertDialogDescription>
                          </AlertDialogHeader>
                          <AlertDialogFooter>
                            <AlertDialogCancel>Keep Booking</AlertDialogCancel>
                            <AlertDialogAction
                              onClick={onCancel}
                              className="bg-red-600 hover:bg-red-700"
                            >
                              Cancel Booking
                            </AlertDialogAction>
                          </AlertDialogFooter>
                        </AlertDialogContent>
                      </AlertDialog>
                    </DropdownMenuContent>
                  </DropdownMenu>
                )}
              </div>
            </div>

            {/* Guest/Host Info */}
            <div className="flex items-center gap-3">
              <Avatar className="h-8 w-8">
                <AvatarImage
                  src="/user-placeholder.svg"
                  alt={isGuest ? booking.host.fullName : booking.guest.fullName}
                />
                <AvatarFallback>
                  {isGuest
                    ? booking.host.fullName.charAt(0)
                    : booking.guest.fullName.charAt(0)}
                </AvatarFallback>
              </Avatar>
              <div>
                <p className="text-sm font-medium">
                  {isGuest
                    ? `Host: ${booking.host.fullName}`
                    : `Guest: ${booking.guest.fullName}`}
                </p>
                <p className="text-xs text-muted-foreground">
                  {isGuest ? "Property owner" : booking.guest.email}
                </p>
              </div>
            </div>

            {/* Booking Info */}
            <div className="grid grid-cols-2 md:grid-cols-4 gap-4 text-sm">
              <div>
                <p className="text-muted-foreground">Check-in</p>
                <p className="font-medium">
                  {new Date(booking.checkIn).toLocaleDateString()}
                </p>
              </div>
              <div>
                <p className="text-muted-foreground">Check-out</p>
                <p className="font-medium">
                  {new Date(booking.checkOut).toLocaleDateString()}
                </p>
              </div>
              <div>
                <p className="text-muted-foreground">Guests</p>
                <p className="font-medium">{booking.guests}</p>
              </div>
              <div>
                <p className="text-muted-foreground">Total</p>
                <p className="font-medium">${booking.totalAmount}</p>
              </div>
            </div>

            {/* Status-specific content */}
            {booking.status === "pending" && (
              <div className="bg-yellow-50 border border-yellow-200 rounded-lg p-3">
                <div className="flex items-start gap-2">
                  <AlertCircle className="h-4 w-4 text-yellow-600 mt-0.5" />
                  <div className="flex-1">
                    <p className="text-sm font-medium text-yellow-800">
                      {isGuest
                        ? "Waiting for host approval"
                        : "Approval required"}
                    </p>
                    <p className="text-xs text-yellow-700 mt-1">
                      {isGuest
                        ? "The host will review your request and respond within 24 hours."
                        : "Review this booking request and approve or decline."}
                    </p>
                  </div>
                </div>
              </div>
            )}

            {booking.status === "approved" && (
              <div className="bg-blue-50 border border-blue-200 rounded-lg p-3">
                <div className="flex items-start gap-2">
                  <CheckCircle className="h-4 w-4 text-blue-600 mt-0.5" />
                  <div className="flex-1">
                    <p className="text-sm font-medium text-blue-800">
                      {isGuest ? "Ready for payment" : "Waiting for payment"}
                    </p>
                    <p className="text-xs text-blue-700 mt-1">
                      {isGuest
                        ? "Your booking has been approved! Complete payment to confirm your reservation."
                        : "The guest can now proceed with payment to confirm the booking."}
                    </p>
                  </div>
                </div>
              </div>
            )}

            {booking.status === "confirmed" && (
              <div className="bg-green-50 border border-green-200 rounded-lg p-3">
                <div className="flex items-start gap-2">
                  <CheckCircle className="h-4 w-4 text-green-600 mt-0.5" />
                  <div className="flex-1">
                    <p className="text-sm font-medium text-green-800">
                      Booking confirmed
                    </p>
                    <p className="text-xs text-green-700 mt-1">
                      Payment completed.{" "}
                      {isGuest
                        ? "Enjoy your stay!"
                        : "Prepare for your guest's arrival."}
                    </p>
                  </div>
                </div>
              </div>
            )}

            {booking.status === "cancelled" && (
              <div className="bg-red-50 border border-red-200 rounded-lg p-3">
                <div className="flex items-start gap-2">
                  <XCircle className="h-4 w-4 text-red-600 mt-0.5" />
                  <div className="flex-1">
                    <p className="text-sm font-medium text-red-800">
                      Booking cancelled
                    </p>
                  </div>
                </div>
              </div>
            )}

            {booking.status === "completed" && (
              <div className="bg-gray-50 border border-gray-200 rounded-lg p-3">
                <div className="flex items-start gap-2">
                  <CheckCircle className="h-4 w-4 text-gray-600 mt-0.5" />
                  <div className="flex-1">
                    <p className="text-sm font-medium text-gray-800">
                      Stay completed
                    </p>
                    {booking.rating && (
                      <div className="flex items-center gap-1 mt-1">
                        <div className="flex">
                          {Array.from({ length: 5 }).map((_, i) => (
                            <Star
                              key={i}
                              className={`h-3 w-3 ${
                                i < booking.rating!
                                  ? "fill-yellow-400 text-yellow-400"
                                  : "text-gray-300"
                              }`}
                            />
                          ))}
                        </div>
                        <span className="text-xs text-gray-600">
                          ({booking.rating}/5)
                        </span>
                      </div>
                    )}
                  </div>
                </div>
              </div>
            )}

            {/* Guest Message */}
            {booking.message && (
              <div className="bg-muted/50 rounded-lg p-3">
                <p className="text-sm text-muted-foreground">
                  <span className="font-medium">
                    {isGuest
                      ? "Your message:"
                      : `Message from ${booking.guest.fullName}:`}
                  </span>
                </p>
                <p className="text-sm mt-1">"{booking.message}"</p>
              </div>
            )}

            {/* Action Buttons */}
            <div className="flex gap-2 pt-2">
              {canApprove && (
                <Button onClick={onApprove} size="sm">
                  <CheckCircle className="h-4 w-4 mr-2" />
                  Approve Booking
                </Button>
              )}
              {canPay && (
                <Button onClick={onPayment} size="sm">
                  <DollarSign className="h-4 w-4 mr-2" />
                  Complete Payment
                </Button>
              )}
            </div>
          </div>
        </div>
      </CardContent>
    </Card>
  );
};

const StatusBadge = ({ status }: { status: BookingStatus }) => {
  const statusConfig = bookingStatuses[status];
  const Icon = statusConfig.icon;

  return (
    <Badge variant="outline" className={`${statusConfig.color} border`}>
      <Icon className="h-3 w-3 mr-1" />
      {statusConfig.label}
    </Badge>
  );
};

export default BookingCard;
