import { FormBuilder, ReactiveFormsModule, FormGroup } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CommentService } from '../../services/comment.service';
import { SafeUrl } from '@angular/platform-browser';
import { CommentSelectionService } from '../../services/comment-selection.service';
import { Subscription, Observable } from 'rxjs';
import { Component, OnInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { CaptchaImageService } from '../../services/captcha-image.service';
import { BuildCommentForm } from './comment-form.builder';
import { sanitizeComment } from '../../Utils/sanitize-comment';
import { tap } from 'rxjs/operators';
import { CaptchaService } from '../../services/captcha.service';
import { shareReplay } from 'rxjs/operators';
import DOMPurify from 'dompurify';

@Component({
  selector: 'app-comment-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './comment-form.component.html',
  styleUrls: ['./comment-form.component.css'],
})
export class CommentFormComponent implements OnInit, OnDestroy {
  @ViewChild('messageInput') messageInput!: ElementRef<HTMLTextAreaElement>;

  form!: FormGroup;

  captchaData$!: Observable<{ captchaImageBase64: SafeUrl; captchaToken: string }>;
  captchaToken: string = '';

  subscriptions = new Subscription();
  parentId: number | null = null;
  selectedFile: File | null = null;

  message: string | null = null;
  isError: boolean = false;

  showPreview = false;
  previewHtml = '';

  constructor(
    private fb: FormBuilder,
    private commentService: CommentService,
    private commentSelectionService: CommentSelectionService,
    private captchaImageService: CaptchaImageService,
    private captchaService: CaptchaService
  ) {
    this.form = new BuildCommentForm(fb).form;
  }

  ngOnInit(): void {
    this.reloadCaptcha();
    this.subscriptions.add(this.commentSelectionService.parentId$.subscribe((id) => (this.parentId = id)));
     this.form.get('message')?.valueChanges.subscribe((value) => 
      {this.previewHtml = DOMPurify.sanitize(value, {
        ALLOWED_TAGS: ['a', 'code', 'i', 'strong'],
        ALLOWED_ATTR: ['href', 'title']
      });
  });
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  reloadCaptcha(): void {
  this.captchaData$ = this.captchaImageService.getCaptchaImage().pipe(
    tap(data => {
      this.captchaToken = data.captchaToken;
    }),
    shareReplay(1)
  );
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

  onSubmit(): void {
  if (!this.form.valid) return;

  const userInput = this.form.value.captcha;
  const captchaToken = this.captchaToken;

  this.captchaService.validateCaptcha({ userInput, captchaToken }).subscribe({
    next: () => this.submitComment(),
    error: () => this.handleError('Incorrect Captcha')
  });
}

private submitComment(): void {
  const formData = new FormData();

  if (this.parentId !== null) {
    formData.append('ParentCommentId', this.parentId.toString());
  }

  formData.append('UserName', this.form.value.userName);
  formData.append('Email', this.form.value.email);

  if (this.form.value.homePage) {
    formData.append('HomePage', this.form.value.homePage);
  }

  const cleanMessage = sanitizeComment(this.form.value.message);
  formData.append('Message', cleanMessage);

  if (this.selectedFile) {
    formData.append('File', this.selectedFile, this.selectedFile.name);
  }

  this.commentService.postComment(formData).subscribe({
    next: (res) => this.handleSuccess('Success!'),
    error: (err) => this.handleError('Error: ' + (err.error?.error || err.message))
  });
}

private handleSuccess(message: string): void {
  console.log('Comment added');
  this.message = message;
  this.isError = false;
  this.resetFormAndCaptcha();
}

private handleError(message: string): void {
  console.error(message);
  this.message = message;
  this.isError = true;
  this.resetFormAndCaptcha();
}

private resetFormAndCaptcha(): void {
  this.form.reset();
  this.selectedFile = null;
  this.reloadCaptcha();
}

togglePreview() {
  this.showPreview = !this.showPreview;
}

wrapText(tag: string, attributes: string = '') {
    const textarea = this.messageInput.nativeElement;
    const value = textarea.value;
    const start = textarea.selectionStart;
    const end = textarea.selectionEnd;

    const selectedText = value.slice(start, end);
    const tagOpen = `<${tag}${attributes}>`;
    const tagClose = `</${tag}>`;

    let newText: string;

    if (start !== end) {
      newText = value.slice(0, start) + tagOpen + selectedText + tagClose + value.slice(end);
    } else {
      newText = value.slice(0, start) + tagOpen + tagClose + value.slice(end);
    }

    this.form.get('message')!.setValue(newText);

    const cursorPosition = start + tagOpen.length + (selectedText.length ? selectedText.length + tagClose.length : 0);
    setTimeout(() => textarea.setSelectionRange(cursorPosition, cursorPosition));
  }
}
