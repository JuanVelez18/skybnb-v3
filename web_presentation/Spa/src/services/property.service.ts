import httpClient, { type ApiResponse } from "@/core/httpClient";
import type { CreationMediaFile } from "@/models/multimedia";
import {
  type PropertyBasicInformation,
  type PropertyFilters,
  type PropertySummary,
  type PropertyType,
} from "@/models/properties";
import type { CreationAddress } from "@/models/ubication";
import { MultimediaService } from "./multimedia.service";
import {
  dtoToPage,
  type Page,
  type PageDto,
  type PaginationOptions,
} from "@/models/pagination";
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
      checkIn: filters?.checkIn?.toISOString().split("T")[0],
      checkOut: filters?.checkOut?.toISOString().split("T")[0],
    };

    let response: ApiResponse<PageDto<PropertySummary>>;

    if (isAuthenticated) {
      response = await httpClient.get(endpoint, {
        params,
      });
    } else {
      response = await httpClient.publicRequest("GET", endpoint, undefined, {
        params,
      });
    }

    return dtoToPage(response.data);
  }
}
