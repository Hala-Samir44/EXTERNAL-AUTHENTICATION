import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { SocialAuthService,FacebookLoginProvider, GoogleLoginProvider, SocialUser } from 'angularx-social-login';
import { UserService } from '../user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  resultMessage: string="";
user:SocialUser | undefined;
isSignedin: boolean| undefined;  
notExist=false;
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
  
  logInWithFacebook(): void {
    this.socialAuthService.signIn(FacebookLoginProvider.PROVIDER_ID);

  }
  logInWithGoogle(): void {


   var platform = GoogleLoginProvider.PROVIDER_ID;
    //Sign In and get user Info using authService that we just injected
    this.socialAuthService.signIn(platform).then(
      (response) => {
        //Get all user details
        var idToken = response.idToken;
        var provider = response.provider;
        var externalLoginDto = {
          Token: response.idToken,
          ExternalLoginType: response.provider,
        };
       this.notExist= this.userService.login(externalLoginDto, 'ExternalLogin');
      },
      (error) => {
        console.log(error);
        this.resultMessage = error;
      }
    );
  }

  
}
