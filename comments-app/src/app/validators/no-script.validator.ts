import { AbstractControl, ValidationErrors } from '@angular/forms';

export function htmlTagValidator(control: AbstractControl): ValidationErrors | null {
  const value = control.value;
  if (typeof value !== 'string') return null;

  const allowedTags = ['a', 'code', 'i', 'strong'];

  const tagRegex = /<\/?([a-z]+)(\s+[^>]*)?>/gi;
  const stack: string[] = [];

  let match: RegExpExecArray | null;
  while ((match = tagRegex.exec(value)) !== null) {
    const [, tagNameRaw] = match;
    const tagName = tagNameRaw.toLowerCase();

    const isClosing = match[0].startsWith('</');

    if (!allowedTags.includes(tagName)) {
      return { disallowedTag: tagName };
    }

    if (isClosing) {
      if (stack.length === 0 || stack[stack.length - 1] !== tagName) {
        return { unclosedTag: tagName };
      }
      stack.pop();
    } else {
      stack.push(tagName);
    }
  }

  if (stack.length > 0) {
    return { unclosedTag: stack[stack.length - 1] };
  }

  return null;
}
