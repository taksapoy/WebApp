import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { map, of, take } from 'rxjs';
import { PaginationResult } from '../_models/Pagination';
import { UserParams } from '../_models/userParams';
import { User } from '../_models/user';
import { ListParams } from '../_models/listParams';
import { getPaginationHeaders, getPaginationResult } from './paginationHelper';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  // userParams: UserParams | undefined
  user: User | undefined
  memberCache = new Map( )
  baseUrl = environment.apiUrl;
  members: Member[] = [];

  constructor(private http: HttpClient) { }

  private _key(userParams: UserParams) 
  { return Object.values(userParams).join('_'); }


  getMembers (userParams: UserParams) {
    const key = this._key(userParams)
    const response = this.memberCache.get(key) 
        if (response) return of(response)

    // let params = getPaginationHeaders(userParams.pageNumber, userParams.pageSize)
    let params = getPaginationHeaders(userParams.pageNumber, userParams.pageSize)
    params = params.append('minAge', userParams.minAge)
    params = params.append('maxAge', userParams.maxAge)
    params = params.append('gender', userParams.gender)
    params = params.append('orderBy', userParams.orderBy)
    const url = this.baseUrl + 'users'
    return getPaginationResult<Member[]>(url, params, this.http)
    }

    private getPaginationResult<T>(url: string, params: HttpParams, key:string | null=null) {
      const paginationResult: PaginationResult<T> = new PaginationResult<T>
      return this.http.get<T>(url, { observe: 'response', params }).pipe(
        map(response => {
          if (response.body)
            paginationResult.result = response.body
  
          const pagination = response.headers.get('Pagination')
          if (pagination)
            paginationResult.pagination = JSON.parse(pagination)

            if (key)
            this.memberCache.set(key, paginationResult)
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
    const cache = [...this.memberCache.values()]
    const members = cache.reduce((arr, item) => arr.concat(item.result), [])
    const member = members.find((member: Member) => member.userName === username)
    if (member) return of(member)

    return this.http.get<Member>(this.baseUrl + 'users/username/' + username);
  }


  updateProfile(member: Member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map((_) => {
        const index = this.members.indexOf(member);
        this.members[index] = { ...this.members[index], ...member };
      })
    );
  }


  setMainPhoto(photoId: number) {
    const endpoint = this.baseUrl + 'users/set-main-photo/' + photoId
    return this.http.put(endpoint, {})
  }


  deletePhoto(photoId: number) {
    const endpoint = this.baseUrl + 'users/delete-photo/' + photoId
    return this.http.delete(endpoint)
  }


  addLike(username: string) {
    return this.http.post(this.baseUrl + 'likes/' + username, {})
  }

  
  getLikes(listParams: ListParams) {
    let httpParams = getPaginationHeaders(listParams.pageNumber, listParams.pageSize)
    const url = this.baseUrl + 'likes'
    return getPaginationResult<Member[]>(url, httpParams, this.http)
  }
  
}
