import { type UserConfig, defineConfig } from "vite";

import react from "@vitejs/plugin-react";
import tailwindcss from "@tailwindcss/vite";
import checker from "vite-plugin-checker";
import path from "path";

// Pattern for CSS files
const cssPattern = /\.css$/;
// Pattern for image files
const imagePattern = /\.(png|jpe?g|gif|svg|webp|avif)$/;

// Export Vite configuration
export default defineConfig(async () => {
  // Define Vite configuration
  const config: UserConfig = {
    plugins: [
      react(),
      tailwindcss(),
      checker({
        typescript: true,
      }),
    ],
    root: "Spa",
    publicDir: "public",
    build: {
      manifest: true,
      emptyOutDir: false,
      outDir: "../wwwroot/spa",
      rollupOptions: {
        input: "Spa/main.tsx",
        output: {
          // Save entry files to the appropriate folder
          entryFileNames: "js/[name].[hash].js",
          // Save chunk files to the js folder
          chunkFileNames: "js/[name]-chunk.js",
          // Save asset files to the appropriate folder
          assetFileNames: (info) => {
            if (info.name) {
              // If the file is a CSS file, save it to the css folder
              if (cssPattern.test(info.name)) {
                return "css/[name][extname]";
              }
              // If the file is an image file, save it to the images folder
              if (imagePattern.test(info.name)) {
                return "images/[name][extname]";
              }

              // If the file is any other type of file, save it to the assets folder
              return "assets/[name][extname]";
            } else {
              // If the file name is not specified, save it to the output directory
              return "[name][extname]";
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
    resolve: {
      alias: {
        "@": path.resolve(__dirname, "./Spa/src"),
      },
    },
  };

  return config;
});
