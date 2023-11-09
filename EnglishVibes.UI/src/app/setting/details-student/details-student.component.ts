import { Component, OnInit } from '@angular/core';
import { AdminService } from '../Services/admin.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-details-student',
  templateUrl: './details-student.component.html',
  styleUrls: ['./details-student.component.css']
})
export class DetailsStudentComponent implements OnInit {
  DetailsStudent: any;
  studentId: any;

  constructor(private _AdminService: AdminService, private _ActivatedRoute: ActivatedRoute) { }
  ngOnInit(): void {
    this._ActivatedRoute.paramMap.subscribe((param) => {
      this.studentId = param.get('id');
    })
    this._AdminService.getDetailsStudent(this.studentId).subscribe({
      next: (response) => {
        console.log(response)
        this.DetailsStudent = response;
      }
    })
  }

}
