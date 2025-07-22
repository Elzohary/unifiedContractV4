import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { appConfig } from './app/app.config';

// Add error handling for uncaught errors
window.addEventListener('error', (event) => {
  console.error('Uncaught error:', event.error);
});

// Add error handling for unhandled promise rejections
window.addEventListener('unhandledrejection', (event) => {
  console.error('Unhandled promise rejection:', event.reason);
});

// Bootstrap the application using appConfig
bootstrapApplication(AppComponent, appConfig)
  .catch((err) => {
    console.error('Error bootstrapping application:', err);
    // Display a user-friendly error message
    const errorElement = document.createElement('div');
    errorElement.style.cssText = `
      position: fixed;
      top: 0;
      left: 0;
      right: 0;
      padding: 20px;
      background-color: #f44336;
      color: white;
      text-align: center;
      z-index: 9999;
    `;
    errorElement.textContent = 'An error occurred while loading the application. Please try refreshing the page.';
    document.body.appendChild(errorElement);
  });
