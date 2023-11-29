import { Component } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent {
  
    model: { username: string | undefined, password: string | undefined } = {
        username: undefined,
        password: undefined
    }
    isLogin = false

    constructor (private accountService : AccountService) {

    }
    login(): void {
      this.accountService.login(this.model).subscribe({ //Observable
          next: response => {
              console.log(response)
              this.isLogin = true
          },
          error: err => console.log(err) //anything that's not in 200 range of HTTP status
      })
  }

}
