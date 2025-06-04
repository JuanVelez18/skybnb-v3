import { MapPin } from "lucide-react";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";

type Props = {
  location: string;
};

const LocationCard = ({ location }: Props) => {
  return (
    <Card>
      <CardHeader>
        <CardTitle>Location</CardTitle>
      </CardHeader>
      <CardContent>
        <div className="space-y-3">
          <p className="flex items-center gap-2">
            <MapPin className="h-4 w-4" />
            {location}
          </p>
          <div className="h-48 bg-muted rounded-lg flex items-center justify-center">
            <p className="text-muted-foreground">Map placeholder</p>
          </div>
        </div>
      </CardContent>
    </Card>
  );
};

export default LocationCard;
