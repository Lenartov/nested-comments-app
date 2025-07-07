import { CaptchaResponse } from "../models/captcha.model";
import { CaptchaService } from "./captcha.service";
import { Observable, map } from "rxjs";
import { SafeUrl } from "@angular/platform-browser";
import { Injectable } from "@angular/core";
import { DomSanitizer } from "@angular/platform-browser";

@Injectable({ providedIn: 'root' })
export class CaptchaImageService {
  constructor(private captchaService: CaptchaService, private sanitizer: DomSanitizer) {}

  getCaptchaImage(): Observable<{ captchaImageBase64: SafeUrl; captchaToken: string }> {
    return this.captchaService.getCaptcha().pipe(
      map((captcha: CaptchaResponse) => ({
        captchaImageBase64: this.sanitizer.bypassSecurityTrustUrl(`data:image/png;base64,${captcha.captchaImageBase64}`),
        captchaToken: captcha.captchaToken
      }))
    );
  }
}
