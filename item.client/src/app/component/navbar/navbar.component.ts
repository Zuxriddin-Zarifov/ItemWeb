import { Component, inject,PLATFORM_ID,OnInit} from '@angular/core';
import { Router, RouterModule} from '@angular/router';
import { CommonModule,isPlatformBrowser } from '@angular/common';
import { UserService } from '../../services/user-service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent implements OnInit {

  private router: Router = inject(Router);
  public userService = inject(UserService);  
  private platformId = inject(PLATFORM_ID);
  public title = 'item-project';

  public ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      this.userService.isloggedIn = !!localStorage.getItem('authToken');
    }
  }
  public singIn() {
    this.router.navigate(['/login']);
  }
  public singUp() {
    this.router.navigate(['/registration']);
  }
  public logout() {
    localStorage.removeItem('authToken');
    this.router.navigate(['/login']);
    this.userService.isloggedIn = false;
  }
}
