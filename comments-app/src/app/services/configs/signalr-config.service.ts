import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalRConfigService {
  readonly HUB_METHOD_NAME = 'CommentsAddedBatch';

  get apiUrl(): string {
    console.log(`${environment.baseUrl}/commentHub`)
    return `${environment.baseUrl}/commentHub`;
  }
}