import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './auth/login/login.component';
import { AddRoleComponent } from './role/add-role/add-role.component';
import { RegisterComponent } from './auth/register/register.component';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { SocialLoginModule, SocialAuthServiceConfig } from 'angularx-social-login';
import {FacebookLoginProvider, GoogleLoginProvider } from 'angularx-social-login';
import { HomeComponent } from './Home/home/home.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RolesListComponent } from './role/roles-list/roles-list.component';
import { ShellComponent } from './shell/shell.component';
import { AuthInterceptor } from './auth/auth-interceptor';
import { PermissionListComponent } from './permission/permission-list/permission-list.component';
import { AddPermissionComponent } from './permission/add-permission/add-permission.component';

import { AuthGuard } from './auth/auth-guard';



// let config = new SocialAuthServiceConfig([
//   {
//     id: GoogleLoginProvider.PROVIDER_ID,
//     provider: new GoogleLoginProvider('933409184992-pbdbfktt8ibd0h9ca68es6m36tfralha.apps.googleusercontent.com'),
//   },
//   // {
//   //   id: FacebookLoginProvider.PROVIDER_ID,
//   //   provider: new FacebookLoginProvider('525262001501781'),
//   // },
// ]);

// export function provideConfig() {
//   return config;
// }
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    AddRoleComponent,
    RegisterComponent,
    HomeComponent,
    RolesListComponent,
    ShellComponent,
    PermissionListComponent,
    AddPermissionComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    SocialLoginModule, 
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,

     NgbModule,
  ],
  providers: [
    AuthGuard,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    {
      provide: 'SocialAuthServiceConfig',
      useValue: {
        autoLogin: false,
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,
            provider: new GoogleLoginProvider(
              '933409184992-pbdbfktt8ibd0h9ca68es6m36tfralha.apps.googleusercontent.com'
            )
          }
        ]
      } as SocialAuthServiceConfig,
    }, 
    // {
    //   provide: 'SocialAuthServiceConfig',
    //   useValue: {
    //     autoLogin: false,
    //     providers: [
    //       {
    //         id: FacebookLoginProvider.PROVIDER_ID,
    //         provider: new FacebookLoginProvider(
    //           '1630753603777146'
    //         )
    //       }
    //     ]
    //   } as SocialAuthServiceConfig,
    // }    
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
