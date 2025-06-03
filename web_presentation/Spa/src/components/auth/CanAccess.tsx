import { useGetUserSummary } from "@/queries/users.queries";
import { useAuthStore } from "@/stores/auth.store";
import { hasPermission } from "@/utils/auth";
import type { PropsWithChildren } from "react";

type Props = PropsWithChildren<{
  permission?: string;
}>;

const CanAccess = ({ permission, children }: Props) => {
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
  const { user } = useGetUserSummary(isAuthenticated);

  if (!permission) return children;

  if (!isAuthenticated || !user) return null;

  if (hasPermission(user, permission)) {
    return children;
  }
};

export default CanAccess;
