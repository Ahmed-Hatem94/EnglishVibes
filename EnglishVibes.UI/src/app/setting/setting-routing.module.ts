import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin/admin.component';
import { InstructorsComponent } from './instructors/instructors.component';
import { ActiveGroupComponent } from './active-group/active-group.component';
import { InactiveGroupComponent } from './inactive-group/inactive-group.component';
import { WaitingListComponent } from './waiting-list/waiting-list.component';
import { ActiveStudentsComponent } from './active-students/active-students.component';
import { InactiveDetailsComponent } from './inactive-details/inactive-details.component';
import { DetailsStudentComponent } from './details-student/details-student.component';
import { InstructorsDetailsComponent } from './instructors-details/instructors-details.component';
import { CompleteWaitinComponent } from './complete-waitin/complete-waitin.component';
import { AdminDetailsComponent } from './admin-details/admin-details.component';
import { isAdminGuard } from './Guards/is-admin.guard';

const routes: Routes = [
  { path: '', redirectTo: 'admin', pathMatch: 'full' },
  {
    path: "admin", component: AdminComponent, children: [
      { path: '', redirectTo: 'admin', pathMatch: 'full' },
      { path: 'ins', component: InstructorsComponent, canActivate: [isAdminGuard] },
      { path: 'active', component: ActiveGroupComponent, canActivate: [isAdminGuard] },
      { path: 'Inactive', component: InactiveGroupComponent, canActivate: [isAdminGuard] },
      { path: 'InactiveDetails/:id', component: InactiveDetailsComponent, canActivate: [isAdminGuard] },
      { path: 'Waiting', component: WaitingListComponent, canActivate: [isAdminGuard] },
      { path: 'actStudent', component: ActiveStudentsComponent, canActivate: [isAdminGuard] },
      { path: 'Detalis/:id', component: DetailsStudentComponent, canActivate: [isAdminGuard] },
      { path: 'instructDetalis/:id', component: InstructorsDetailsComponent, canActivate: [isAdminGuard] },
      { path: 'admindetails', component: AdminDetailsComponent, canActivate: [isAdminGuard] },




      { path: 'completeWaiting/:id', component: CompleteWaitinComponent },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SettingRoutingModule { }
