import { useState } from "react";

const HomePage = () => {
  const [count, setCount] = useState(0);

  return (
    <div className="text-center">
      <h1 className="text-4xl font-bold text-gray-900 mb-8">
        Welcome to SkyBnB
      </h1>

      <div className="flex justify-center gap-4 mb-8">
        <a href="https://vite.dev" target="_blank" rel="noopener noreferrer">
          <img src="/vite.svg" className="h-16 w-16" alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank" rel="noopener noreferrer">
          <img
            src="/src/assets/react.svg"
            className="h-16 w-16"
            alt="React logo"
          />
        </a>
      </div>

      <div className="bg-white p-6 rounded-lg shadow-md max-w-md mx-auto">
        <button
          onClick={() => setCount((count) => count + 1)}
          className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded mb-4"
        >
          count is {count}
        </button>
        <p className="text-gray-600">
          Edit{" "}
          <code className="bg-gray-100 px-2 py-1 rounded">
            src/pages/HomePage.tsx
          </code>{" "}
          and save to test HMR
        </p>
      </div>

      <p className="text-gray-500 mt-8">
        Click on the Vite and React logos to learn more
      </p>
    </div>
  );
};

export default HomePage;
