import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CommentService } from '../../services/comment.service';
import { SafeUrl } from '@angular/platform-browser';
import { CommentSelectionService } from '../../services/comment-selection.service';
import { Subscription } from 'rxjs';
import { Component } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CaptchaImageService } from '../../services/captcha-image.service';
import { BuildCommentForm } from './comment-form.builder';
import { Observable } from "rxjs";
import { sanitizeComment } from '../../Utils/sanitize-comment';

@Component({
  selector: 'app-comment-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './comment-form.component.html',
  styleUrls: ['./comment-form.component.css'],
})
export class CommentFormComponent {
  form!: FormGroup;
  captchaImage$!: Observable<SafeUrl>;
  subscriptions = new Subscription();
  parentId: number | null = null;
  selectedFile: File | null = null;

  message: string | null = null;  // текст повідомлення
  isError: boolean = false;       // чи помилка це чи успіх

  constructor(private fb: FormBuilder,
    private commentService: CommentService,
    private commentSelectionService: CommentSelectionService,
    private captchaImageService: CaptchaImageService) 
    {
      this.form = new BuildCommentForm(fb).form;
    }

  ngOnInit(): void {
    this.reloadCaptcha();
    this.subscriptions.add(this.commentSelectionService.parentId$.subscribe(id => this.parentId = id));
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  reloadCaptcha(): void {
  this.captchaImage$ = this.captchaImageService.getSafeCaptcha();
  }

  clearParentId() {
    this.commentSelectionService.clearParentId();
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length) {
      this.selectedFile = input.files[0];
    }
  }

  onSubmit() {
    if (!this.form.valid) return;

    const formData = new FormData();

    if (this.parentId !== null) {
      formData.append('ParentCommentId', this.parentId.toString());
    }

    formData.append('UserName', this.form.value.userName);
    formData.append('Email', this.form.value.email);

    if (this.form.value.homePage) {
      formData.append('HomePage', this.form.value.homePage);
    }

    formData.append('Captcha', this.form.value.captcha);
      
    const cleanMessage = sanitizeComment(this.form.value.message);
    formData.append('Message', cleanMessage);

    if (this.selectedFile) {
      formData.append('File', this.selectedFile, this.selectedFile.name);
    }

    this.commentService.postComment(formData).subscribe({
      next: (res) => {
        console.log('Comment added', res);
        this.form.reset();
        this.selectedFile = null;
        this.reloadCaptcha();

        this.message = 'Success!';
        this.isError = false;
      },
      error: (err) => {
        console.error('Comment post failed:', err);
        this.form.reset();
        this.selectedFile = null;
        this.reloadCaptcha();

        this.message = 'Error: ' + err.error.error;
        this.isError = true;
      }});
  }
}
