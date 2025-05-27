import { PropertyService } from "@/services/property.service";
import { useQuery } from "@tanstack/react-query";

export const PropertiesQueryKeys = {
  all: ["properties"] as const,
  allTypes: () => [...PropertiesQueryKeys.all, "types"] as const,
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
