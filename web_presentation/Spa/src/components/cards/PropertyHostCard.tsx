import { Avatar, AvatarImage, AvatarFallback } from "@/components/ui/avatar";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";

type Props = {
  name: string;
  yearSince: string;
  avatar?: string;
};

const PropertyHostCard = ({ name, yearSince, avatar }: Props) => {
  return (
    <Card>
      <CardHeader>
        <CardTitle>Meet your host</CardTitle>
      </CardHeader>
      <CardContent className="space-y-4">
        <div className="flex items-center gap-4">
          <Avatar className="h-16 w-16">
            <AvatarImage src={avatar || "/user-placeholder.svg"} alt={name} />
            <AvatarFallback>{name.charAt(0)}</AvatarFallback>
          </Avatar>
          <div>
            <h4 className="font-semibold text-lg">{name}</h4>
            <p className="text-sm text-muted-foreground">
              Host since {yearSince}
            </p>
          </div>
        </div>
      </CardContent>
    </Card>
  );
};

export default PropertyHostCard;
