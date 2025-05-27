import httpClient from "@/core/httpClient";
import type { CreationMediaFile } from "@/models/multimedia";
import type {
  PropertyBasicInformation,
  PropertyType,
} from "@/models/properties";
import type { CreationAddress } from "@/models/ubication";
import { MultimediaService } from "./multimedia.service";

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
}
