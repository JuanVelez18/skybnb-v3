import { storage } from "@/core/firebaseClient";
import type { CreationMediaFile } from "@/models/multimedia";
import { getDownloadURL, ref, uploadBytes } from "firebase/storage";

export class MultimediaService {
  public static async uploadMultimedia(
    files: CreationMediaFile[]
  ): Promise<string[]> {
    const responses = await Promise.allSettled(
      files.map(async (file) => {
        const filePath = `${file.type}/${crypto.randomUUID()}`;
        const storageRef = ref(storage, filePath);

        const snapshot = await uploadBytes(storageRef, file.file);
        const downloadURL = await getDownloadURL(snapshot.ref);

        return downloadURL;
      })
    );

    return responses
      .filter(
        (response): response is PromiseFulfilledResult<string> =>
          response.status === "fulfilled"
      )
      .map((response) => response.value);
  }
}
