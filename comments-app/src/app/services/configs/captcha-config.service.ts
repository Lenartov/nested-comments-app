import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CaptchaConfigService {
  get apiUrl(): string {
    return `${environment.apiBaseUrl}/JwtCaptcha`;
  }
}