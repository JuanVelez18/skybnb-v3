import { SlidersHorizontal } from "lucide-react";

import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Checkbox } from "@/components/ui/checkbox";
import { Label } from "@/components/ui/label";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Separator } from "@/components/ui/separator";
import { Slider } from "@/components/ui/slider";
import { usePropertiesStore } from "@/stores/properties.store";
import { useGetAllPropertyTypes } from "@/queries/properties.queries";

const PropertiesFilters = () => {
  const { propertyTypes, arePropertyTypesLoading, isPropertyTypesError } =
    useGetAllPropertyTypes();

  const {
    filters,
    updatePriceRange,
    tooglePropertyType,
    updateBedrooms,
    updateBathrooms,
  } = usePropertiesStore();

  return (
    <Card className="w-80 h-fit">
      <CardHeader>
        <CardTitle className="flex items-center gap-2">
          <SlidersHorizontal className="h-5 w-5" />
          Filters
        </CardTitle>
      </CardHeader>
      <CardContent className="space-y-6">
        {/* Price Range */}
        <div className="space-y-3">
          <Label>Price per night</Label>
          <Slider
            value={[filters.minPrice || 0, filters.maxPrice || 500]}
            onValueChange={([min, max]) => updatePriceRange(min, max)}
            max={500}
            min={0}
            step={10}
            className="w-full"
          />
          <div className="flex justify-between text-sm text-muted-foreground">
            <span>${filters.minPrice}</span>
            <span>${filters.maxPrice}</span>
          </div>
        </div>

        <Separator />

        {/* Property Type */}
        <div className="space-y-3">
          <Label>Property Type</Label>
          <div className="space-y-2">
            {arePropertyTypesLoading && "Loading property types..."}
            {!arePropertyTypesLoading && isPropertyTypesError && (
              <span className="text-red-500">Error loading property types</span>
            )}
            {!arePropertyTypesLoading &&
              !isPropertyTypesError &&
              propertyTypes!.map((type) => (
                <div key={type.id} className="flex items-center space-x-2">
                  <Checkbox
                    id={type.id}
                    checked={filters.types?.includes(+type.id)}
                    onCheckedChange={() => tooglePropertyType(+type.id)}
                  />
                  <Label htmlFor={type.id} className="text-sm font-normal">
                    {type.name}
                  </Label>
                </div>
              ))}
          </div>
        </div>

        <Separator />

        {/* Rooms and Beds */}
        <div className="space-y-3">
          <Label>Rooms & Beds</Label>
          <div className="grid grid-cols-2 gap-3">
            <div className="space-y-2">
              <Label htmlFor="bedrooms" className="text-sm">
                Bedrooms
              </Label>
              <Select
                value={filters.bedrooms?.toString()}
                onValueChange={(value) => updateBedrooms(+value)}
              >
                <SelectTrigger>
                  <SelectValue placeholder="Any" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="any">Any</SelectItem>
                  {[1, 2, 3, 4, 5].map((num) => (
                    <SelectItem key={num} value={num.toString()}>
                      {num}+
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <Label htmlFor="bathrooms" className="text-sm">
                Bathrooms
              </Label>
              <Select
                value={filters.bathrooms?.toString()}
                onValueChange={(value) => updateBathrooms(+value)}
              >
                <SelectTrigger>
                  <SelectValue placeholder="Any" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="any">Any</SelectItem>
                  {[1, 2, 3, 4, 5].map((num) => (
                    <SelectItem key={num} value={num.toString()}>
                      {num}+
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>
          </div>
        </div>
      </CardContent>
    </Card>
  );
};

export default PropertiesFilters;
