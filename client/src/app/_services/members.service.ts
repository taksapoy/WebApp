import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { map, of } from 'rxjs';
import { PaginationResult } from '../_models/Pagination';
import { UserParams } from '../_models/userParams';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  // paginationResult: PaginationResult<Member[]> = new PaginationResult<Member[]> 
  baseUrl = environment.apiUrl;
  members: Member[] = [];

  constructor(private http: HttpClient) {}

  getMembers (userParams: UserParams) {
    let params = this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize)
    params = params.append('minAge', userParams.minAge)
    params = params.append('maxAge', userParams.maxAge)
    params = params.append('gender', userParams.gender)
    const url = this.baseUrl + '/users'
    return this.getPaginationResult<Member[]>(url, params)
    }

    private getPaginationResult<T>(url: string, params: HttpParams) {
      const paginationResult: PaginationResult<T> = new PaginationResult<T>
      return this.http.get<T>(url, { observe: 'response', params }).pipe(
        map(response => {
          if (response.body)
            paginationResult.result = response.body
  
          const pagination = response.headers.get('Pagination')
          if (pagination)
            paginationResult.pagination = JSON.parse(pagination)
  
          return paginationResult
        })
      )
    }
    private getPaginationHeaders(pageNumber: number, pageSize: number) {
      let params = new HttpParams()
      params = params.append('pageNumber', pageNumber)
      params = params.append('pageSize', pageSize)
      return params
    }  

  getMember(username: string) {
    const member = this.members.find((user) => user.userName === username);
    if (member) return of(member);
    return this.http.get<Member>(this.baseUrl + '/users/username/' + username);
  }

  updateProfile(member: Member) {
    return this.http.put(this.baseUrl + '/users', member).pipe(
      map((_) => {
        const index = this.members.indexOf(member);
        this.members[index] = { ...this.members[index], ...member };
      })
    );
  }
  setMainPhoto(photoId: number) {
    const endpoint = this.baseUrl + '/users/set-main-photo/' + photoId
    return this.http.put(endpoint, {})
  }

  deletePhoto(photoId: number) {
    const endpoint = this.baseUrl + '/users/delete-photo/' + photoId
    return this.http.delete(endpoint)
  }
}
