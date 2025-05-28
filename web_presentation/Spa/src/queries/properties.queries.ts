import { useInfiniteQuery, useMutation, useQuery } from "@tanstack/react-query";
import { toast } from "sonner";

import type { CreationMediaFile } from "@/models/multimedia";
import type {
  PropertyBasicInformation,
  PropertyFilters,
} from "@/models/properties";
import type { CreationAddress } from "@/models/ubication";
import { PropertyService } from "@/services/property.service";
import type { PaginationOptions } from "@/models/pagination";
import { useMemo } from "react";

type CreationPropertyData = {
  information: PropertyBasicInformation;
  address: CreationAddress;
  multimedia: CreationMediaFile[];
};

export const PropertiesQueryKeys = {
  all: ["properties"] as const,
  allTypes: () => [...PropertiesQueryKeys.all, "types"] as const,
  search: () => [...PropertiesQueryKeys.all, "search"] as const,
  filteredSearch: (filters: PropertyFilters) =>
    [...PropertiesQueryKeys.search(), filters] as const,
};

export const useGetAllPropertyTypes = () => {
  const { data, isLoading, isError } = useQuery({
    queryKey: PropertiesQueryKeys.allTypes(),
    queryFn: async () => PropertyService.getAllProperties(),
  });

  return {
    propertyTypes: data,
    arePropertyTypesLoading: isLoading,
    isPropertyTypesError: isError,
  };
};

export const useCreateProperty = () => {
  const { mutate, isPending, isError } = useMutation({
    mutationFn: (data: CreationPropertyData) =>
      PropertyService.createProperty(
        data.information,
        data.address,
        data.multimedia
      ),
    onSuccess() {
      toast.success("Property created successfully!");
    },
    onError() {
      toast.error("Failed to create property. Please try again.");
    },
  });

  return {
    createProperty: mutate,
    isCreatingProperty: isPending,
    isCreatePropertyError: isError,
  };
};

export function useInfinitePropertiesSearch(filters: PropertyFilters) {
  const {
    data,
    isLoading,
    isError,
    hasNextPage,
    isFetchingNextPage,
    fetchNextPage,
  } = useInfiniteQuery({
    queryKey: PropertiesQueryKeys.filteredSearch(filters),
    queryFn: ({ pageParam }) =>
      PropertyService.searchProperties(pageParam, filters),
    initialPageParam: {} as PaginationOptions,
    getNextPageParam({ page, totalPages }): PaginationOptions | undefined {
      const nextPage = page + 1;
      if (nextPage > totalPages) return undefined;

      return { page: nextPage, pageSize: 10 };
    },
  });

  const properties = useMemo(() => {
    return data?.pages.flatMap((page) => page.results) ?? [];
  }, [data]);

  return {
    properties,
    total: data?.pages[0].total ?? 0,
    arePropertiesLoading: isLoading,
    isLoadingNextPropertiesPage: isFetchingNextPage,
    isPropertiesError: isError,
    canLoadMoreProperties: hasNextPage && !isFetchingNextPage,
    hasMoreProperties: hasNextPage,
    fetchNextPropertiesPage: fetchNextPage,
  };
}
