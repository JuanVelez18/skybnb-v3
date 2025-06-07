import { Card, CardContent } from "@/components/ui/card";
import { Star } from "lucide-react";

const NoReviewsCard = () => {
  return (
    <Card className="border-dashed">
      <CardContent className="flex flex-col items-center justify-center py-12 text-center">
        <div className="rounded-full bg-muted p-3 mb-4">
          <Star className="h-8 w-8 text-muted-foreground" />
        </div>
        <h4 className="text-lg font-semibold mb-2">No reviews yet</h4>
        <p className="text-muted-foreground mb-4 max-w-md">
          This property hasn't received any reviews yet. Be the first guest to
          stay here and share your experience!
        </p>
        <p className="text-sm text-muted-foreground">
          Reviews help other travelers make informed decisions and provide
          valuable feedback to hosts.
        </p>
      </CardContent>
    </Card>
  );
};

export default NoReviewsCard;
