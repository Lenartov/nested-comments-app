import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter, withEnabledBlockingInitialNavigation } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { InjectionToken } from '@angular/core';

import { CommentForm } from './components/comment-form/comment-form';
import { CommentList } from './components/comment-list/comment-list';

export const APP_CONFIG = new InjectionToken<ApplicationConfig>('app.config');

export const appConfig: ApplicationConfig = {
  providers: [
    importProvidersFrom(BrowserModule),
    provideRouter(
      [
        { path: '', component: CommentList },
        { path: 'add', component: CommentForm }
      ],
      withEnabledBlockingInitialNavigation()
    )
  ]
};