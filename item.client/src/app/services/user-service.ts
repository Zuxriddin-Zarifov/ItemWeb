import { inject, Injectable, OnInit, PLATFORM_ID } from '@angular/core';
import { UserApiService } from "../api/user-api-service";
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})

export class UserService implements OnInit {
  
  public userApiService: UserApiService = inject(UserApiService);

  public isloggedIn: boolean = false;
  private platformId: Object = inject(PLATFORM_ID);
  public ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      this.isloggedIn = !!localStorage.getItem('authToken');
    }
  }
}