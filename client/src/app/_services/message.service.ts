import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { getPaginationHeaders, getPaginationResult } from './paginationHelper';
import { Message } from '../_models/Message';
import { HttpClient } from '@angular/common/http';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, take } from 'rxjs';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})

export class MessageService {
  baseUrl = environment.apiUrl
  hubUrl = environment.hubUrl
  private _hubConnection?: HubConnection
  private _messageThread = new BehaviorSubject<Message[]>([])
  messageThread$ = this._messageThread.asObservable();

  constructor(private http: HttpClient) { }


  createHubConnection(user: User, otherUsername: string) {
    const url = this.hubUrl + 'message?user=' + otherUsername
      this._hubConnection = new HubConnectionBuilder()
        .withUrl(url, { accessTokenFactory: () => user.token })
        .withAutomaticReconnect()
        .build()
      this._hubConnection.start().catch(error => console.log(error))

      this._hubConnection.on('MessageThread', message => {
      this._messageThread.next(message)
    })
      this._hubConnection.on('NewMessage', newMessage => {
      this.messageThread$.pipe(take(1)).subscribe({
        next: messages => {
        this._messageThread.next([...messages, newMessage])
        }
      })
    })
  }

  
  stopHubConnection() {
    if(this._hubConnection)
      this._hubConnection?.stop().catch(error => console.log(error))
  }

  
  deleteMessage(id: number) {
    const url = this.baseUrl + 'messages/' + id
    return this.http.delete(url)
  }
  

  async sendMessage(recipientUsername: string, content: string) {
    // const url = this.baseUrl + 'messages'
    // const body = { recipientUsername, content }
    // return this.http.post<Message>(url, body)
    return this._hubConnection?.invoke('SendMessage', { //invoke เรียกใช้ method 'SendMessage' ใน MessageHub.cs
      recipientUsername, content
    }).catch(error => console.log(error))
  }


  getMessagesThread(username: string) {
    const url = this.baseUrl + 'messages/thread/' + username
    return this.http.get<Message[]>(url)
  }


  getMessages(pageNumber: number, pageSize: number, label: string = "Unread") {
    let httpParams = getPaginationHeaders(pageNumber, pageSize)
    httpParams = httpParams.append('Label', label)

    const url = this.baseUrl + 'messages'
    
    return getPaginationResult<Message[]>(url, httpParams, this.http)
  }
}
