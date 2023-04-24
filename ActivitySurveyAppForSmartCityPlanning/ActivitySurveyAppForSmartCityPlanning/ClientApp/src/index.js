import React from "react";
import ReactDOM from "react-dom/client";
import { RecoilRoot } from "recoil";
import App from "./App";
import { QueryClient, QueryClientProvider } from "react-query";

// set up react query
const queryClient = new QueryClient();

const root = ReactDOM.createRoot(document.getElementById("root"));

// set up recoil for state management
root.render(
  <React.StrictMode>
    <RecoilRoot>
      <QueryClientProvider client={queryClient}>
        <App />
      </QueryClientProvider>
    </RecoilRoot>
  </React.StrictMode>
);
