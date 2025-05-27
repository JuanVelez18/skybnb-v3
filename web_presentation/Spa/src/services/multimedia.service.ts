import { storage } from "@/core/firebaseClient";
import type { CreationMediaFile } from "@/models/multimedia";
import type { MediaFileDto } from "@/models/properties";
import { getDownloadURL, ref, uploadBytes } from "firebase/storage";

export class MultimediaService {
  public static async uploadMultimedia(
    files: CreationMediaFile[]
  ): Promise<MediaFileDto[]> {
    const responses = await Promise.allSettled(
      files.map(async (file) => {
        const filePath = `${file.type}/${crypto.randomUUID()}`;
        const storageRef = ref(storage, filePath);

        const snapshot = await uploadBytes(storageRef, file.file);
        const downloadURL = await getDownloadURL(snapshot.ref);

        return {
          url: downloadURL,
          type: file.type,
        };
      })
    );

    return responses
      .filter(
        (
          response
        ): response is PromiseFulfilledResult<{
          url: string;
          type: "image" | "video";
        }> => response.status === "fulfilled"
      )
      .map((response, index) => ({
        Url: response.value.url,
        Type: response.value.type,
        Order: index + 1,
      }));
  }
}
