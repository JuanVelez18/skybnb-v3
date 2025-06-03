import httpClient from "@/core/httpClient";
import {
  dtoToCountry,
  type City,
  type Country,
  type CountryDto,
} from "@/models/ubication";

export class UbicationService {
  public static async getAllCities(): Promise<City[]> {
    const response = await httpClient.get<City[]>("/cities");

    return response.data;
  }

  public static async getAllCountries(): Promise<Country[]> {
    const response = await httpClient.get<CountryDto[]>("/countries");

    return response.data.map(dtoToCountry);
  }
}
