import { UserService } from "@/services/user.service";
import { useQuery } from "@tanstack/react-query";

export const UserQueryKeys = {
  all: ["users"] as const,
  me: () => [...UserQueryKeys.all, "me"] as const,
};

export const useGetUserSummary = (enabled?: boolean) => {
  const { data, isLoading, isError } = useQuery({
    queryKey: UserQueryKeys.me(),
    queryFn: async () => UserService.getUserSummary(),
    enabled: enabled ?? true,
  });

  return {
    user: data,
    isUserLoading: isLoading,
    isUserError: isError,
  };
};
