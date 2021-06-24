import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './auth/auth-guard';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { HomeComponent } from './Home/home/home.component';
import { PermissionListComponent } from './permission/permission-list/permission-list.component';
import { RolesListComponent } from './role/roles-list/roles-list.component';
import { ShellComponent } from './shell/shell.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', component: LoginComponent },
  {
    path: '',
    component: ShellComponent,
    children:[
      { path: 'home', component: HomeComponent ,canActivate:[AuthGuard]},
      { path: 'roles', component: RolesListComponent,canActivate:[AuthGuard] },
      { path: 'permissions', component: PermissionListComponent,canActivate:[AuthGuard] },
    ]
    ,canActivate:[AuthGuard]
  },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
