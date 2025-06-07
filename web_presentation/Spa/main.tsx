import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { initializeSession } from "@/utils/auth.ts";
import App from "./App.tsx";

import "./index.css";

initializeSession();
createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <App />
  </StrictMode>
);
