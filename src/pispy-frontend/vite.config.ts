import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import tailwindcss from '@tailwindcss/vite';

export default defineConfig({
  plugins: [react(), tailwindcss()],
  optimizeDeps: {
    include: ['preline']
  }
  /* server: {
    proxy: {
      '/alarmHub': {
        target: 'http://localhost:5272',
        ws: true,
        changeOrigin: true,
        rewriteWsOrigin: true,
        secure: false,             
      }
    }
} */});
