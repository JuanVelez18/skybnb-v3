import httpClient, { type ApiResponse } from "@/core/httpClient";
import type { CreationMediaFile } from "@/models/multimedia";
import type {
  PropertyBasicInformation,
  PropertyFilters,
  PropertySummary,
  PropertyType,
} from "@/models/properties";
import type { CreationAddress } from "@/models/ubication";
import { MultimediaService } from "./multimedia.service";
import type { Page, PaginationOptions } from "@/models/pagination";
import { getTokensFromStorage } from "@/utils/auth";

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

  public static async createProperty(
    information: PropertyBasicInformation,
    address: CreationAddress,
    multimedia: CreationMediaFile[]
  ): Promise<void> {
    const multimediaDto = await MultimediaService.uploadMultimedia(multimedia);

    const propertyDto = {
      Title: information.title,
      TypeId: parseInt(information.propertyType, 10),
      NumBeds: information.beds,
      MaxGuests: information.maxGuests,
      Description: information.description,
      NumBedrooms: information.bedrooms,
      NumBathrooms: information.bathrooms,
      BasePricePerNight: information.basePrice,
      Address: {
        Street: address.street,
        StreetNumber: address.streetNumber,
        IntersectionNumber: address.intersectionNumber,
        DoorNumber: address.doorNumber,
        CityId: address.cityId,
        Complement: address.complement,
      },
      Multimedia: multimediaDto,
    };

    await httpClient.post("/properties", propertyDto);
  }

  public static async searchProperties(
    pagination: PaginationOptions,
    filters?: PropertyFilters
  ): Promise<Page<PropertySummary>> {
    const isAuthenticated = getTokensFromStorage() !== null;
    const endpoint = "/properties";
    const params = {
      ...pagination,
      ...(filters ?? {}),
    };

    let response: ApiResponse<Page<PropertySummary>>;

    if (isAuthenticated) {
      response = await httpClient.get<Page<PropertySummary>>(endpoint, {
        params,
      });
    } else {
      response = await httpClient.publicRequest<Page<PropertySummary>>(
        "GET",
        endpoint,
        undefined,
        {
          params,
        }
      );
    }

    return response.data;
  }
}
