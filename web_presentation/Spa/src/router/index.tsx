import { createBrowserRouter, type RouteObject } from "react-router-dom";
import Layout from "../components/Layout.tsx";
import HomePage from "../pages/HomePage.tsx";
import NotFoundPage from "../pages/NotFoundPage.tsx";
import { RouteNames } from "./routes.ts";
import PropertyCreationPage from "@/pages/PropertyCreationPage.tsx";
import ProtectedRoute from "@/components/auth/ProtectedRoute.tsx";
import PropertyDetailPage from "@/pages/PropertyDetailPage.tsx";

export interface RouteConfig {
  layoutConfig?: {
    title?: string;
    backTo?: string;
  };
}

// Centralized route configuration
export const routes: RouteObject[] = [
  {
    element: <Layout />,
    children: [
      {
        path: RouteNames.HOME,
        element: <HomePage />,
        handle: {
          layoutConfig: {
            title: "Search Properties",
          },
        } as RouteConfig,
      },
      {
        path: RouteNames.PROPERTY_DETAIL,
        element: <PropertyDetailPage />,
        handle: {
          layoutConfig: {
            title: "Property Details",
            backTo: RouteNames.HOME,
          },
        } as RouteConfig,
      },
      {
        path: RouteNames.CREATE_PROPERTY,
        element: (
          <ProtectedRoute authenticated permission="create:property">
            <PropertyCreationPage />
          </ProtectedRoute>
        ),
        handle: {
          layoutConfig: {
            title: "Create Property",
          },
        } as RouteConfig,
      },
      {
        path: "*",
        element: <NotFoundPage />,
      },
    ],
  },
];

// Create the router instance
export const router = createBrowserRouter(routes);
