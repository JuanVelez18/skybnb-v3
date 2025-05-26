import { RouterProvider } from "react-router-dom";
import { router } from "./src/router";
import { useEffect } from "react";
import { initializeSession } from "./src/utils/auth";

function App() {
  useEffect(() => {
    initializeSession();
  }, []);

  return (
    <>
      <RouterProvider router={router} />
    </>
  );
}

export default App;
