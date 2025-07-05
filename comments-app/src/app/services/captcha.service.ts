import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface CaptchaResponse {
  captchaId: string;
  imageUrl: string; 
}

@Injectable({
  providedIn: 'root'
})
export class CaptchaService {
  private readonly apiUrl = 'http://localhost:5237/api/Captcha';

  constructor(private http: HttpClient) {}

  getCaptcha(): Observable<Blob> {
    return this.http.get(this.apiUrl, {
      responseType: 'blob', 
      withCredentials: true
    });
  }
}
