import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent {
  constructor(private _AuthService: AuthService, private _Router: Router) {}
  isloading: boolean = false;
  apierror: string = '';
  registerForm: FormGroup = new FormGroup(
    {
      Age: new FormControl(null, [
        Validators.required,
        Validators.min(16),
        Validators.max(80),
      ]),
      UserName: new FormControl(null, [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(20),
      ]),
      Email: new FormControl(null, [Validators.required, Validators.email]),
      Password: new FormControl(null, [
        Validators.required,
        Validators.pattern(
          /^(?=.*[A-Z])|(?=.*[a-z])|(?=.*\d)(?=.*[!@#$%^&*(),.?":{}|<>])[A-Za-z\d!@#$%^&*(),.?":{}|<>]{8,}$/
        ),
      ]),
      ConfirmPassword: new FormControl(null, [Validators.required]), //,  Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*(),.?":{}|<>])[A-Za-z\d!@#$%^&*(),.?":{}|<>]{8,}$/)]),
      PhoneNumber: new FormControl(null, [
        Validators.required,
        Validators.pattern(/^01[0125][0-9]{8}$/),
      ]),
      StudyPlan: new FormControl(null, [Validators.required]),
    },
    { validators: this.ConfirmPasswordMatch }
  );

  ConfirmPasswordMatch(registerForm: any) {
    let passwordcontrol = registerForm.get('Password');
    let ConfirmPasswordcontrol = registerForm.get('ConfirmPassword');

    if (passwordcontrol.value === ConfirmPasswordcontrol.value) {
      return null;
    } else {
      ConfirmPasswordcontrol.setErrors({
        passwordmatch: 'password and confirmpassword didnt match',
      });
      return { passwordmatch: 'password and confirmpassword didnt match' };
    }
  }

  handleregister(registerForm: FormGroup) {
    this.isloading = true;
    if (registerForm.valid) {
      console.log(registerForm);

      this._AuthService.register(registerForm.value).subscribe({
        next: (response) => {
          if (response.message === 'success') {
            this.isloading = false;
            this._Router.navigate(['../login']);
          }
        },
        error: (err) => {
          this.isloading = false;
          this.apierror = err.error[''];
          console.log(err.error);
        },
      });
    } else console.log('Not Valid');
  }
}
