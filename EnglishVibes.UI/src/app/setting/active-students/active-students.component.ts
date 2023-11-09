import { Component, OnInit } from '@angular/core';
import { AdminService } from '../Services/admin.service';

@Component({
  selector: 'app-active-students',
  templateUrl: './active-students.component.html',
  styleUrls: ['./active-students.component.css']
})

export class ActiveStudentsComponent implements OnInit {
  allActiveStudent: any;
  constructor(private _AdminService: AdminService) { }
  ngOnInit(): void {
    this._AdminService.getActiveStudent().subscribe({
      next: (response) => {
        console.log(response)
        this.allActiveStudent = response;
      }
    })
  }

}


