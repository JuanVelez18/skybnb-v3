export type CreationAddress = {
  street: string;
  streetNumber: number;
  intersectionNumber: number;
  doorNumber: number;
  cityId: number;
  countryId: number;
  complement: string;
  latitude: number | null;
  longitude: number | null;
};

export type City = {
  id: number;
  name: string;
};

export type Country = {
  id: number;
  name: string;
  code: string;
};

export type CountryDto = {
  id: number;
  name: string;
  isoCode: string;
};

export const dtoToCountry = (dto: CountryDto): Country => ({
  id: dto.id,
  name: dto.name,
  code: dto.isoCode,
});
