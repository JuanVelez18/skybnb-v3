import { GripVertical, ImageIcon, Play, Upload, X } from "lucide-react";

import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
  CardDescription,
} from "@/components/ui/card";
import type { CreationMediaFile } from "@/models/multimedia";
import { useCallback, useState } from "react";

type MediaFile = {
  id: string;
  file: File;
  preview: string;
  type: "image" | "video";
};

type Props = {
  formId?: string;
  initialValues?: CreationMediaFile[] | null;
  onSubmit(files: CreationMediaFile[]): void;
};

const modelToMediaFile = (files: CreationMediaFile[]): MediaFile[] => {
  return files.map((file) => ({
    id: crypto.randomUUID(),
    file: file.file,
    preview: URL.createObjectURL(file.file),
    type: file.type,
  }));
};

const PropertyMultimediaForm = ({ formId, initialValues, onSubmit }: Props) => {
  const [mediaFiles, setMediaFiles] = useState<MediaFile[]>(
    initialValues ? modelToMediaFile(initialValues) : []
  );
  const [isDragOver, setIsDragOver] = useState(false);
  const [error, setError] = useState("");

  const handleSubmit = (
    e: React.FormEvent<HTMLFormElement>,
    data: MediaFile[]
  ) => {
    e.preventDefault();

    const creationFiles: CreationMediaFile[] = [];
    let hasImage = false;

    data.forEach((file) => {
      creationFiles.push({
        file: file.file,
        type: file.type,
      });

      if (file.type === "image") hasImage = true;

      URL.revokeObjectURL(file.preview);
    });
    if (!hasImage) {
      setError("Please upload at least one image");
      return;
    }

    onSubmit(mediaFiles);
  };

  const handleFileUpload = useCallback(
    (files: FileList) => {
      const newFiles: MediaFile[] = [];
      let hasImage = false;

      Array.from(files).forEach((file) => {
        if (file.type.startsWith("image/") || file.type.startsWith("video/")) {
          const id = crypto.randomUUID();
          const preview = URL.createObjectURL(file);
          const type = file.type.startsWith("image/") ? "image" : "video";
          if (type === "image") hasImage = true;

          newFiles.push({
            id,
            file,
            preview,
            type,
          });
        }
      });
      if (hasImage && error) setError("");

      setMediaFiles((prev) => [...prev, ...newFiles]);
    },
    [error]
  );

  const handleDragOver = useCallback((e: React.DragEvent) => {
    e.preventDefault();
    setIsDragOver(true);
  }, []);

  const handleDragLeave = useCallback((e: React.DragEvent) => {
    e.preventDefault();
    setIsDragOver(false);
  }, []);

  const handleDrop = useCallback(
    (e: React.DragEvent) => {
      e.preventDefault();
      setIsDragOver(false);

      const files = e.dataTransfer.files;
      handleFileUpload(files);
    },
    [handleFileUpload]
  );

  const removeFile = (id: string) => {
    setMediaFiles((prev) => {
      const updated = prev.filter((file) => file.id !== id);
      return updated;
    });
  };

  const moveFile = (fromIndex: number, toIndex: number) => {
    setMediaFiles((prev) => {
      const updated = [...prev];
      const [movedFile] = updated.splice(fromIndex, 1);
      updated.splice(toIndex, 0, movedFile);
      return updated;
    });
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle>Media Upload</CardTitle>
        <CardDescription>
          Upload high-quality photos and videos to showcase your property to
          potential guests.
        </CardDescription>
      </CardHeader>
      <CardContent>
        <form
          id={formId}
          onSubmit={(e) => handleSubmit(e, mediaFiles)}
          className="space-y-6"
        >
          {/* File Upload Area */}
          <div className="space-y-4">
            <div className="flex items-center justify-between">
              <div>
                <div className="flex items-center gap-2">
                  <h3 className="text-lg font-medium">Property Media</h3>
                  {error && (
                    <p className="text-sm text-red-500 mt-1">{error}</p>
                  )}
                </div>
                <p className="text-sm text-muted-foreground">
                  Upload images and videos to showcase your property. At least
                  one image is required.
                </p>
              </div>
              <Badge variant="secondary">
                {mediaFiles.length} files uploaded
              </Badge>
            </div>

            {/* Drag and Drop Area */}
            <div
              className={`relative border-2 border-dashed rounded-lg p-8 text-center transition-colors ${
                isDragOver
                  ? "border-primary bg-primary/5"
                  : "border-muted-foreground/25 hover:border-muted-foreground/50"
              }`}
              onDragOver={handleDragOver}
              onDragLeave={handleDragLeave}
              onDrop={handleDrop}
            >
              <Upload className="mx-auto h-12 w-12 text-muted-foreground mb-4" />
              <div className="space-y-2">
                <p className="text-lg font-medium">Drop your files here</p>
                <p className="text-sm text-muted-foreground">
                  or{" "}
                  <label className="text-primary hover:text-primary/80 cursor-pointer underline">
                    browse files
                    <input
                      type="file"
                      multiple
                      accept="image/*,video/*"
                      className="hidden"
                      onChange={(e) => {
                        if (e.target.files) {
                          handleFileUpload(e.target.files);
                        }
                      }}
                    />
                  </label>
                </p>
                <p className="text-xs text-muted-foreground">
                  Supports: JPG, PNG, GIF, MP4, MOV (Max 10MB per file)
                </p>
              </div>
            </div>

            {/* File Preview Grid */}
            {mediaFiles.length > 0 && (
              <div className="space-y-4">
                <div className="flex items-center justify-between">
                  <h4 className="font-medium">Uploaded Files</h4>
                  <p className="text-sm text-muted-foreground">
                    Drag to reorder • First image will be the cover photo
                  </p>
                </div>

                <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
                  {mediaFiles.map((file, index) => (
                    <div
                      key={file.id}
                      className="relative group bg-muted rounded-lg overflow-hidden aspect-square"
                      draggable
                      onDragStart={(e) => {
                        e.dataTransfer.setData("text/plain", index.toString());
                      }}
                      onDragOver={(e) => {
                        e.preventDefault();
                      }}
                      onDrop={(e) => {
                        e.preventDefault();
                        const fromIndex = Number.parseInt(
                          e.dataTransfer.getData("text/plain")
                        );
                        if (fromIndex !== index) {
                          moveFile(fromIndex, index);
                        }
                      }}
                    >
                      {/* Cover Photo Badge */}
                      {index === 0 && (
                        <Badge className="absolute top-2 left-2 z-10 text-xs">
                          Cover Photo
                        </Badge>
                      )}

                      {/* File Type Icon */}
                      <div className="absolute top-2 right-2 z-10">
                        {file.type === "image" ? (
                          <ImageIcon className="h-4 w-4 text-white drop-shadow-lg" />
                        ) : (
                          <Play className="h-4 w-4 text-white drop-shadow-lg" />
                        )}
                      </div>

                      {/* Preview */}
                      {file.type === "image" ? (
                        <img
                          src={file.preview || "/placeholder.svg"}
                          alt={`Preview ${index + 1}`}
                          className="w-full h-full object-cover"
                        />
                      ) : (
                        <video
                          src={file.preview}
                          className="w-full h-full object-cover"
                          muted
                        />
                      )}

                      {/* Overlay with controls */}
                      <div className="absolute inset-0 bg-black/50 opacity-0 group-hover:opacity-100 transition-opacity flex items-center justify-center">
                        <div className="flex items-center gap-2">
                          <Button
                            type="button"
                            size="sm"
                            variant="secondary"
                            className="h-8 w-8 p-0"
                          >
                            <GripVertical className="h-4 w-4" />
                          </Button>
                          <Button
                            type="button"
                            size="sm"
                            variant="destructive"
                            className="h-8 w-8 p-0"
                            onClick={() => removeFile(file.id)}
                          >
                            <X className="h-4 w-4" />
                          </Button>
                        </div>
                      </div>

                      {/* File info */}
                      <div className="absolute bottom-0 left-0 right-0 bg-black/75 text-white p-2">
                        <p className="text-xs truncate">{file.file.name}</p>
                        <p className="text-xs text-gray-300">
                          {(file.file.size / 1024 / 1024).toFixed(1)} MB
                        </p>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            )}

            {/* Upload Guidelines */}
            <div className="bg-muted/50 rounded-lg p-4">
              <h4 className="font-medium mb-2">Photo Guidelines</h4>
              <ul className="text-sm text-muted-foreground space-y-1">
                <li>• Use high-quality, well-lit photos</li>
                <li>• Show different angles and rooms of your property</li>
                <li>• Include exterior and interior shots</li>
                <li>• The first image will be used as the main cover photo</li>
                <li>• Videos should highlight unique features or amenities</li>
              </ul>
            </div>
          </div>
        </form>
      </CardContent>
    </Card>
  );
};

export default PropertyMultimediaForm;
