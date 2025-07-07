export interface CaptchaResponse {
  captchaToken: string;
  captchaImageBase64: string;
}

export interface ValidateCaptchaRequest {
  userInput: string;
  captchaToken: string;
}
