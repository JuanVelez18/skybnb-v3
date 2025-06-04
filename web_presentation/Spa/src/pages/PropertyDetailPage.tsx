import { MapPin, Star } from "lucide-react";

import { Button } from "@/components/ui/button";
import Carousel from "@/components/Carousel";
import ReviewsSummarySection from "@/components/ReviewsSummarySection";
import {
  LocationCard,
  NoReviewsCard,
  PropertyHostCard,
  PropertyInfoDetailCard,
} from "@/components/cards";
import { useParams } from "react-router-dom";
import { useGetPropertyDetail } from "@/queries/properties.queries";
import { Skeleton } from "@/components/ui/skeleton";
import { useMemo } from "react";

const fallbackImages = [
  {
    url: "/placeholder.svg?height=400&width=600",
    type: "image",
  },
  {
    url: "/placeholder.svg?height=400&width=600",
    type: "image",
  },
  {
    url: "/placeholder.svg?height=400&width=600",
    type: "image",
  },
];

const PropertyDetailPage = () => {
  const { id } = useParams();
  const { propertyDetail, isPropertyDetailLoading, isPropertyDetailError } =
    useGetPropertyDetail(id ?? "");

  const reviews = useMemo(
    () =>
      propertyDetail?.reviews.map((review) => ({
        ...review,
        author: review.guestFullName,
        avatar: undefined,
        rating: review.rating,
      })),
    [propertyDetail?.reviews]
  );

  if (isPropertyDetailLoading)
    return (
      <div className="max-w-6xl mx-auto space-y-8">
        <Skeleton className="h-20" />
        <Skeleton className="h-96" />
        <Skeleton className="h-20" />
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          <div className="lg:col-span-2 space-y-8">
            <Skeleton className="h-16" />
            <Skeleton className="h-96" />
            <Skeleton className="h-16" />
          </div>
          <div className="space-y-6">
            <Skeleton className="h-16" />
            <Skeleton className="h-96" />
          </div>
        </div>
      </div>
    );

  if (isPropertyDetailError)
    return (
      <div className="max-w-6xl mx-auto space-y-8">
        <h1 className="text-3xl font-bold">Error loading property details</h1>
        <p className="text-muted-foreground">
          Please try again later or contact support.
        </p>
      </div>
    );

  return (
    <div className="max-w-6xl mx-auto space-y-8">
      <div className="space-y-4">
        <div className="flex justify-between items-start">
          <div className="space-y-2">
            <h1 className="text-3xl font-bold">{propertyDetail!.title}</h1>
            <div className="flex items-center gap-4 text-muted-foreground">
              {propertyDetail!.reviewsCount > 0 ? (
                <div className="flex items-center gap-1">
                  <Star className="h-4 w-4 fill-yellow-400 text-yellow-400" />
                  <span className="font-medium">{propertyDetail!.rating}</span>
                  <span>({propertyDetail!.reviewsCount} reviews)</span>
                </div>
              ) : (
                <span className="text-muted-foreground">New listing</span>
              )}
              <div className="flex items-center gap-1">
                <MapPin className="h-4 w-4" />
                <span>
                  {propertyDetail!.location.city}{" "}
                  {propertyDetail!.location.country}
                </span>
              </div>
            </div>
          </div>

          <div className="flex items-center gap-2">{/* Form Sheet */}</div>
        </div>
      </div>

      <Carousel
        assets={
          propertyDetail?.multimedia.length
            ? propertyDetail.multimedia
            : fallbackImages
        }
      />

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <div className="lg:col-span-2 space-y-8">
          <PropertyInfoDetailCard
            type={propertyDetail!.type}
            maxGuests={propertyDetail!.guests}
            bedrooms={propertyDetail!.bedrooms}
            beds={propertyDetail!.beds}
            bathrooms={propertyDetail!.bathrooms}
            price={propertyDetail!.price}
            description={propertyDetail!.description}
          />

          <div className="space-y-6">
            <div className="flex items-center gap-4">
              <h3 className="text-lg font-semibold">Reviews</h3>
              {propertyDetail!.reviewsCount > 0 ? (
                <div className="flex items-center gap-1">
                  <Star className="h-4 w-4 fill-yellow-400 text-yellow-400" />
                  <span className="font-medium">{propertyDetail!.rating}</span>
                  <span className="text-muted-foreground">
                    ({propertyDetail!.reviewsCount} reviews)
                  </span>
                </div>
              ) : (
                <span className="text-muted-foreground">No reviews yet</span>
              )}
            </div>

            {propertyDetail!.reviewsCount > 0 ? (
              <>
                <ReviewsSummarySection reviews={reviews!} />

                <Button variant="outline" className="w-full">
                  Show all {propertyDetail!.reviewsCount} reviews
                </Button>
              </>
            ) : (
              <NoReviewsCard />
            )}
          </div>
        </div>

        <div className="space-y-6">
          <PropertyHostCard
            name={propertyDetail!.host.fullName}
            yearSince={propertyDetail!.host.createdAt.getFullYear()}
          />

          <LocationCard location={propertyDetail!.location.address} />
        </div>
      </div>
    </div>
  );
};

export default PropertyDetailPage;
