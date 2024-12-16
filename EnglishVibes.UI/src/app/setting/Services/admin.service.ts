import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private _HttpClient: HttpClient) { }
  userData = null;

  getInActiveGroup(): Observable<any> {
    return this._HttpClient.get("http://localhost:80/api/Group/inactive")
  }
  getActiveGroup(): Observable<any> {
    return this._HttpClient.get("http://localhost:80/api/Group/active")
  }


  getActiveStudent(): Observable<any> {
    return this._HttpClient.get("http://localhost:80/api/Student/active/all")

  }


  getWaitingList(): Observable<any> {
    return this._HttpClient.get("http://localhost:80/api/Student/waitinglist")

  }

  getDetailsStudent(id: any): Observable<any> {

    return this._HttpClient.get(`http://localhost:80/api/Student/active/${id}`)

  }


  decodeuserdata() {
    let encodedToken = JSON.stringify(localStorage.getItem('userToken'))
    let decodedtoken: any = jwtDecode(encodedToken);
    console.log(decodedtoken);
    this.userData = decodedtoken;
  }



  register(userData: object): Observable<any> {
    return this._HttpClient.post('http://localhost:80/api/Account/register/admin', userData)
  }



  getinstructors(): Observable<any> {
    return this._HttpClient.get("http://localhost:80/api/Instructor/all")
  }
  getinstructorsid(id: any): Observable<any> {
    return this._HttpClient.get(`http://localhost:80/api/Instructor/schedule/${id}`)
  }

  getInActiveGroupById(id: any): Observable<any> {
    return this._HttpClient.get(`http://localhost:80/api/Group/${id}`)
  }

  generateInActiveGroup(id: any, data: any): Observable<any> {
    return this._HttpClient.post(`http://localhost:80/api/Group/${id}`, data)
  }
  generateInActiveStudent(id: any, data: any): Observable<any> {
    return this._HttpClient.put(`http://localhost:80/api/Student/${id}`, data)
  }
}
