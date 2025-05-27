import { RouterProvider } from "react-router-dom";
import { useEffect } from "react";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { Toaster } from "@/components/ui/sonner";

import { router } from "./src/router";
import { initializeSession } from "./src/utils/auth";

const queryClient = new QueryClient();

function App() {
  useEffect(() => {
    initializeSession();
  }, []);

  return (
    <>
      <QueryClientProvider client={queryClient}>
        <RouterProvider router={router} />
        <ReactQueryDevtools initialIsOpen={false} />
        <Toaster />
      </QueryClientProvider>
    </>
  );
}

export default App;
