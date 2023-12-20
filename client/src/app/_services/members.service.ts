import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { Member } from '../_modules/member';

@Injectable({
  providedIn: 'root'
})

export class MembersService {
  baseUrl = environment.apiUrl
  constructor(private http : HttpClient) { }

  getMembers() {
    const endpoint = this.baseUrl + 'users'
    return this.http.get<Member[]>(this.baseUrl + 'users')
  }

  getMember(username: string) {
    const endpoint = this.baseUrl + 'users/username/'+ username
    return this.http.get<Member>(endpoint)
  }
}
