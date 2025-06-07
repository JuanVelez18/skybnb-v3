import { Bath, Bed, Users } from "lucide-react";

import { Separator } from "@/components/ui/separator";

type Props = {
  type: string;
  maxGuests: number;
  bedrooms: number;
  beds: number;
  bathrooms: number;
  price: number;
  description: string;
};

const PropertyInfoDetailCard = ({
  type,
  maxGuests,
  bedrooms,
  beds,
  bathrooms,
  price,
  description,
}: Props) => {
  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h2 className="text-2xl font-semibold">{type}</h2>
          <div className="flex items-center gap-4 text-muted-foreground mt-1">
            <span className="flex items-center gap-1">
              <Users className="h-4 w-4" />
              {maxGuests} guests
            </span>
            <span className="flex items-center gap-1">
              <Bed className="h-4 w-4" />
              {bedrooms} bedrooms
            </span>
            <span className="flex items-center gap-1">
              <Bed className="h-4 w-4" />
              {beds} beds
            </span>
            <span className="flex items-center gap-1">
              <Bath className="h-4 w-4" />
              {bathrooms} bathrooms
            </span>
          </div>
        </div>
        <div className="text-right">
          <p className="text-2xl font-bold">${price}</p>
          <p className="text-sm text-muted-foreground">per night</p>
        </div>
      </div>

      <Separator />

      <div>
        <h3 className="text-lg font-semibold mb-3">About this place</h3>
        <p className="text-muted-foreground leading-relaxed">{description}</p>
      </div>
    </div>
  );
};

export default PropertyInfoDetailCard;
