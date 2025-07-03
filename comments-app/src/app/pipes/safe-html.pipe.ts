import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import DOMPurify from 'dompurify';

@Pipe({
  name: 'safeHtml',
  standalone: true
})
export class SafeHtmlPipe implements PipeTransform {
  constructor(private sanitizer: DomSanitizer) {}

  transform(value: string): SafeHtml {
    // Дозволені теги
    const clean = DOMPurify.sanitize(value, {
      ALLOWED_TAGS: ['a', 'code', 'i', 'strong'],
      ALLOWED_ATTR: ['href', 'title']
    });
    return this.sanitizer.bypassSecurityTrustHtml(clean);
  }
}
