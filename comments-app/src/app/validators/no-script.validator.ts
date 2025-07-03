import { AbstractControl, ValidationErrors } from '@angular/forms';

export function noScriptValidator(control: AbstractControl): ValidationErrors | null {
  const value = control.value;
  if (typeof value === 'string' && /<script[\s\S]*?>[\s\S]*?<\/script>/gi.test(value)) {
    return { scriptTag: true };
  }
  return null;
}