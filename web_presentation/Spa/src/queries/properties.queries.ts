import { useMutation, useQuery } from "@tanstack/react-query";
import { toast } from "sonner";

import type { CreationMediaFile } from "@/models/multimedia";
import type { PropertyBasicInformation } from "@/models/properties";
import type { CreationAddress } from "@/models/ubication";
import { PropertyService } from "@/services/property.service";

type CreationPropertyData = {
  information: PropertyBasicInformation;
  address: CreationAddress;
  multimedia: CreationMediaFile[];
};

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
