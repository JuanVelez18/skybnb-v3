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
