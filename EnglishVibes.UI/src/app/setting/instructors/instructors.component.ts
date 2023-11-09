import { Component, OnInit } from '@angular/core';
import { AdminService } from '../Services/admin.service';

@Component({
  selector: 'app-instructors',
  templateUrl: './instructors.component.html',
  styleUrls: ['./instructors.component.css']
})





export class InstructorsComponent implements OnInit {
  allinstrutors: any;
   allinstrutorsid: any;

  constructor(private _AdminService: AdminService) { }
  ngOnInit(): void {
    this._AdminService.getinstructors().subscribe({
      next: (response) => {
        console.log(response)
        this.allinstrutors = response;
      }
    });

   
  }

}














// export class InstructorsComponent implements OnInit {
//   allinstrutors: any;
//   allinstrutorsid: any;

//   constructor(private _AdminService: AdminService) {}

//   ngOnInit(): void {
//     this._AdminService.getinstructors().subscribe({
//       next: (response) => {
//         console.log(response)
//         this.allinstrutors = response;
//       }
//     });

//     // this._AdminService.getinstructorsid().subscribe({
//     //   next: (response) => {
//     //     console.log(response)
//     //     this.allinstrutorsid = response;
//     //   }
//     // });
//   }
// }

