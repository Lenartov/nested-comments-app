import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { SignalRConfigService } from './configs/signalr-config.service'

@Injectable({
  providedIn: 'root'
})

export class SignalRService {
  private hubConnection!: signalR.HubConnection;

  constructor(private config: SignalRConfigService) {}

  private updateSubject = new Subject<(number | null)[]>();
  public update$ = this.updateSubject.asObservable();

  public startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.config.apiUrl)
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start()
      .then(() => console.log('SignalR Connected'))
      .catch(err => console.error('SignalR Connection Error:', err));

    this.hubConnection.on(this.config.HUB_METHOD_NAME, (parentIds: (number | null)[]) => {
      console.log('Received CommentsUpdatedBatch:', parentIds);
      this.updateSubject.next(parentIds);
    });
  }

  public stopConnection(): void {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }
}
