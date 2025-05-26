import { type UserConfig, defineConfig } from "vite";

import react from "@vitejs/plugin-react";
import tailwindcss from "@tailwindcss/vite";

// Pattern for CSS files
const cssPattern = /\.css$/;
// Pattern for image files
const imagePattern = /\.(png|jpe?g|gif|svg|webp|avif)$/;

// Export Vite configuration
export default defineConfig(async () => {
  // Define Vite configuration
  const config: UserConfig = {
    plugins: [react(), tailwindcss()],
    root: "Spa",
    publicDir: "public",
    build: {
      manifest: true,
      emptyOutDir: false,
      outDir: "../wwwroot",
      rollupOptions: {
        input: "main.tsx",
        output: {
          // Save entry files to the appropriate folder
          entryFileNames: "spa-js/[name].[hash].js",
          // Save chunk files to the js folder
          chunkFileNames: "spa-js/[name]-chunk.js",
          // Save asset files to the appropriate folder
          assetFileNames: (info) => {
            if (info.name) {
              // If the file is a CSS file, save it to the css folder
              if (cssPattern.test(info.name)) {
                return "spa-css/[name][extname]";
              }
              // If the file is an image file, save it to the images folder
              if (imagePattern.test(info.name)) {
                return "spa-images/[name][extname]";
              }

              // If the file is any other type of file, save it to the assets folder
              return "spa-assets/[name][extname]";
            } else {
              // If the file name is not specified, save it to the output directory
              return "spa-[name][extname]";
            }
          },
        },
      },
    },
    server: {
      strictPort: true,
    },
    optimizeDeps: {
      include: [],
    },
  };

  return config;
});
