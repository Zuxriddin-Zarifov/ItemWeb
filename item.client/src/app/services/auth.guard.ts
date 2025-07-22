import { Injectable, PLATFORM_ID, inject } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  private readonly router: Router = inject(Router);
  private readonly platformId: Object = inject(PLATFORM_ID);


  public canActivate(): boolean {
    if (isPlatformBrowser(this.platformId)) {
      const token = localStorage.getItem('authToken');
      const expiryTime = localStorage.getItem('expiryTime');

      if (token && expiryTime && new Date().getTime() < parseInt(expiryTime, 10)) {
        return true;
      }
    }

    this.router.navigate(['/login']);
    return false;
  }
}
