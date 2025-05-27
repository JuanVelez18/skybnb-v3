import { UserService } from "@/services/user.service";
import { useQuery } from "@tanstack/react-query";

export const UserQueryKeys = {
  all: ["users"] as const,
  me: () => [...UserQueryKeys.all, "me"] as const,
};

export const useGetUserSummary = () => {
  const { data, isLoading, isError } = useQuery({
    queryKey: UserQueryKeys.me(),
    queryFn: async () => UserService.getUserSummary(),
    staleTime: 1000 * 60 * 5, // 5 minutes
  });

  return {
    user: data,
    isUserLoading: isLoading,
    isUserError: isError,
  };
};
