import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ButtonsModule } from 'ngx-bootstrap/buttons'
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FileUploadModule } from 'ng2-file-upload';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TimeagoModule } from "ngx-timeago";

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(),
    TimeagoModule.forRoot(),
    ToastrModule.forRoot(),
    TabsModule.forRoot(),
    ButtonsModule.forRoot(),
    NgxSpinnerModule.forRoot({ type: 'square-spin' }),
    FileUploadModule,
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
  ],
  
  exports: [
    BsDropdownModule,
    TabsModule,
    TimeagoModule,
    ToastrModule,
    NgxSpinnerModule,
    FileUploadModule,
    FileUploadModule,
    BsDatepickerModule,
    PaginationModule,
    ButtonsModule,
  ],
})
export class SharedModule {}
