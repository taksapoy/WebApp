import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { getPaginationHeaders, getPaginationResult } from './paginationHelper';
import { Message } from '../_models/Message';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class MessageService {
  baseUrl = environment.apiUrl
  constructor(private http: HttpClient) { }

  
  deleteMessage(id: number) {
    const url = this.baseUrl + '/messages/' + id
    return this.http.delete(url)
  }
  

  sendMessage(recipientUsername: string, content: string) {
    const url = this.baseUrl + '/messages'
    const body = { recipientUsername, content } //ต้องสะกดตรงกับ CreateMessageDto.cs
    return this.http.post<Message>(url, body)
  }


  getMessagesThread(username: string) {
    const url = this.baseUrl + '/messages/thread/' + username
    return this.http.get<Message[]>(url)
  }


  getMessages(pageNumber: number, pageSize: number, label: string = "Unread") {
    let httpParams = getPaginationHeaders(pageNumber, pageSize)
    httpParams = httpParams.append('Label', label)
    const url = this.baseUrl + '/messages'
    return getPaginationResult<Message[]>(url, httpParams, this.http)
  }
}
