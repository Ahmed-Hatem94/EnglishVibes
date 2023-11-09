import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SettingRoutingModule } from './setting-routing.module';
import { AdminComponent } from './admin/admin.component';
import { ActiveGroupComponent } from './active-group/active-group.component';
import { ActiveStudentsComponent } from './active-students/active-students.component';
import { WaitingListComponent } from './waiting-list/waiting-list.component';
import { InactiveGroupComponent } from './inactive-group/inactive-group.component';
import { InstructorsComponent } from './instructors/instructors.component';
import { ReactiveFormsModule } from '@angular/forms';
import { InactiveDetailsComponent } from './inactive-details/inactive-details.component';
import { DetailsStudentComponent } from './details-student/details-student.component';
import { InstructorsDetailsComponent } from './instructors-details/instructors-details.component';
import { CompleteWaitinComponent } from './complete-waitin/complete-waitin.component';
import { ScheduleModule, RecurrenceEditorModule } from '@syncfusion/ej2-angular-schedule';
import { DayService, WeekService, WorkWeekService, MonthService, AgendaService, MonthAgendaService, TimelineViewsService, TimelineMonthService } from '@syncfusion/ej2-angular-schedule';
import { AdminDetailsComponent } from './admin-details/admin-details.component';
import { DatePipe } from '@angular/common'; // Import DatePipe

@NgModule({
  declarations: [
    AdminComponent,
    ActiveGroupComponent,
    ActiveStudentsComponent,
    WaitingListComponent,
    InactiveGroupComponent,
    InactiveDetailsComponent,
    DetailsStudentComponent,
    InstructorsComponent,
    InstructorsDetailsComponent,
    CompleteWaitinComponent,
    AdminDetailsComponent,

  ],
  imports: [
    CommonModule,
    SettingRoutingModule,
    ReactiveFormsModule,
    ScheduleModule,
    RecurrenceEditorModule,
  ],
  providers: [DayService, WeekService, WorkWeekService, MonthService, AgendaService, MonthAgendaService, TimelineViewsService, TimelineMonthService, DatePipe],

})
export class SettingModule { }
