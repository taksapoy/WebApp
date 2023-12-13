import { HttpClient } from '@angular/common/http'
import { Component, OnInit } from '@angular/core'
import { faBell } from '@fortawesome/free-solid-svg-icons'
import { AccountService } from './_services/account.service'
import { User } from './_models/user'


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Hello';
  faBell = faBell
  users: any

  constructor(private accountService: AccountService, private http: HttpClient) { }

  setCurrentUser() {
    const userSrting = localStorage.getItem('user')
    if (userSrting == null) return
    const user: User = JSON.parse(userSrting)
    this.accountService.setCurrentUser(user)
  }

  ngOnInit(): void {
    //this.getUser()
    this.setCurrentUser()
  }


}