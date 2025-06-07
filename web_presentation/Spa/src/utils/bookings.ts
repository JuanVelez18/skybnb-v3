import { CheckCircle, Clock, XCircle } from "lucide-react";

export const bookingStatuses = {
  pending: {
    label: "Pending",
    color: "bg-yellow-100 text-yellow-800 border-yellow-200",
    icon: Clock,
    description: "Waiting for host approval",
  },
  approved: {
    label: "Approved",
    color: "bg-blue-100 text-blue-800 border-blue-200",
    icon: CheckCircle,
    description: "Ready for payment",
  },
  confirmed: {
    label: "Confirmed",
    color: "bg-green-100 text-green-800 border-green-200",
    icon: CheckCircle,
    description: "Payment completed",
  },
  cancelled: {
    label: "Cancelled",
    color: "bg-red-100 text-red-800 border-red-200",
    icon: XCircle,
    description: "Booking cancelled",
  },
  completed: {
    label: "Completed",
    color: "bg-gray-100 text-gray-800 border-gray-200",
    icon: CheckCircle,
    description: "Stay completed",
  },
} as const;
