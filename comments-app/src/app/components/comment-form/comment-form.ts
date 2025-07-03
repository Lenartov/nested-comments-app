import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CommentService } from '../../services/comment.service';
import DOMPurify from 'dompurify';
import { noScriptValidator } from '../../validators/no-script.validator';

@Component({
  selector: 'app-comment-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './comment-form.html',
  styleUrls: ['./comment-form.css'],
})
export class CommentForm {
  form: FormGroup;
  selectedFile: File | null = null;

  constructor(private fb: FormBuilder, private commentService: CommentService) {
    this.form = this.fb.group({
      userName: ['', [Validators.required, Validators.pattern('^[a-zA-Z0-9]+$'), Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(100)]],
      homePage: ['', Validators.pattern('https?://.+')],
      captcha: ['', [Validators.required, Validators.pattern('^[a-zA-Z0-9]+$')]],
      message: ['', [Validators.required, Validators.maxLength(1000), noScriptValidator]],
      parentCommentId: [null],
    });
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length) {
      this.selectedFile = input.files[0];
    }
  }

  onSubmit() {
    if (this.form.valid) {
      const formData = new FormData();

      formData.append('UserName', this.form.value.userName);
      formData.append('Email', this.form.value.email);

      if (this.form.value.homePage) {
        formData.append('HomePage', this.form.value.homePage);
      }

      formData.append('Captcha', this.form.value.captcha);
      
      const cleanMessage = DOMPurify.sanitize(this.form.value.message, {
      ALLOWED_TAGS: ['a', 'code', 'i', 'strong'],
      ALLOWED_ATTR: ['href', 'title'],});
      formData.append('Message', cleanMessage);

      if (this.form.value.parentCommentId != null) {
        formData.append('ParentCommentId', this.form.value.parentCommentId.toString());
      }

      if (this.selectedFile) {
        formData.append('File', this.selectedFile, this.selectedFile.name);
      }

      this.commentService.postComment(formData).subscribe({
        next: (res) => {
          console.log('Коментар додано:', res);
          this.form.reset();
          this.selectedFile = null;
          // Тут можна додати emit події або логіку оновлення списку
        },
        error: (err) => {
          console.error('Помилка при додаванні коментаря:', err);
        },
      });
    }
  }
}
