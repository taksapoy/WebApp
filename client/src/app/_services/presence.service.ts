import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { BehaviorSubject, take } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  private _onlineUsers = new BehaviorSubject<string[]>([]); 
  onlineUser$ = this._onlineUsers.asObservable(); 

  hubUrl = environment.hubUrl
  private _hubConnection?: HubConnection

  constructor(private router: Router,private toastr: ToastrService) { }

  createHubConnection(user: User) {
    const url = this.hubUrl + 'presence'
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(url, { accessTokenFactory: () => user.token })
      .withAutomaticReconnect()
      .build()

    this._hubConnection.start().catch(error => console.log(error))
    this._hubConnection.on('UserOnline', username => {
      // this.toastr.info(username + ' has connected', '', { positionClass: 'toast-bottom-right' })
      this.onlineUser$.pipe(take(1)).subscribe({
        next: onlineusers => this._onlineUsers.next([...onlineusers, username])
      })
    })

    this._hubConnection.on('UserOffline', username => {
      // this.toastr.warning(username + ' has disconnected', '', { positionClass: 'toast-bottom-right' })
      this.onlineUser$.pipe(take(1)).subscribe({
        next: onlineusers => this._onlineUsers.next(onlineusers.filter(item => item !== username))
      })
    })
      this._hubConnection.on('OnlineUsers', users => { 
        this._onlineUsers.next(users) 
    })

    this._hubConnection.on('NewMessageReceived', ({ username, aka }) => {
      this.toastr.info(aka + ' has sent you a new message! click', '', { positionClass: 'toast-bottom-right' })
        .onTap.pipe(take(1)).subscribe({
          next: _ => this.router.navigateByUrl('/members/name/' + username + '?tab=Messages')
        })
    })
  }

  
  stopHubConnection() {
    this._hubConnection?.stop().catch(error => console.log(error))
  }
}
