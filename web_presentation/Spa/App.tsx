import { RouterProvider } from "react-router-dom";
import { useEffect } from "react";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";

import { router } from "./src/router";
import { initializeSession } from "./src/utils/auth";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

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
      </QueryClientProvider>
    </>
  );
}

export default App;
