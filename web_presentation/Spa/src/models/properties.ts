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

export type PropertyDetailLocation = {
  address: string;
  city: string;
  country: string;
  latitude: number | null;
  longitude: number | null;
};

export type PropertyDetailHost = {
  fullName: string;
  createdAt: Date;
};

export type PropertyDetailAsset = {
  url: string;
  type: string;
};

export type PropertyDetailReview = {
  guestFullName: string;
  rating: number;
  comment: string;
  createdAt: Date;
};

export type PropertyDetail = {
  id: string;
  title: string;
  description: string;
  guests: number;
  bedrooms: number;
  bathrooms: number;
  beds: number;
  price: number;
  type: string;
  location: PropertyDetailLocation;
  host: PropertyDetailHost;
  rating: number;
  reviewsCount: number;
  reviews: PropertyDetailReview[];
  multimedia: PropertyDetailAsset[];
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

export interface PropertyDetailHostDto {
  fullName: string;
  createdAt: Date;
}

export interface PropertyDetailLocationDto {
  address: string;
  city: string;
  country: string;
  latitude: null;
  longitude: null;
}

export interface PropertyDetailAssetDto {
  url: string;
  type: string;
}

export interface PropertyDetailReviewDto {
  guestFullName: string;
  rating: number;
  comment: string;
  createdAt: Date;
}

export interface PropertyDetailDto {
  id: string;
  title: string;
  description: string;
  guests: number;
  bedrooms: number;
  beds: number;
  bathrooms: number;
  pricePerNight: number;
  type: string;
  rating: number;
  reviewsCount: number;
  location: PropertyDetailLocationDto;
  host: PropertyDetailHostDto;
  lastReviews: PropertyDetailReviewDto[];
  multimedia: PropertyDetailAssetDto[];
}

export const dtoToPropertyDetail = (dto: PropertyDetailDto): PropertyDetail => {
  return {
    id: dto.id,
    title: dto.title,
    description: dto.description,
    guests: dto.guests,
    bedrooms: dto.bedrooms,
    bathrooms: dto.bathrooms,
    beds: dto.beds,
    price: dto.pricePerNight,
    type: dto.type,
    location: {
      address: dto.location.address,
      city: dto.location.city,
      country: dto.location.country,
      latitude: dto.location.latitude,
      longitude: dto.location.longitude,
    },
    host: {
      fullName: dto.host.fullName,
      createdAt: new Date(dto.host.createdAt),
    },
    rating: dto.rating,
    reviewsCount: dto.reviewsCount,
    reviews: dto.lastReviews.map((review) => ({
      guestFullName: review.guestFullName,
      rating: review.rating,
      comment: review.comment,
      createdAt: new Date(review.createdAt),
    })),
    multimedia: dto.multimedia.map((asset) => ({
      url: asset.url,
      type: asset.type,
    })),
  };
};
