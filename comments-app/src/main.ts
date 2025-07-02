import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig, APP_CONFIG } from './app/app.config';
import { App } from './app/app';
import { provideHttpClient, withFetch } from '@angular/common/http';

bootstrapApplication(App, {
  providers: [
    provideHttpClient(withFetch()),
    { provide: APP_CONFIG, useValue: appConfig }
  ]
});