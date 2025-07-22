import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable, tap } from 'rxjs';
import { User } from '../services/models/user';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserApiService {
  private apiUrl = `${environment.apiBaseUrl}/User`;
  private http: HttpClient = inject(HttpClient);

  public login(email: string, password: string): Observable<string> {
    const params = new HttpParams()
      .set('Email', email)
      .set('Password', password);



    const expiresIn: number = 60 * 60 * 1000; // 1 soat (millisekund)
    const expiryTime = new Date().getTime() + expiresIn;

    return this.http.get<{ accessToken: string }>(`${this.apiUrl}/Login`, { params }).pipe(
      tap(result => {
          localStorage.setItem('authToken', result.accessToken);
          localStorage.setItem('expiryTime', expiryTime.toString());
        
      }),
      map(result => result.accessToken)
    );
  }

  public register(firstName: string, lastName: string, email: string, password: string): Observable<User> {
    const body = {
      FirstName: firstName,
      LastName: lastName,
      Email: email,
      Password: password
    };
    return this.http.post<User>(`${this.apiUrl}/Registration`, body);
  }
}
