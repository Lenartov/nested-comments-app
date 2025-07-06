import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CaptchaConfigService } from './captcha-config.service';

export interface CaptchaResponse {
  captchaId: string;
  imageUrl: string; 
}

@Injectable({
  providedIn: 'root'
})
export class CaptchaService {
  constructor(private http: HttpClient, private config: CaptchaConfigService) {}

  getCaptcha(): Observable<Blob> {
    return this.http.get(this.config.apiUrl, {
      responseType: 'blob', 
      withCredentials: true
    });
  }
}
