import { HttpClient } from '@angular/common/http'
import { Component, OnInit } from '@angular/core'
import { faBell } from '@fortawesome/free-solid-svg-icons'


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Hello';
  faBell = faBell
  users: any

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.http.get('https://localhost:7777/api/users').subscribe({
      next: (response) => this.users = response,
      error: (err) => console.log(err),
      complete: () => console.log('request completed')
    })
  }
}