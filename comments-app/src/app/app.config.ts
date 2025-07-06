import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { InjectionToken } from '@angular/core';

export const APP_CONFIG = new InjectionToken<ApplicationConfig>('app.config');

export const appConfig: ApplicationConfig = {
  providers: [
    importProvidersFrom(BrowserModule),
  ]
};