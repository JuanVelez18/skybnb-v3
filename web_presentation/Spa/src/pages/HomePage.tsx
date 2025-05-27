import { useState } from "react";
import { Grid3X3, List } from "lucide-react";

import { Button } from "@/components/ui/button";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";

import PropertyFilters from "@/components/PropertyFilters";
import PropertySearch from "@/components/PropertySearch";
import type { PropertySummary, SortBy } from "@/models/properties";
import { usePropertiesStore } from "@/stores/properties.store";
import PropertyCard from "@/components/cards/PropertyCard";

const sortOptions: { value: SortBy; label: string }[] = [
  { value: "price-low", label: "Price: Low to High" },
  { value: "price-high", label: "Price: High to Low" },
  { value: "rating", label: "Highest Rated" },
  { value: "newest", label: "Newest" },
];

const mockProperties: PropertySummary[] = [
  {
    id: "1",
    title: "Beautiful Oceanfront House",
    description: "Stunning beachfront property with panoramic ocean views",
    photoUrl: "",
    price: 150,
    rating: 4.8,
    reviews: 124,
    city: "Miami Beach",
    country: "USA",
    bedrooms: 3,
    bathrooms: 2,
    maxGuests: 6,
    propertyType: "Entire house",
  },
  {
    id: "2",
    title: "Modern Downtown Apartment",
    description: "Stylish apartment in the heart of the city",
    photoUrl: "",
    price: 89,
    rating: 4.6,
    reviews: 89,
    city: "New York",
    country: "USA",
    bedrooms: 2,
    bathrooms: 1,
    maxGuests: 4,
    propertyType: "Apartment",
  },
  {
    id: "3",
    title: "Cozy Mountain Cabin",
    description: "Perfect retreat in the mountains with fireplace",
    photoUrl: "",
    price: 120,
    rating: 4.9,
    reviews: 67,
    city: "Aspen",
    country: "USA",
    bedrooms: 2,
    bathrooms: 1,
    maxGuests: 4,
    propertyType: "Cabin",
  },
  {
    id: "4",
    title: "Luxury Villa with Pool",
    description: "Spacious villa with private pool and garden",
    photoUrl: "",
    price: 280,
    rating: 4.7,
    reviews: 156,
    city: "Los Angeles",
    country: "USA",
    bedrooms: 4,
    bathrooms: 3,
    maxGuests: 8,
    propertyType: "Villa",
  },
  {
    id: "5",
    title: "Historic Loft in Arts District",
    description: "Unique loft space with exposed brick and high ceilings",
    photoUrl: "",
    price: 95,
    rating: 4.5,
    reviews: 43,
    city: "Chicago",
    country: "USA",
    bedrooms: 1,
    bathrooms: 1,
    maxGuests: 2,
    propertyType: "Loft",
  },
  {
    id: "6",
    title: "Beachside Studio",
    description: "Compact studio steps away from the beach",
    photoUrl: "",
    price: 75,
    rating: 4.4,
    reviews: 78,
    city: "San Diego",
    country: "USA",
    bedrooms: 1,
    bathrooms: 1,
    maxGuests: 2,
    propertyType: "Studio",
  },
];

const HomePage = () => {
  const { filters, updateSortBy } = usePropertiesStore();

  const [viewMode, setViewMode] = useState<"grid" | "list">("grid");
  const [areFiltersVisible, setAreFiltersVisible] = useState(false);

  return (
    <div className="space-y-6">
      <PropertySearch
        onFiltersClick={() => setAreFiltersVisible(!areFiltersVisible)}
      />

      <div className="flex gap-6">
        {areFiltersVisible && <PropertyFilters />}

        <div className="flex-1 space-y-4">
          <div className="flex justify-between items-center">
            <div>
              <h2 className="text-lg font-semibold">
                {mockProperties.length} properties found
              </h2>
              <p className="text-sm text-muted-foreground">
                {filters.location && `in ${filters.location}`}
              </p>
            </div>

            <div className="flex items-center gap-3">
              <Select value={filters.sortBy} onValueChange={updateSortBy}>
                <SelectTrigger className="w-48">
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  {sortOptions.map((option) => (
                    <SelectItem key={option.value} value={option.value}>
                      {option.label}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>

              <div className="flex border rounded-md">
                <Button
                  variant={viewMode === "grid" ? "default" : "ghost"}
                  size="sm"
                  onClick={() => setViewMode("grid")}
                  className="rounded-r-none"
                >
                  <Grid3X3 className="h-4 w-4" />
                </Button>
                <Button
                  variant={viewMode === "list" ? "default" : "ghost"}
                  size="sm"
                  onClick={() => setViewMode("list")}
                  className="rounded-l-none"
                >
                  <List className="h-4 w-4" />
                </Button>
              </div>
            </div>
          </div>

          <div
            className={`grid gap-6 ${
              viewMode === "grid"
                ? "grid-cols-1 md:grid-cols-2 lg:grid-cols-3"
                : "grid-cols-1"
            }`}
          >
            {mockProperties.map((property) => (
              <PropertyCard key={property.id} property={property} />
            ))}
          </div>
        </div>
      </div>
    </div>
  );
};

export default HomePage;
