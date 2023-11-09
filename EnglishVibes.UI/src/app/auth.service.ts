import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { Router } from '@angular/router';
import { is, tr } from 'date-fns/locale';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  isAdmin: BehaviorSubject<boolean>;
  isLogged: BehaviorSubject<boolean>;
  userData = null;

  constructor(private _HttpClient: HttpClient, private _Router: Router) {
    this.isAdmin = new BehaviorSubject(false);
    this.isLogged = new BehaviorSubject(false);
    if (localStorage.getItem("userToken")) {
      this.isLogged.next(true)
      if (localStorage.getItem("role")?.includes("admin"))
        this.isAdmin.next(true);
    }
  }
  decodeuserdata() {
    let encodedToken = JSON.stringify(localStorage.getItem('userToken'))
    let decodedtoken: any = jwtDecode(encodedToken);
    this.userData = decodedtoken;
    this.isLogged.next(true);
  }



  register(userData: object): Observable<any> {
    return this._HttpClient.post('https://localhost:44385/api/Account/register/student', userData)
  }

  login(userData: object): Observable<any> {
    return this._HttpClient.post('https://localhost:44385/api/Account/login', userData)
  }

  // inactiveGroup(userData:object):Observable<any>
  // {

  //   return this._HttpClient.get('https://localhost:44385/api/Group/inactive',userData)
  // }

  inactiveGroup(): Observable<string> {
    return this._HttpClient.get<string>('https://localhost:44385/api/Group/inactive'); // Replace with your actual API endpoint
  }

  logOut() {
    localStorage.removeItem("userToken")
    localStorage.removeItem("role")
    this.isLogged.next(false);
    this.isAdmin.next(false);
    this._Router.navigate(['/home'])
  }
}
