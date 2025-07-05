import DOMPurify from 'dompurify';

export function sanitizeComment(input: string): string {
  return DOMPurify.sanitize(input, {
    ALLOWED_TAGS: ['a', 'code', 'i', 'strong'],
    ALLOWED_ATTR: ['href', 'title'],
  });
}