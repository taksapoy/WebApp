import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { User } from 'src/_models/user';

@Injectable({
    providedIn: 'root'
})
export class AccountService {
    baseUrl = 'https://localhost:7777/api/'

    constructor(private http: HttpClient) { }

    private currentUserSource = new BehaviorSubject<User | null>(null)
    currentUser$ = this.currentUserSource.asObservable()//the $ is convention to signify that this is observable

    login(model: any) {
        return this.http.post<User>(`${this.baseUrl}account/login`, model).pipe(
            map((user: User) => {
                if (user) {
                    localStorage.setItem('user', JSON.stringify(user))
                    this.currentUserSource.next(user)
                }
            })
        )
    }
    register(model: any) {
        return this.http.post<User>(`${this.baseUrl}account/register`, model).pipe(
            map(user => {
                if (user) {
                    localStorage.setItem('user', JSON.stringify(user))
                    this.currentUserSource.next(user)
                }
            })
        )
    }
    logout() {
        localStorage.removeItem('user')
        this.currentUserSource.next(null)
    }
    setCurrentUser(user: User) {
        this.currentUserSource.next(user)
    }

}