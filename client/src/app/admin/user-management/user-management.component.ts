import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_services/admin.service';
import { RolesModalComponent } from 'src/app/modals/roles-modal/roles-modal.component';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
  bsModalRef: BsModalRef<RolesModalComponent> = new BsModalRef<RolesModalComponent>()
  users: User[] = []
  constructor(private adminService: AdminService ,private bsModalService: BsModalService) { }
  availableRoles: any[] = ['Administrator', 'Moderator', 'Member']

  
  openRolesModal(user: User) {
    const modalOptions: ModalOptions = {
      class: 'modal-dialog-centered',
      initialState: {
        user,
        availableRoles: this.availableRoles,
        selectedRoles: [...user.roles]
      },
    }
    this.bsModalRef = this.bsModalService.show(RolesModalComponent, modalOptions)
    this.bsModalRef.onHide?.subscribe({
      next: () => {
        const isConfirmUpdate = this.bsModalRef.content?.isConfirmUpdate
        const selectedRoles = this.bsModalRef.content?.selectedRoles.join(',')
        if (isConfirmUpdate && selectedRoles && selectedRoles !== "")
          this.adminService.updateUserRoles(user.username, selectedRoles).subscribe({
            next: response => user.roles = response
          })
      }
    })
  }


  ngOnInit(): void {
    this.getUserWithRoles()
  }


  getUserWithRoles() {
    this.adminService.getUsersWithRoles().subscribe({
      next: response => this.users = response
    })
  }
}
