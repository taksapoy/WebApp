import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { User } from '../_models/user';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  model: any = {}
  user: User | null = null

  currentUser$: Observable<User | null> = of(null)

  constructor(private toastr: ToastrService, private router: Router, public accountService: AccountService) { }

  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$
    this.currentUser$.subscribe({
      next: user => {this.user = user}
    })
  }

  getCurrentUser() {
    this.accountService.currentUser$.subscribe({
      next: user => console.log(user),
      error: err => this.toastr.error(err)
    })
  }

  login(): void {
    this.accountService.login(this.model).subscribe({
      next: response => { this.router.navigateByUrl('/member')},
      error: err => this.toastr.error (err.error)
    })
  }


  logout() {
    this.accountService.logout()
    this.router.navigateByUrl('/')
  }
}