import { Component, EventEmitter, Input, Output } from '@angular/core'
import { AccountService } from '../_services/account.service'
import { Router } from '@angular/router'
import { ToastrService } from 'ngx-toastr'

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  //@Input() usersFromHomeComponent: any
  @Output() isCancel = new EventEmitter()


  constructor(private toastr: ToastrService, private router: Router, private accountService: AccountService) { }

  model: any = {}

  register() {
    //console.log(this.model)
    this.accountService.register(this.model).subscribe(
      {
        error: err => this.toastr.error(err),
        next: () => this.router.navigateByUrl('/members')

      }
    )
  }

  cancel() {
    console.log('cancel')
    this.isCancel.emit(true)
  }

}
