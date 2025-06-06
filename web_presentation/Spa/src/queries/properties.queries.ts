import {
  useInfiniteQuery,
  useMutation,
  useQuery,
  useQueryClient,
} from "@tanstack/react-query";
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
  detail: (propertyId: string) =>
    [...PropertiesQueryKeys.all, "detail", propertyId] as const,
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
  const queryClient = useQueryClient();
  const { mutate, isPending, isError } = useMutation({
    mutationFn: (data: CreationPropertyData) =>
      PropertyService.createProperty(
        data.information,
        data.address,
        data.multimedia
      ),
    onSuccess() {
      queryClient.invalidateQueries({
        queryKey: PropertiesQueryKeys.search(),
      });
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
  const PAGE_SIZE = 10;

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
    initialPageParam: { page: 1, pageSize: PAGE_SIZE } as PaginationOptions,
    getNextPageParam({ page, totalPages }): PaginationOptions | undefined {
      const nextPage = page + 1;
      if (nextPage > totalPages) return undefined;

      return { page: nextPage, pageSize: PAGE_SIZE };
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

export const useGetPropertyDetail = (propertyId: string) => {
  const { data, isLoading, isError } = useQuery({
    queryKey: PropertiesQueryKeys.detail(propertyId),
    queryFn: () => PropertyService.getPropertyDetail(propertyId),
    enabled: !!propertyId,
  });

  return {
    propertyDetail: data,
    isPropertyDetailLoading: isLoading,
    isPropertyDetailError: isError,
  };
};
