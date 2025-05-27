import httpClient from "@/core/httpClient";
import type { City } from "@/models/ubication";

export class UbicationService {
  public static async getAllCities(): Promise<City[]> {
    const response = await httpClient.get<City[]>("/cities");

    return response.data;
  }
}
