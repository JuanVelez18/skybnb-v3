import { createBrowserRouter, type RouteObject } from "react-router-dom";
import Layout from "../components/Layout.tsx";
import HomePage from "../pages/HomePage.tsx";
import NotFoundPage from "../pages/NotFoundPage.tsx";
import { RouteNames } from "./routes.ts";
import PropertyCreationPage from "@/pages/PropertyCreationPage.tsx";

// Centralized route configuration
export const routes: RouteObject[] = [
  {
    path: RouteNames.HOME,
    element: <Layout />,
    children: [
      {
        index: true,
        element: <HomePage />,
      },
      {
        path: RouteNames.CREATE_PROPERTY,
        element: <PropertyCreationPage />,
      },
    ],
  },
  {
    path: "*",
    element: <NotFoundPage />,
  },
];

// Create the router instance
export const router = createBrowserRouter(routes);
