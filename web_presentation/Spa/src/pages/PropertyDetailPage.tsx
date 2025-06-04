import { MapPin, Star } from "lucide-react";

import { Button } from "@/components/ui/button";
import Carousel from "@/components/Carousel";
import ReviewsSummarySection from "@/components/ReviewsSummarySection";
import {
  LocationCard,
  PropertyHostCard,
  PropertyInfoDetailCard,
} from "@/components/cards";

// TODO: Replace with actual data fetching logic
const mockProperty = {
  id: 1,
  title: "Beautiful Oceanfront House with Stunning Views",
  description:
    "Experience the ultimate beachfront getaway in this stunning oceanfront house. Wake up to breathtaking sunrise views over the Atlantic Ocean from your private balcony. This spacious and elegantly furnished home features an open-concept living area with floor-to-ceiling windows that showcase the spectacular ocean panorama. The fully equipped gourmet kitchen is perfect for preparing meals with fresh local ingredients. Relax in the comfortable living room or step outside to your private deck where you can enjoy morning coffee or evening cocktails while listening to the soothing sounds of the waves. The master bedroom offers a king-size bed and direct ocean views, while the additional bedrooms provide comfortable accommodations for family and friends.",
  images: [
    "/placeholder.svg?height=400&width=600",
    "/placeholder.svg?height=400&width=600",
    "/placeholder.svg?height=400&width=600",
    "/placeholder.svg?height=400&width=600",
    "/placeholder.svg?height=400&width=600",
  ],
  price: 150,
  rating: 4.8,
  reviewCount: 124,
  location: "Miami Beach, FL",
  bedrooms: 3,
  beds: 4,
  bathrooms: 2,
  maxGuests: 6,
  propertyType: "Entire house",
  amenities: ["wifi", "parking", "kitchen", "tv", "ac"],
  host: {
    name: "Sarah Johnson",
    avatar: undefined,
    memberSince: "2019",
    responseRate: 98,
    responseTime: "within an hour",
  },
  reviews: [
    {
      id: "1",
      author: "Michael Chen",
      avatar: undefined,
      rating: 5,
      date: "December 2024",
      comment:
        "Absolutely incredible stay! The ocean views were breathtaking and the house was even better than the photos. Sarah was an amazing host - very responsive and helpful with local recommendations. The kitchen was fully stocked and the beds were super comfortable. We'll definitely be back!",
    },
    {
      id: "2",
      author: "Emma Rodriguez",
      avatar: undefined,
      rating: 5,
      date: "November 2024",
      comment:
        "Perfect location right on the beach! The house was spotless and had everything we needed for our family vacation. The kids loved being able to walk straight out to the beach. Sarah provided great local restaurant recommendations too.",
    },
    {
      id: "3",
      author: "David Thompson",
      avatar: undefined,
      rating: 4,
      date: "October 2024",
      comment:
        "Beautiful property with stunning views. The house is spacious and well-maintained. Only minor issue was the WiFi was a bit slow, but that didn't detract from our overall amazing experience. Highly recommend!",
    },
  ],
};

const PropertyDetailPage = () => {
  return (
    <div className="max-w-6xl mx-auto space-y-8">
      <div className="space-y-4">
        <div className="flex justify-between items-start">
          <div className="space-y-2">
            <h1 className="text-3xl font-bold">{mockProperty.title}</h1>
            <div className="flex items-center gap-4 text-muted-foreground">
              <div className="flex items-center gap-1">
                <Star className="h-4 w-4 fill-yellow-400 text-yellow-400" />
                <span className="font-medium">{mockProperty.rating}</span>
                <span>({mockProperty.reviewCount} reviews)</span>
              </div>
              <div className="flex items-center gap-1">
                <MapPin className="h-4 w-4" />
                <span>{mockProperty.location}</span>
              </div>
            </div>
          </div>

          <div className="flex items-center gap-2">{/* Form Sheet */}</div>
        </div>
      </div>

      <Carousel images={mockProperty.images} />

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <div className="lg:col-span-2 space-y-8">
          <PropertyInfoDetailCard
            type={mockProperty.propertyType}
            maxGuests={mockProperty.maxGuests}
            bedrooms={mockProperty.bedrooms}
            beds={mockProperty.beds}
            bathrooms={mockProperty.bathrooms}
            price={mockProperty.price}
            description={mockProperty.description}
          />

          <div className="space-y-6">
            <div className="flex items-center gap-4">
              <h3 className="text-lg font-semibold">Reviews</h3>
              <div className="flex items-center gap-1">
                <Star className="h-4 w-4 fill-yellow-400 text-yellow-400" />
                <span className="font-medium">{mockProperty.rating}</span>
                <span className="text-muted-foreground">
                  ({mockProperty.reviewCount} reviews)
                </span>
              </div>
            </div>

            <ReviewsSummarySection reviews={mockProperty.reviews} />

            <Button variant="outline" className="w-full">
              Show all {mockProperty.reviewCount} reviews
            </Button>
          </div>
        </div>

        <div className="space-y-6">
          <PropertyHostCard
            name={mockProperty.host.name}
            yearSince={mockProperty.host.memberSince}
            avatar={mockProperty.host.avatar}
          />

          <LocationCard location={mockProperty.location} />
        </div>
      </div>
    </div>
  );
};

export default PropertyDetailPage;
