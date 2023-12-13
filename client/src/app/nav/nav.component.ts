import { Component, OnInit } from "@angular/core"
import { User } from "../_models/user"
import { ToastrService } from "ngx-toastr"
import { Router } from "@angular/router"
import { AccountService } from "../_services/account.service"
import { Observable, of } from "rxjs"


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}
  user: User | null = null
  currentUser$: Observable<User | null> = of(null)  // isLogin = false




  constructor(private toastr: ToastrService, private router: Router, private accountService: AccountService) { }

  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$
    this.currentUser$.subscribe({
      next: user => this.user = user
    })

  }

  getCurrentUser() {
    this.accountService.currentUser$.subscribe({
      next: user => console.log(user),
      error: err => console.log(err)
    })
  }


  login(): void {
    this.accountService.login(this.model).subscribe({ //Observable
      next: () => {
        this.router.navigateByUrl("/members")
        //console.log(response)

      },
      error: err => this.toastr.error(err.error) //anything that's not in 200 range of HTTP status
    })
  }
  logout() {
    this.accountService.logout()

  }

}
