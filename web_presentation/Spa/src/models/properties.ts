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

export type PropertySummary = {
  id: string;
  title: string;
  description: string;
  photoUrl: string;
  city: string;
  country: string;
  maxGuests: number;
  bedrooms: number;
  bathrooms: number;
  price: number;
  propertyType: string;
  rating: number;
  reviews: number;
};

export type SortBy = "price-low" | "price-high" | "rating" | "newest";

export type PropertyFilters = {
  minPrice?: number;
  maxPrice?: number;
  types?: number[];
  bedrooms?: number;
  bathrooms?: number;
  location?: string;
  checkIn?: Date;
  checkOut?: Date;
  guests?: number;
  sortBy?: SortBy;
};

export type AddressDto = {
  Street: string;
  StreetNumber: number;
  IntersectionNumber: number;
  DoorNumber: number;
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
  City: number;
  Country: number;
  Multimedia: MediaFileDto[];
};
