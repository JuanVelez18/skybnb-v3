import { Star } from "lucide-react";

import { Avatar, AvatarImage, AvatarFallback } from "@/components/ui/avatar";

type Review = {
  author: string;
  avatar?: string;
  rating: number;
  createdAt: Date;
  comment: string;
};

type Props = {
  reviews: Review[];
};

const ReviewsSummarySection = ({ reviews }: Props) => {
  return (
    <div className="space-y-6">
      {reviews.map((review) => (
        <div key={review.comment} className="space-y-3">
          <div className="flex items-center gap-3">
            <Avatar>
              <AvatarImage
                src={review.avatar || "/user-placeholder.svg"}
                alt={review.author}
              />
              <AvatarFallback>{review.author.charAt(0)}</AvatarFallback>
            </Avatar>
            <div>
              <p className="font-medium">{review.author}</p>
              <div className="flex items-center gap-2">
                <div className="flex">
                  {Array.from({ length: 5 }).map((_, i) => (
                    <Star
                      key={i}
                      className={`h-3 w-3 ${
                        i < review.rating
                          ? "fill-yellow-400 text-yellow-400"
                          : "text-gray-300"
                      }`}
                    />
                  ))}
                </div>
                <span className="text-sm text-muted-foreground">
                  {review.createdAt.toLocaleDateString("en-US", {
                    month: "long",
                  })}{" "}
                  {review.createdAt.getFullYear()}
                </span>
              </div>
            </div>
          </div>
          <p className="text-muted-foreground leading-relaxed">
            {review.comment}
          </p>
        </div>
      ))}
    </div>
  );
};

export default ReviewsSummarySection;
