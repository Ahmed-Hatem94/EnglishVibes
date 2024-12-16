import { Component } from '@angular/core';
import { AdminService } from '../Services/admin.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-inactive-details',
  templateUrl: './inactive-details.component.html',
  styleUrls: ['./inactive-details.component.css']
})
export class InactiveDetailsComponent {
  groupId: any;
  group: any;
  groupform: FormGroup;
  dayOfWeek: string[] = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday']

  currentDate = new Date();

  year = this.currentDate.getFullYear();
  month = (this.currentDate.getMonth() + 1).toString().padStart(2, '0');
  day = this.currentDate.getDate().toString().padStart(2, '0');
  formattedDate = `${this.year}-${this.month}-${this.day}`
  constructor(private _AdminService: AdminService, private _ActivatedRoute: ActivatedRoute, private _FormBuilder: FormBuilder, private toastr: ToastrService, private _Router: Router) {
    this.groupform = _FormBuilder.group({
      id: [null, [Validators.required]],
      StartDate: [null, [Validators.required,]],
      instructorId: [null, [Validators.required]],
      TimeSlot: [null, [Validators.required]],
      d1: [null, [Validators.required]],
      d2: [null, [Validators.required]]
    })
  }
  ngOnInit(): void {
    this._ActivatedRoute.paramMap.subscribe((param) => {
      this.groupId = param.get('id');
    })
    this._AdminService.getInActiveGroupById(this.groupId).subscribe({
      next: (response) => {
        this.group = response;
        console.log(this.group)
      }
    })
  }


  sendGroup(formData: FormGroup) {
    let dateeeee = {
      id: formData.value.id,
      StartDate: formData.value.StartDate,
      instructorId: formData.value.instructorId,
      TimeSlot: formData.value.TimeSlot + ':00',
      day1: formData.value.d1,
      day2: formData.value.d2,
    };
    this._AdminService.generateInActiveGroup(this.groupId, dateeeee).subscribe({
      next: (respone) => {
        this.toastr.success('Success', 'Completed Data', {
          positionClass: 'toast-bottom-right'
        });
        this._Router.navigate(["/setting/admin/Inactive"])
      },
      error: (error) => {
        this.toastr.error('Sorry', 'Try Again', {
          positionClass: 'toast-bottom-right'
        })
      }
    })
  }

  get StartDate() {
    return this.groupform.get('StartDate')
  }
  get TimeSlot() {
    return this.groupform.get('TimeSlot')
  }
  get instructorId() {
    return this.groupform.get('instructorId')
  }
  get d1() {
    return this.groupform.get('d1')
  }
  get d2() {
    return this.groupform.get('d2')
  }
}
