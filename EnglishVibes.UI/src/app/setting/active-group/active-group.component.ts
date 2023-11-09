import { Component, OnInit } from '@angular/core';
import { AdminService } from '../Services/admin.service';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-active-group',
  templateUrl: './active-group.component.html',
  styleUrls: ['./active-group.component.css']
})
export class ActiveGroupComponent implements OnInit {
  allActiveGroup: any;
  constructor(private _AdminService: AdminService, private datePipe: DatePipe) { }
  ngOnInit(): void {
    this._AdminService.getActiveGroup().subscribe({
      next: (response) => {
        console.log(response)
        this.allActiveGroup = response;
      }
    })
  }

}
