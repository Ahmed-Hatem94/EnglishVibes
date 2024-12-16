import { Component } from '@angular/core';
import { AuthService } from 'src/app/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  isAdmin: boolean = false;
  isLogged: boolean = false;
  imgsrc: string = 'assets/images/logo.png';

  constructor(private _AuthServices: AuthService) { }
  ngOnInit(): void {
    this._AuthServices.isAdmin.subscribe((value) => {
      this.isAdmin = value;
    })
    this._AuthServices.isLogged.subscribe((value) => {
      this.isLogged = value;
    })

  }
  logOut() {
    this._AuthServices.logOut();
  }
}
