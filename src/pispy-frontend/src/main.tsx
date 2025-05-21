import "./index.css"; // ← EXACT so, sonst lädst du kein Tailwind-CSS
import "preline"; // nur JS, keine weiteren CSS-Imports
import App from "./App";
import { StrictMode } from "react";
import { createRoot } from "react-dom/client";

const rootElement = document.getElementById("root");
if (rootElement) {
  createRoot(rootElement).render(
    <StrictMode>
      <App />
    </StrictMode>
  );
} else {
  throw new Error("Root element not found");
}
