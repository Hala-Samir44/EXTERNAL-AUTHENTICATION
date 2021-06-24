import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {    } from 'angularx-social-login';
import { SocialAuthService, GoogleLoginProvider,FacebookLoginProvider, SocialUser } from 'angularx-social-login';
import { UserService } from '../user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  notExist=false;
  resultMessage: string="";
  isLoading = false;
  user:SocialUser | undefined;
  isSignedin: boolean| undefined;  
  constructor(private router: Router, private socialAuthService: SocialAuthService,private userService:UserService) { }

  ngOnInit(): void {
    this.socialAuthService.authState.subscribe((user) => {
      this.user = user;
      this.isSignedin = (user != null);
      console.log(this.user);
    });
    if(localStorage.getItem('token'))
    this.router.navigate(['/home']);
  }

  registerWithGoogle(): void {
   var google = GoogleLoginProvider.PROVIDER_ID;

    //Sign In and get user Info using authService that we just injected
    this.socialAuthService.signIn(google).then(
      (response:any) => {
        // //Take the details we need and store in an array
        this.userService.idToken = response.idToken;
        this.isLoading = true;

        this.userService.SocialRegister(response.idToken, 'google');
        // this.router.navigate(['/externalLogin']);
      },
      (error:any) => {
        this.resultMessage = error;
      }
    );
  }

  //logIn with facebook method. Takes the platform (Facebook) parameter.
  registerWithFacebook(): void {
    var platform = FacebookLoginProvider.PROVIDER_ID;
    //Sign In and get user Info using authService that we just injected
    this.socialAuthService.signIn(platform).then(
      (response) => {
        //Get all user details
        this.isLoading = true;
        console.log(response);
        this.userService.SocialRegister(response.idToken, 'facebook');
      },
      (error) => {
        console.log(error);
        this.resultMessage = error;
      }
    );
  }
}
