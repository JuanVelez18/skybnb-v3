import { RouterProvider } from "react-router-dom";
import { useEffect } from "react";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import { QueryClientProvider } from "@tanstack/react-query";
import { Toaster } from "@/components/ui/sonner";

import { router } from "./src/router";
import { initializeSession } from "@/utils/auth";
import { queryClient } from "@/core/queryClient";

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
