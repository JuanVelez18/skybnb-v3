export type CreationAddress = {
  street: string;
  streetNumber: number;
  intersectionNumber: number;
  doorNumber: number;
  cityId: number;
  complement: string;
  latitude: number | null;
  longitude: number | null;
};

export type City = {
  id: number;
  name: string;
};
