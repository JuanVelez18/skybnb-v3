import { Star, MapPin, Users, Bed, Bath } from "lucide-react";

import { Badge } from "@/components/ui/badge";
import { Card, CardContent } from "@/components/ui/card";
import type { PropertySummary } from "@/models/properties";

type Props = {
  property: PropertySummary;
};

const PropertyCard = ({ property }: Props) => {
  return (
    <Card
      key={property.id}
      className="overflow-hidden hover:shadow-lg transition-shadow"
    >
      <div className="relative">
        <img
          src={
            property.photoUrl ||
            "https://kzmol23mnnxrlf8ez8tp.lite.vusercontent.net/placeholder.svg?height=200&width=300"
          }
          alt={property.title}
          className="w-full h-48 object-cover"
        />
        <Badge className="absolute bottom-2 left-2">
          {property.propertyType}
        </Badge>
      </div>

      <CardContent className="p-4">
        <div className="space-y-2">
          <div className="flex justify-between items-start">
            <h3 className="font-semibold text-lg line-clamp-1">
              {property.title}
            </h3>
            <div className="flex items-center gap-1">
              <Star className="h-4 w-4 fill-yellow-400 text-yellow-400" />
              <span className="text-sm font-medium">{property.rating}</span>
              <span className="text-sm text-muted-foreground">
                ({property.reviews})
              </span>
            </div>
          </div>

          <p className="text-sm text-muted-foreground flex items-center gap-1">
            <MapPin className="h-3 w-3" />
            {property.city}, {property.country}
          </p>

          <p className="text-sm text-muted-foreground line-clamp-2">
            {property.description}
          </p>

          <div className="flex items-center gap-4 text-sm text-muted-foreground">
            <span className="flex items-center gap-1">
              <Users className="h-3 w-3" />
              {property.maxGuests} guests
            </span>
            <span className="flex items-center gap-1">
              <Bed className="h-3 w-3" />
              {property.bedrooms} bed{property.bedrooms !== 1 ? "s" : ""}
            </span>
            <span className="flex items-center gap-1">
              <Bath className="h-3 w-3" />
              {property.bathrooms} bath{property.bathrooms !== 1 ? "s" : ""}
            </span>
          </div>

          <div className="flex justify-between items-center pt-2">
            <div className="text-right">
              <p className="text-lg font-bold">${property.price}</p>
              <p className="text-xs text-muted-foreground">per night</p>
            </div>
          </div>
        </div>
      </CardContent>
    </Card>
  );
};

export default PropertyCard;
