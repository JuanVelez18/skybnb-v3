import { create } from "zustand";

import type { PropertyFilters, SortBy } from "@/models/properties";

type State = {
  filters: PropertyFilters;
};

type Action = {
  updatePriceRange: (min: number, max: number) => void;
  updatePropertyTypes: (types: number[]) => void;
  updateBedrooms: (bedrooms?: string) => void;
  updateBathrooms: (bathrooms?: string) => void;
  tooglePropertyType: (type: number) => void;
  updateLocation: (location: string) => void;
  updateCheckIn: (checkIn?: Date) => void;
  updateCheckOut: (checkOut?: Date) => void;
  updateGuests: (guests?: string) => void;
  updateSortBy: (sortBy: SortBy) => void;
};

const stringToInteger = (str?: string): number | undefined => {
  if (!str) return undefined;

  const parsed = parseInt(str, 10);
  return isNaN(parsed) ? undefined : parsed;
};

export const usePropertiesStore = create<State & Action>((set) => ({
  filters: {
    minPrice: 0,
    maxPrice: 500,
    types: [],
    bedrooms: undefined,
    bathrooms: undefined,
    location: undefined,
    checkIn: undefined,
    checkOut: undefined,
    guests: 1,
    sortBy: "newest",
  },

  updateLocation: (location) =>
    set((state) => ({
      filters: { ...state.filters, location: location || undefined },
    })),

  updatePriceRange: (min, max) =>
    set((state) => ({
      filters: { ...state.filters, minPrice: min, maxPrice: max },
    })),

  updatePropertyTypes: (types) =>
    set((state) => ({ filters: { ...state.filters, types } })),

  updateBedrooms: (bedrooms) =>
    set((state) => ({
      filters: { ...state.filters, bedrooms: stringToInteger(bedrooms) },
    })),

  updateBathrooms: (bathrooms) =>
    set((state) => ({
      filters: { ...state.filters, bathrooms: stringToInteger(bathrooms) },
    })),

  tooglePropertyType: (type) =>
    set((state) => {
      if (!state.filters.types)
        return { filters: { ...state.filters, types: [type] } };

      const types = state.filters.types.includes(type)
        ? state.filters.types.filter((t) => t !== type)
        : [...state.filters.types, type];

      return { filters: { ...state.filters, types } };
    }),

  updateCheckIn: (checkIn) =>
    set((state) => ({ filters: { ...state.filters, checkIn } })),

  updateCheckOut: (checkOut) =>
    set((state) => ({ filters: { ...state.filters, checkOut } })),

  updateGuests: (guests) =>
    set((state) => ({
      filters: { ...state.filters, guests: stringToInteger(guests) },
    })),

  updateSortBy: (sortBy) =>
    set((state) => ({ filters: { ...state.filters, sortBy } })),
}));
