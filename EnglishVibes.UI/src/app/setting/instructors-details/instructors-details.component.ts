import { Component ,OnInit } from '@angular/core';
import { AdminService } from '../Services/admin.service';
import { ActivatedRoute } from '@angular/router';
import { DayService, WeekService, WorkWeekService, MonthService, AgendaService } from '@syncfusion/ej2-angular-schedule';
import { EventSettingsModel, ScheduleModule } from '@syncfusion/ej2-angular-schedule';

@Component({
  selector: 'app-instructors-details',
  // templateUrl: './instructors-details.component.html',
  styleUrls: ['./instructors-details.component.css'],
  template: `<ejs-schedule> </ejs-schedule>`,

  providers: [DayService, WeekService, WorkWeekService, MonthService, AgendaService], 

})


export class InstructorsDetailsComponent implements OnInit {
  allinstrutorsid: any;
  instructorId: any;
  public eventSettings: EventSettingsModel;

  constructor(private _AdminService: AdminService, private _ActivatedRoute: ActivatedRoute) { 

    this.eventSettings = {
      dataSource: [
        {
          Id: 1,
          Subject: 'Meeting with Client',
          StartTime: new Date(2023, 10, 8, 10, 0),
          EndTime: new Date(2023, 10, 8, 11, 0),
        },
        {
          Id: 2,
          Subject: 'Team Stand-up',
          StartTime: new Date(2023, 10, 8, 12, 0),
          EndTime: new Date(2023, 10, 8, 12, 30),
        },
        // Add more events as needed
      ],
    };
  }
  ngOnInit(): void {
    this._ActivatedRoute.paramMap.subscribe((param) => {
      this.instructorId = param.get('id');
    })
    this._AdminService.getinstructorsid(this.instructorId).subscribe({
      next: (response) => {
        console.log(response)
        this.allinstrutorsid = response;
      }
    })
   
  }

}









