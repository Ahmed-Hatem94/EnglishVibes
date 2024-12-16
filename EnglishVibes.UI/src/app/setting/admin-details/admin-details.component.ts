import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AdminService } from '../Services/admin.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-details',
  templateUrl: './admin-details.component.html',
  styleUrls: ['./admin-details.component.css']
})




export class AdminDetailsComponent {
  constructor(private AdminService: AdminService, private _Router: Router, private toastr: ToastrService) { }
  isloading: boolean = false;
  apierror: string = '';
  registerForm: FormGroup = new FormGroup({
    Age: new FormControl(null, [Validators.required, Validators.min(16), Validators.max(80)]),
    UserName: new FormControl(null, [Validators.required, Validators.minLength(3), Validators.maxLength(20)]),
    Email: new FormControl(null, [Validators.required, Validators.email]),
    Password: new FormControl(null, [Validators.required, Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*(),.?":{}|<>])[A-Za-z\d!@#$%^&*(),.?":{}|<>]{8,}$/)]),
    ConfirmPassword: new FormControl(null, [Validators.required, Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*(),.?":{}|<>])[A-Za-z\d!@#$%^&*(),.?":{}|<>]{8,}$/)]),
    PhoneNumber: new FormControl(null, [Validators.required, Validators.pattern(/^01[0125][0-9]{8}$/)]),
  }, { validators: this.ConfirmPasswordMatch });


  ConfirmPasswordMatch(registerForm: any) {
    let passwordcontrol = registerForm.get('Password');
    let ConfirmPasswordcontrol = registerForm.get('ConfirmPassword');

    if (passwordcontrol.value === ConfirmPasswordcontrol.value) {
      return null;
    }
    else {
      ConfirmPasswordcontrol.setErrors({ passwordmatch: 'password and confirmpassword didnt match' })
      return { passwordmatch: 'password and confirmpassword didnt match' }
    }
  }




  handleregister(registerForm: FormGroup) {
    this.isloading = true;
    if (registerForm.valid) {
      console.log(registerForm);

      this.AdminService.register(registerForm.value).subscribe({
        next: (response) => {
          console.log(response)
          if (response.message === 'Admin Registered') {
            this.toastr.success('Success', 'Admin Added', {
              positionClass: 'toast-bottom-right'
            });
            this.isloading = false;
            this._Router.navigate(['/setting/admin/ins']);
          }
        },
        error: (err) => {
          this.toastr.error('Sorry', 'Try Again', {
            positionClass: 'toast-bottom-right'
          })
          this.isloading = false;

          console.log(err.error);
        },
      });
    } else console.log('Not Valid');
  }
}

















