import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { User } from 'src/_models/user';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-nav',
    templateUrl: './nav.component.html',
    styleUrls: ['./nav.component.css']
})
export class NavComponent {
    model: any = {}

    constructor(private toastr: ToastrService, private router: Router, public accountService: AccountService){ }

    login(): void {
        this.accountService.login(this.model).subscribe({
            next: response => {
                this.router.navigateByUrl('/members')
            },
            error: err => this.toastr.error(err.error)
        })
    }
    logout() {
        this.accountService.logout()
        this.router.navigateByUrl('/')
    }
}
