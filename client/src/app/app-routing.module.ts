import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { HomeComponent } from './home/home.component'
import { MemberListComponent } from './members/member-list/member-list.component'
import { MemberDetailComponent } from './members/member-detail/member-detail.component'
import { ListsComponent } from './lists/lists.component'
import { MessagesComponent } from './messages/messages.component'
import { authGuard } from './_guard/auth.guard'
import { TestErrorComponent } from './errors/test-error/test-error.component'
import { NotFoundComponent } from './errors/not-found/not-found.component'

const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '', runGuardsAndResolvers: 'always', canActivate: [authGuard], children: [
      { path: 'members', component: MemberListComponent },
      { path: 'members/:id', component: MemberDetailComponent },
      { path: 'members/name/:username', component: MemberDetailComponent },
      { path: 'lists', component: ListsComponent },
      { path: 'messages', component: MessagesComponent },
    ]
  },
  { path: 'errors', component: TestErrorComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'server-error', component: TestErrorComponent },
  { path: '**', component: NotFoundComponent, pathMatch: 'full' }, // ** = anything not in list
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

