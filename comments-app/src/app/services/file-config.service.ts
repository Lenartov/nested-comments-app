import { environment } from "../../environments/environment";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root',
})
export class FileConfigService {
  get baseUrl(): string {
    return environment.fileBaseUrl;
  }

  get imageExtensions(): string[] {
    return ['jpg', 'jpeg', 'png', 'gif'];
  }
}
