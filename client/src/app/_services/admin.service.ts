import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';


@Injectable({
  providedIn: 'root'
})


export class AdminService {
  baseUrl = environment.apiUrl
  constructor(private http: HttpClient) { }


  getUsersWithRoles() {
    const url = this.baseUrl + 'admin/users-with-roles'
    return this.http.get<User[]>(url)
  }

  updateUserRoles(username: string, roles: string) {
    const queryString = '?roles=' + roles
    const url = this.baseUrl + 'admin/edit-roles/' + username + queryString
    return this.http.post<string[]>(url, {})
  }
}
