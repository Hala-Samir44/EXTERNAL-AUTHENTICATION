
import { Subject } from 'rxjs';
import { Router } from '@angular/router';
import { Inject, Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';
@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(
    private http: HttpClient,
    private router: Router
  ) {}
  readonly baseUrl = '';

  isSelected = false;
  idToken="";
  private tokenTimer: any;
  public afterLoginAction: string="";
  isAuth = false;
  private authStatusListener = new Subject<boolean>();
  public RolesStatusListener = new Subject<string[]>();
  incorrect = false;
  incorrectMassege = '';
  Roles: string[]=[];

  loginUser = (incorrectMassege: string) => {
    return ({ data, username }: any) => {
      if (data != null) {
        this.idToken = data.token;
        if (this.idToken) {
          const expiresInDuration = data.expiresIn;
          this.setAuthTimer(expiresInDuration);
          this.authStatusListener.next(true);
          const now = new Date();
          const expirationDate = new Date(now.getTime() + expiresInDuration * 1000);
          this.saveAuthData(this.idToken, expirationDate, username);
          this.isAuth = true;
          if (this.afterLoginAction != null) {
          }
          // execute callbacl coming from LoginComponent

          this.router.navigate(['/home']);
        }
      } else {
        this.incorrect = true;
        this.incorrectMassege = incorrectMassege;
        this.authStatusListener.next(false);
      }
    };
  };

  getAuthStatusListener() {
    return this.authStatusListener.asObservable();
  }
  getRolesStatusListener() {
    return this.RolesStatusListener.asObservable();
  }
  private setAuthTimer(duration: number) {
    this.tokenTimer = setTimeout(() => {
      this.logout();
    }, duration * 1000);
  }
  logout() {
    this.incorrect = false;
    this.idToken = "";
    this.authStatusListener.next(false);
    this.RolesStatusListener.next([]);
    this.isAuth = false;
    clearTimeout(this.tokenTimer);
    this.clearAuthData();
    this.router.navigate(['/']);
  }
  private saveAuthData(token: string, expirationDate: Date, username: string) {
    localStorage.setItem('token', token);
    localStorage.setItem('expiration', expirationDate.toISOString());
    localStorage.setItem('username', username);
    localStorage.setItem('currentProfile', 'student');
  }

   clearAuthData() {
    localStorage.removeItem('token');
    localStorage.removeItem('expiration');
    localStorage.removeItem('username');
    localStorage.removeItem('roles');
    localStorage.removeItem('currentProfile');
  }

  autoAuthUser() {
    this.incorrect = false;
    const authInformation = this.getAuthData();
    if (!authInformation) {
      return;
    }
    const now = new Date();
    const expiresIn = authInformation.expirationDate.getTime() - now.getTime();

    if (expiresIn > 0) {
      this.idToken = authInformation.token;
      this.isAuth = true;
      this.authStatusListener.next(true);
      this.RolesStatusListener.next(this.Roles);
      this.setAuthTimer(expiresIn / 1000);
    }
  }

  private getAuthData() {
    const token = localStorage.getItem('token');
    const expirationDate = localStorage.getItem('expiration');
    if (!token || !expirationDate) {
      return;
    }
    return { token: token, expirationDate: new Date(expirationDate) };
  }

SocialRegister(IdToken: string, type: string) {
    this.incorrect = false;
    this.idToken = IdToken;

    var data = {
        Token: IdToken,
        ExternalLoginType: type,
      };
      
      if (type.toLocaleLowerCase() === 'google') {
        return this.http.post('https://localhost:44307/api/Authentication/GoogleSignUp', data).subscribe(
          (res: any) => {
            if (res.status == 5) {
              this.incorrect = true;
              this.incorrectMassege = 'Email Already Exists';
              this.authStatusListener.next(false);
              
            } else if (res.status == 1) {
              this.incorrect = false;
              const loginuser = this.loginUser('');
              loginuser({ data: res.data, username: res.username });
            }
            return this.incorrect;
          },
          (error) => {
            this.authStatusListener.next(false);
          }
        );
       
      } else {
        return this.http.post('/Authentication/FacebookSignUp', data).subscribe(
          (res: any) => {
            if (res.status == 3) {
              this.incorrect = true;
              this.incorrectMassege = 'Email Already Exists';
              this.authStatusListener.next(false);
            } else if (res.status == 1) {
              const loginuser = this.loginUser('');
              loginuser({ data: res.data, username: res.username });
            }
          },
          (error) => {
            this.authStatusListener.next(false);
          }
        );
      }
      
  }
  login(formData: any, rout: string) {
    this.incorrect = false;

    this.http.post('https://localhost:44307/api/Authentication/' + rout, formData).subscribe(
      (res: any) => {
        console.log("halalo",res);
        if (res.status == 3 || res.status == 0) {
          this.incorrect = true;
          this.incorrectMassege = 'Incorrect UserName Or Password';
          if (rout === 'ExternalLogin') {
            this.incorrectMassege = "User doesn't Exist please signup";
          }
          this.authStatusListener.next(false);
        } else if (res.status == 1) {
          const loginuser = this.loginUser('');
          loginuser({ data: res.data, username: res.username });
        }
      },
      (error) => {
        this.incorrect = true;
        this.incorrectMassege = 'Incorrect UserName Or Password';
        this.authStatusListener.next(false);
      }
    );
    return this.incorrectMassege? false:true;
  }
}
