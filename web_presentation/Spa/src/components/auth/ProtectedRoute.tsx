import { useGetUserSummary } from "@/queries/users.queries";
import { useAuthStore } from "@/stores/auth.store";
import { type PropsWithChildren } from "react";
import { Navigate } from "react-router-dom";
import Loader from "../common/Loader";
import { RouteNames } from "@/router/routes";

type Props = PropsWithChildren<{
  authenticated?: boolean;
  permission?: string;
}>;

const ProtectedRoute = ({ authenticated, permission, children }: Props) => {
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
  const { user, isUserLoading } = useGetUserSummary(isAuthenticated);

  if (authenticated && !isAuthenticated) {
    return <Navigate to={RouteNames.NOT_FOUND} replace />;
  }

  if (permission) {
    if (!isAuthenticated) {
      return <Navigate to={RouteNames.NOT_FOUND} replace />;
    }

    if (isUserLoading) {
      return <Loader />;
    }

    if (!user || !user.permissions.includes(permission)) {
      return <Navigate to={RouteNames.NOT_FOUND} replace />;
    }
  }

  return <>{children}</>;
};

export default ProtectedRoute;
