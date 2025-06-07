import httpClient, { type ApiResponse } from "@/core/httpClient";
import type { CreationMediaFile } from "@/models/multimedia";
import {
  dtoToPropertyDetail,
  type CreationPropertyDto,
  type PropertyBasicInformation,
  type PropertyDetail,
  type PropertyDetailDto,
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
import { useAuthStore } from "@/stores/auth.store";
import { dateToDateOnlyString } from "@/utils/dates";

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

    const propertyDto: CreationPropertyDto = {
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
        Complement: address.complement,
      },
      City: address.cityId,
      Country: address.countryId,
      Multimedia: multimediaDto,
    };

    await httpClient.post("/properties", propertyDto);
  }

  public static async searchProperties(
    pagination: PaginationOptions,
    filters?: PropertyFilters
  ): Promise<Page<PropertySummary>> {
    const isAuthenticated = useAuthStore.getState().isAuthenticated;
    const endpoint = "/properties";
    const params = {
      ...pagination,
      ...(filters ?? {}),
      checkIn: filters?.checkIn
        ? dateToDateOnlyString(filters.checkIn)
        : undefined,
      checkOut: filters?.checkOut
        ? dateToDateOnlyString(filters.checkOut)
        : undefined,
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

  public static async getPropertyDetail(
    propertyId: string
  ): Promise<PropertyDetail> {
    const isAuthenticated = useAuthStore.getState().isAuthenticated;

    let response: ApiResponse<PropertyDetailDto>;

    if (isAuthenticated) {
      response = await httpClient.get(`/properties/${propertyId}`);
    } else {
      response = await httpClient.publicRequest(
        "GET",
        `/properties/${propertyId}`
      );
    }

    return dtoToPropertyDetail(response.data);
  }
}
