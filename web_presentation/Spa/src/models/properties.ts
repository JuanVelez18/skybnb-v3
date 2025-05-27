export type PropertyBasicInformation = {
  title: string;
  description: string;
  bathrooms: number;
  bedrooms: number;
  beds: number;
  maxGuests: number;
  basePrice: number;
  propertyType: string;
};

export type PropertyType = {
  id: string;
  name: string;
};

export type AddressDto = {
  Street: string;
  StreetNumber: number;
  IntersectionNumber: number;
  DoorNumber: number;
  CityId: number;
  Complement: string;
};

export type MediaFileDto = {
  Url: string;
  Type: string;
  Order: number;
};

export type CreationPropertyDto = {
  Title: string;
  TypeId: number;
  NumBeds: number;
  MaxGuests: number;
  Description: string;
  NumBedrooms: number;
  NumBathrooms: number;
  BasePricePerNight: number;
  Address: AddressDto;
  MediaFiles: MediaFileDto[];
};
