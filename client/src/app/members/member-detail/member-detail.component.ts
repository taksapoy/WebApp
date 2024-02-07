import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TabDirective, TabsModule, TabsetComponent } from 'ngx-bootstrap/tabs';
import { TimeagoModule } from 'ngx-timeago';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { MemberMessagesComponent } from '../member-messages/member-messages.component';
import { Message } from 'src/app/_models/Message';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
  standalone: true,
  imports: [MemberMessagesComponent, CommonModule, TabsModule, GalleryModule, TimeagoModule]
})
export class MemberDetailComponent implements OnInit {
  member: Member = {} as Member
  photos: GalleryItem[] = [];

  @ViewChild('memberTabs') memberTabs?: TabsetComponent
  activeTab?: TabDirective
  messages: Message[] = []

  constructor(
    private messageService:MessageService ,private memberService:MembersService ,private route:ActivatedRoute) {}


    ngOnInit(): void { //
      // this.loadMember()//<--ไม่ใช้แล้ว เพราะ ได้ member จาก resolver
      this.route.data.subscribe({
          next: data => {
              this.member = data['member'] //เพราะเราตั้งชื่อ member ใน app-routing.module.ts
              this.getImages()
          }
      })
      this.route.queryParams.subscribe({
          next: params => params['tab'] && this.selectTab(params['tab'])
      })
  }


  getImages() {
    if (!this.member) return;
    for (const photo of this.member.photos) {
      this.photos.push(new ImageItem({ src: photo.url, thumb: photo.url }));
    }
  }


  // loadMember() {
  //   const username = this.route.snapshot.paramMap.get('/username');
  //   if (!username) return;
  //   this.memberService.getMember(username).subscribe({
  //     next: (user) => {
  //       this.member = user;
  //       this.getImages();
  //     },
  //   });
  // }


  loadMessages() { 
    if (!this.member) return
    this.messageService.getMessagesThread(this.member.userName).subscribe({
      next: response => this.messages = response
    })
  }


  onTabActivated(tab: TabDirective) { 
    this.activeTab = tab
    if (this.activeTab.heading === 'Messages') {
      this.loadMessages()
    }
  }

  
  selectTab(tabHeading: string) {
    if (!this.memberTabs) return
    const tab = this.memberTabs.tabs.find(tab => tab.heading === tabHeading)
    if (!tab) return
    tab.active = true
  }
}
