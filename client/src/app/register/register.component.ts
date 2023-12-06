import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core'
import { AccountService } from '../_services/account.service'
import { Observable, of } from 'rxjs'
import { User } from '../_models/user'
import { Router } from '@angular/router'
import { Toast, ToastrService } from 'ngx-toastr'

@Component({
  selector: 'app-nav',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class NavComponent implements OnInit{
register() {
throw new Error('Method not implemented.')
}
  @Input() usersFromHomeCpmponent: any
  model: any = {}

  currentUser$: Observable<User | null> = of(null) // isLogin = false

  constructor(private toastr: ToastrService, private router: Router, private accountService: AccountService) {}
  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$
  }

  getCurrentUser() {
    this.accountService.currentUser$.subscribe({
        next: user => console.log(user),
        error: err => this.toastr.error(err)
    })
}

  @Output() isCancel = new EventEmitter()
  cancel() {
    this.isCancel.emit(true)
  }
}
