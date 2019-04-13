import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User, UserNameDto, UserPasswordDto, UserRoleDto } from '../Domain/User';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  baseUrl = 'https://localhost:44356/Auth/';
  jwtHelper = new JwtHelperService(); // obiekt biblioteki
  decodedToken: any; // przypisze obiekt zdekodowanego tokenu

  // userId = this.decodedToken.nameid;

  login(model: any) {
    return this.http.post(this.baseUrl + 'Login', model).pipe(  // korzysta normalnie z metody post
      // ale uzywa jeszcze pipe zeby i map zeby pobrac i skopiowac token
      map((response: any) => {  // mapuje odpowiedz i przypisuje do stalej user
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);  // i jezeli dpowiedz nie byla pusta to zapisuje w localStorage
          // pod nazwa token wartosc z odpowiedzi user.teken
          this.decodedToken = this.jwtHelper.decodeToken(user.token); // dekoduje token
          console.log(this.decodedToken);
        }
      })
    );
  }

  register(model: any) {
    return this.http.post(this.baseUrl + 'Register' , model);
  }

  leggedIn() {  // sprawdza czy jestes zalogowany tzn czy token nie jest przedawniony
    const token = localStorage.getItem('token');  // pobieram token z kontenera
    return !this.jwtHelper.isTokenExpired(token); // i uzywajac biblioteki jwtHElper i odpowiedniej metody sprawdzamy to
  }

  getUsersAccounts(): Observable<Array<User>>  {  // pobiera liste uzytkownikow
    console.log('pobieram wszystkie konta uzytkownikow');
    return this.http.get<Array<User>>(this.baseUrl + 'GetUsersAccounts');
  }

  getOneUserAccount(userId: number): Observable<User> {  // pobiera konto uzytkownika
    console.log('pobieram konto jednego uzytkownika');
    return this.http.get<User>(this.baseUrl + 'GetOneUserAccount/' + userId);
  }

  deleteUserAccount(userId: number) {  // usuwanie konta usera
    return this.http.delete(this.baseUrl + 'RemoveOneUserAccount/' + userId, { observe: 'response' }); //  responseType: 'text',
  }

  updateUserNameAccount( user: UserNameDto) {  // edycja nazwe uzytkownika konta
    return this.http.put(this.baseUrl + 'PutUserAccountUserName/' + user.id, user, { observe: 'response' });
  }

  updatePasswordAccount(user: UserPasswordDto) {  // edycja haslo uzytkownika konta
    return this.http.put(this.baseUrl + 'PutUserAccountPassword/' + user.id, user, { observe: 'response' });
  }

  updateUserRole(user: UserRoleDto) {  // edycja haslo uzytkownika konta
    return this.http.put(this.baseUrl + 'PutUserRole/' + user.id, user, { observe: 'response' });
  }
}

