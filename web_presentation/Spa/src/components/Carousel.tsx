import { useState } from "react";

import { Button } from "./ui/button";
import { ChevronLeft, ChevronRight } from "lucide-react";

type Asset = {
  url: string;
  type: string;
};

type Props = {
  assets: Asset[];
};

const Carousel = ({ assets }: Props) => {
  const [currentImageIndex, setCurrentImageIndex] = useState(0);

  const nextImage = () => {
    setCurrentImageIndex((prev) => (prev + 1) % assets.length);
  };

  const prevImage = () => {
    setCurrentImageIndex((prev) => (prev - 1 + assets.length) % assets.length);
  };

  return (
    <div className="relative">
      <div className="relative h-96 md:h-[500px] rounded-xl overflow-hidden">
        {assets[currentImageIndex].type === "image" && (
          <img
            src={assets[currentImageIndex].url || "/placeholder.svg"}
            alt={`Property image ${currentImageIndex + 1}`}
            className="w-full h-full object-cover"
          />
        )}

        {assets[currentImageIndex].type === "video" && (
          <video
            src={assets[currentImageIndex].url}
            className="w-full h-full object-cover"
            controls
          />
        )}

        {/* Navigation Buttons */}
        <Button
          variant="secondary"
          size="icon"
          className="absolute left-4 top-1/2 transform -translate-y-1/2 bg-white/80 hover:bg-white"
          onClick={prevImage}
        >
          <ChevronLeft className="h-4 w-4" />
        </Button>
        <Button
          variant="secondary"
          size="icon"
          className="absolute right-4 top-1/2 transform -translate-y-1/2 bg-white/80 hover:bg-white"
          onClick={nextImage}
        >
          <ChevronRight className="h-4 w-4" />
        </Button>
        {/* Image Counter */}
        <div className="absolute bottom-4 right-4 bg-black/50 text-white px-3 py-1 rounded-full text-sm">
          {currentImageIndex + 1} / {assets.length}
        </div>
      </div>

      {/* Thumbnail Strip */}
      <div className="flex gap-2 mt-4 overflow-x-auto pb-2">
        {assets.map((image, index) => (
          <button
            key={index}
            onClick={() => setCurrentImageIndex(index)}
            className={`flex-shrink-0 w-20 h-20 rounded-lg overflow-hidden border-2 transition-colors ${
              index === currentImageIndex
                ? "border-primary"
                : "border-transparent"
            }`}
          >
            {image.type === "image" && (
              <img
                src={image.url || "/placeholder.svg"}
                alt={`Thumbnail ${index + 1}`}
                className="w-full h-full object-cover"
              />
            )}

            {image.type === "video" && (
              <video
                src={image.url}
                className="w-full h-full object-cover"
                muted
              />
            )}
          </button>
        ))}
      </div>
    </div>
  );
};

export default Carousel;
