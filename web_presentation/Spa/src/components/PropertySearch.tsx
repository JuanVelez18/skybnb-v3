import { CalendarIcon, Filter, MapPin, Search } from "lucide-react";

import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Card, CardContent } from "@/components/ui/card";
import { Calendar } from "@/components/ui/calendar";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { usePropertiesStore } from "@/stores/properties.store";
import { cn } from "@/lib/utils";
import { dateToLocaleString, isPastDate } from "@/utils/dates";

type Props = {
  onFiltersClick: () => void;
  onSearch: () => void;
};

const PropertySearch = ({ onFiltersClick, onSearch }: Props) => {
  const {
    filters,
    updateLocation,
    updateCheckIn,
    updateCheckOut,
    updateGuests,
  } = usePropertiesStore();

  const isDisabledCheckInDate = (date: Date) => {
    return isPastDate(date);
  };

  const isDisabledCheckOutDate = (date: Date) => {
    return isPastDate(date) || (!!filters.checkIn && date <= filters.checkIn);
  };

  return (
    <Card>
      <CardContent className="p-6">
        <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
          <div className="space-y-2">
            <Label htmlFor="location">Where</Label>
            <div className="relative">
              <MapPin className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-muted-foreground" />
              <Input
                id="location"
                placeholder="Search destinations"
                value={filters.location}
                onChange={(e) => updateLocation(e.target.value)}
                className="pl-10"
              />
            </div>
          </div>

          <div className="space-y-2">
            <Label>Check-in</Label>
            <Popover>
              <PopoverTrigger asChild>
                <Button
                  variant={"outline"}
                  className={cn(
                    "w-full pl-3 text-left font-normal",
                    !filters.checkIn && "text-muted-foreground"
                  )}
                >
                  {filters.checkIn ? (
                    dateToLocaleString(filters.checkIn)
                  ) : (
                    <span>mm/dd/yyyy</span>
                  )}
                  <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                </Button>
              </PopoverTrigger>
              <PopoverContent className="w-auto p-0" align="start">
                <Calendar
                  mode="single"
                  selected={filters.checkIn}
                  onSelect={updateCheckIn}
                  disabled={isDisabledCheckInDate}
                />
              </PopoverContent>
            </Popover>
          </div>

          <div className="space-y-2">
            <Label>Check-out</Label>
            <Popover>
              <PopoverTrigger asChild>
                <Button
                  variant={"outline"}
                  className={cn(
                    "w-full pl-3 text-left font-normal",
                    !filters.checkOut && "text-muted-foreground"
                  )}
                >
                  {filters.checkOut ? (
                    dateToLocaleString(filters.checkOut)
                  ) : (
                    <span>mm/dd/yyyy</span>
                  )}
                  <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                </Button>
              </PopoverTrigger>
              <PopoverContent className="w-auto p-0" align="start">
                <Calendar
                  mode="single"
                  selected={filters.checkOut}
                  onSelect={updateCheckOut}
                  disabled={isDisabledCheckOutDate}
                />
              </PopoverContent>
            </Popover>
          </div>

          <div className="space-y-2">
            <Label htmlFor="guests">Guests</Label>
            <Select
              value={filters.guests?.toString() ?? ""}
              onValueChange={updateGuests}
            >
              <SelectTrigger className="w-full">
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                {[1, 2, 3, 4, 5, 6, 7, 8].map((num) => (
                  <SelectItem key={num} value={num.toString()}>
                    {num} {num === 1 ? "guest" : "guests"}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>
        </div>

        <div className="flex justify-between items-center mt-4">
          <Button
            variant="outline"
            onClick={onFiltersClick}
            className="flex items-center gap-2"
          >
            <Filter className="h-4 w-4" />
            Filters
            {filters.types && filters.types.length > 0 && (
              <Badge variant="secondary" className="ml-2">
                {filters.types.length}
              </Badge>
            )}
          </Button>

          <Button className="flex items-center gap-2" onClick={onSearch}>
            <Search className="h-4 w-4" />
            Search
          </Button>
        </div>
      </CardContent>
    </Card>
  );
};

export default PropertySearch;
