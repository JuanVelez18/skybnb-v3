import { useEffect, useRef, useState } from "react";
import { Grid3X3, List } from "lucide-react";

import { Button } from "@/components/ui/button";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";

import PropertyFiltersComponent from "@/components/PropertyFilters";
import PropertySearch from "@/components/PropertySearch";
import type { PropertyFilters, SortBy } from "@/models/properties";
import { usePropertiesStore } from "@/stores/properties.store";
import PropertyCard from "@/components/cards/PropertyCard";
import { useInfinitePropertiesSearch } from "@/queries/properties.queries";
import { CardSkeleton } from "@/components/skeletons";
import { Link } from "react-router-dom";
import { RouteNames } from "@/router/routes";

const sortOptions: { value: SortBy; label: string }[] = [
  { value: "price-low", label: "Price: Low to High" },
  { value: "price-high", label: "Price: High to Low" },
  { value: "rating", label: "Highest Rated" },
  { value: "newest", label: "Newest" },
];

const HomePage = () => {
  const { filters, updateSortBy } = usePropertiesStore();

  const [viewMode, setViewMode] = useState<"grid" | "list">("grid");
  const [areFiltersVisible, setAreFiltersVisible] = useState(false);
  const [queryFilters, setQueryFilters] = useState<PropertyFilters>(
    structuredClone(filters)
  );

  const loadMoreRef = useRef<HTMLDivElement | null>(null);

  const {
    properties,
    total,
    arePropertiesLoading,
    isLoadingNextPropertiesPage,
    hasMoreProperties,
    fetchNextPropertiesPage,
  } = useInfinitePropertiesSearch(queryFilters);

  const showSkeleton =
    arePropertiesLoading || isLoadingNextPropertiesPage || hasMoreProperties;

  const handleSortChange = (value: SortBy) => {
    updateSortBy(value);
    setQueryFilters((prev) => ({ ...prev, sortBy: value }));
  };

  useEffect(() => {
    const target = loadMoreRef.current;

    if (!target || !hasMoreProperties) return;

    const observer = new IntersectionObserver(
      (entries) => {
        if (
          entries[0].isIntersecting &&
          !arePropertiesLoading &&
          !isLoadingNextPropertiesPage
        ) {
          fetchNextPropertiesPage();
        }
      },
      {
        root: null,
        rootMargin: "30px 0px",
      }
    );

    observer.observe(target);

    return () => observer.unobserve(target);
  }, [
    arePropertiesLoading,
    hasMoreProperties,
    isLoadingNextPropertiesPage,
    fetchNextPropertiesPage,
  ]);

  return (
    <div className="h-full space-y-6">
      <PropertySearch
        onFiltersClick={() => setAreFiltersVisible(!areFiltersVisible)}
        onSearch={() => setQueryFilters(structuredClone(filters))}
      />

      <div className="flex gap-6">
        {areFiltersVisible && <PropertyFiltersComponent />}

        <div className="flex-1 space-y-4">
          <div className="flex justify-between items-center">
            <div>
              <h2 className="text-lg font-semibold">
                {arePropertiesLoading
                  ? "Searching properties..."
                  : `${total} properties found`}
              </h2>
              <p className="text-sm text-muted-foreground">
                {filters.location && `in ${filters.location}`}
              </p>
            </div>

            <div className="flex items-center gap-3">
              <Select value={filters.sortBy} onValueChange={handleSortChange}>
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
            {properties.map((property) => (
              <Link
                to={RouteNames.PROPERTY_DETAIL.replace(":id", property.id)}
                key={property.id}
              >
                <PropertyCard key={property.id} property={property} />
              </Link>
            ))}

            {showSkeleton &&
              [...Array(3)].map((_, index) => (
                <CardSkeleton
                  key={index}
                  ref={index === 0 ? loadMoreRef : undefined}
                />
              ))}
          </div>
        </div>
      </div>
    </div>
  );
};

export default HomePage;
