import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import path from "path";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [plugin()],
  resolve: {
    alias: {
      "@": path.resolve(__dirname, "src"), 
    },
  },
  server: {
    port: 58482,
  }, 
  
});
