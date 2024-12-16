import { Component, OnInit } from '@angular/core';
import { AdminService } from '../Services/admin.service';

@Component({
  selector: 'app-waiting-list',
  templateUrl: './waiting-list.component.html',
  styleUrls: ['./waiting-list.component.css']
})
export class WaitingListComponent implements OnInit {
  WaitingList: any;
  constructor(private _AdminService: AdminService) { }
  ngOnInit(): void {
    this._AdminService.getWaitingList().subscribe({
      next: (response) => {
        console.log(response)
        this.WaitingList = response;
      }
    })
  }

}


