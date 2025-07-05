import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { noScriptValidator } from '../../validators/no-script.validator';

export class BuildCommentForm {
  form: FormGroup;

  constructor(private fb: FormBuilder) {
    this.form = this.buildForm();
  }

  private buildForm(): FormGroup {
    return this.fb.group({
      userName: ['', [Validators.required, Validators.pattern('^[a-zA-Z0-9]+$'), Validators.maxLength(30)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(50)]],
      homePage: ['', Validators.pattern('https?://.+')],
      captcha: ['', [Validators.required, Validators.pattern('^[a-zA-Z0-9]+$')]],
      message: ['', [Validators.required, Validators.maxLength(1000), noScriptValidator]],
      parentCommentId: [null],
    });
  }
}