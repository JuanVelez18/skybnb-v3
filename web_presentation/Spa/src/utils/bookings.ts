import type { Booking } from "@/models/bookings";
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

// Mock bookings data
export const mockBookings: Booking[] = [
  {
    id: "BK001",
    property: {
      title: "Beautiful Oceanfront House",
      photoUrl: null,
      city: "Miami Beach",
      country: "USA",
    },
    host: {
      id: "User1",
      fullName: "Sarah Johnson",
    },
    guest: {
      id: "6c11da45-1754-40e8-7235-08dda240116c",
      fullName: "John Doe",
      email: "john.doe@email.com",
    },
    checkIn: new Date("2024-01-15"),
    checkOut: new Date("2024-01-18"),
    guests: 4,
    totalAmount: 475, // 3 nights * $150 + $25 service fee
    status: "pending",
    message:
      "Hi! We're a family of 4 looking forward to our beach vacation. We'll arrive around 3 PM.",
    rating: null,
  },
  {
    id: "BK002",
    property: {
      title: "Cozy Mountain Cabin",
      photoUrl: null,
      city: "Aspen",
      country: "USA",
    },
    host: {
      id: "User3",
      fullName: "Emily Smith",
    },
    guest: {
      id: "6c11da45-1754-40e8-7235-08dda240116c",
      fullName: "Alice Brown",
      email: "alice@yopmail.com",
    },
    checkIn: new Date("2024-02-10"),
    checkOut: new Date("2024-02-15"),
    guests: 2,
    totalAmount: 600,
    status: "approved",
    message: null,
    rating: null,
  },
  {
    id: "BK003",
    property: {
      title: "Modern City Apartment",
      photoUrl: null,
      city: "New York",
      country: "USA",
    },
    host: {
      id: "User5",
      fullName: "Michael Lee",
    },
    guest: {
      id: "User6",
      fullName: "Jane Smith",
      email: "jane@yopmail.com",
    },
    checkIn: new Date("2024-03-05"),
    checkOut: new Date("2024-03-10"),
    guests: 3,
    totalAmount: 800, // 5 nights * $150 + $50 service fee
    status: "confirmed",
    message: null,
    rating: 4.5,
  },
  {
    id: "BK004",
    property: {
      title: "Charming Countryside Villa",
      photoUrl: null,
      city: "Napa Valley",
      country: "USA",
    },
    host: {
      id: "User7",
      fullName: "David Wilson",
    },
    guest: {
      id: "User8",
      fullName: "Laura Green",
      email: "laura@yopmail.com",
    },
    checkIn: new Date("2024-04-01"),
    checkOut: new Date("2024-04-05"),
    guests: 5,
    totalAmount: 900, // 4 nights * $200 + $50 service fee
    status: "cancelled",
    message: null,
    rating: null,
  },
  {
    id: "BK005",
    property: {
      title: "Luxury Beachfront Resort",
      photoUrl: null,
      city: "Cancun",
      country: "Mexico",
    },
    host: {
      id: "User9",
      fullName: "Olivia Taylor",
    },
    guest: {
      id: "User10",
      fullName: "Chris Johnson",
      email: "chris@yopmail.com",
    },
    checkIn: new Date("2024-05-20"),
    checkOut: new Date("2024-05-25"),
    guests: 2,
    totalAmount: 1200, // 5 nights * $200 + $100 service fee
    status: "completed",
    message: null,
    rating: 4,
  },
];
