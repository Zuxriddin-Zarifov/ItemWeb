import { LoginComponent } from './component/login/login.component';
import { RegistrationComponent } from './component/registration/registration.component';
import { ItemViewComponent } from './component/item-view/item-view.component';
import { ItemEditComponent } from './component/item-edit/item-edit.component';
import { LayoutComponent } from './component/layout/layout.component';
import { AuthGuard } from './services/auth.guard';
import { Routes } from '@angular/router';


export const appRoutes: Routes = [
  {
    path: "",
    component: LayoutComponent,
    children: [
      { path: '', redirectTo: 'item-view', pathMatch: 'full' },
      { path: 'login', component: LoginComponent },
      { path: 'registration', component: RegistrationComponent },
      { path: 'item-view', component: ItemViewComponent, canActivate: [AuthGuard] },
      { path: 'item-add', component: ItemEditComponent, canActivate: [AuthGuard] },
      { path: 'item-edit', component: ItemEditComponent, canActivate: [AuthGuard] }
    ]
  }
];
