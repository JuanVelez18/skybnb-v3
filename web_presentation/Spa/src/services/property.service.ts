import httpClient from "@/core/httpClient";
import type { PropertyType } from "@/models/properties";

type PropertyTypeDto = {
  id: number;
  name: string;
};

export class PropertyService {
  public static async getAllProperties(): Promise<PropertyType[]> {
    const response = await httpClient.publicRequest<PropertyTypeDto[]>(
      "GET",
      "/property-types"
    );

    return response.data.map((type) => ({
      id: type.id.toString(),
      name: type.name,
    }));
  }
}
