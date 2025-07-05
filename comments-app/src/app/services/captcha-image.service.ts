import { CaptchaService } from "./captcha.service";
import { Observable, switchMap, map } from "rxjs";
import { SafeUrl } from "@angular/platform-browser";
import { Injectable } from "@angular/core";
import { DomSanitizer } from "@angular/platform-browser";

@Injectable({ providedIn: 'root' })
export class CaptchaImageService {
  constructor(private captchaService: CaptchaService, private sanitizer: DomSanitizer) {}

  getSafeCaptcha(): Observable<SafeUrl> {
    return this.captchaService.getCaptcha().pipe(
      switchMap(blob => this.readAsDataURL(blob)),
      map(base64 => this.sanitizer.bypassSecurityTrustUrl(base64))
    );
  }

  private readAsDataURL(blob: Blob): Observable<string> {
    return new Observable(observer => {
      const reader = new FileReader();
      reader.onloadend = () => {
        observer.next(reader.result as string);
        observer.complete();
      };
      reader.readAsDataURL(blob);
    });
  }
}