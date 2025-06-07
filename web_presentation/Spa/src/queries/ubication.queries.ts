import { UbicationService } from "@/services/ubication.service";
import { useQuery } from "@tanstack/react-query";

export const UbicationQueryKeys = {
  cities: ["cities"] as const,
  allCities: () => [...UbicationQueryKeys.cities, "all"] as const,
  countries: ["countries"] as const,
  allCountries: () => [...UbicationQueryKeys.countries, "all"] as const,
};

export const useGetAllCities = () => {
  const { data, isLoading, isError } = useQuery({
    queryKey: UbicationQueryKeys.allCities(),
    queryFn: () => UbicationService.getAllCities(),
  });

  return {
    cities: data,
    areCitiesLoading: isLoading,
    isCitiesError: isError,
  };
};

export const useGetAllCountries = () => {
  const { data, isLoading, isError } = useQuery({
    queryKey: UbicationQueryKeys.allCountries(),
    queryFn: () => UbicationService.getAllCountries(),
  });

  return {
    countries: data,
    areCountriesLoading: isLoading,
    isCountriesError: isError,
  };
};
