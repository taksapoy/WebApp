import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.css']
})
export class ServerErrorComponent {
  err : any
  constructor(router : Router){
    this.err = router.getCurrentNavigation()?.extras.state ?. ['errors']
  }
}
