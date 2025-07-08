import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CaptchaConfigService } from './configs/captcha-config.service';
import { CaptchaResponse, ValidateCaptchaRequest } from '../models/captcha.model';

@Injectable({
  providedIn: 'root'
})
export class CaptchaService {
  constructor(private http: HttpClient, private config: CaptchaConfigService) {}

  getCaptcha(): Observable<CaptchaResponse> {
    return this.http.get<CaptchaResponse>(this.config.apiUrl + '/generate');
  }

  validateCaptcha(request: ValidateCaptchaRequest): Observable<void> {
    return this.http.post<void>(this.config.apiUrl + '/validate', request);
  }
}
