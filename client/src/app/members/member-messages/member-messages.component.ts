import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input, OnInit, ViewChild } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome'
import { TimeagoModule } from 'ngx-timeago';
import { Message } from 'src/app/_models/Message';
import { MessageService } from 'src/app/_services/message.service';
import { faClock,faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { FormsModule, NgForm } from '@angular/forms';
import { NgxLongPress2Module } from 'ngx-long-press2';

@Component({
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, FontAwesomeModule, TimeagoModule,FormsModule , NgxLongPress2Module],
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @Input() username?: string
  // @Input() messages: Message[] = []
  @ViewChild('messageForm') messageForm?: NgForm
  messageContent = ''

  faClock = faClock
  faPaperPlane = faPaperPlane

  constructor(public messageService: MessageService) { }


  onLongPressMessage(id: number) {
    // console.log('delete me, id: ' + id)
    this.messageService.deleteMessage(id).subscribe({
      // next: _ => this.messages?.splice(this.messages.findIndex(ms => ms.id === id), 1)
    })
  }


  sendMessage() {
    if (!this.username) return
    this.messageService.sendMessage(this.username, this.messageContent) //เราแก้ไขไปเมื่อกี้ทำให้ ได้ promise กลับมา
    .then(() => {
      this.messageForm?.reset()
    })
  }


  ngOnInit(): void {
    // this.loadMessages()
  }
  
  
  loadMessages() {
    if (!this.username) return
    this.messageService.getMessagesThread(this.username).subscribe({
      // next: response => this.messages = response
    })
  }
}
