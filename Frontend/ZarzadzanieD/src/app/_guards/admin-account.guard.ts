import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../mainServices/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AdminAccountGuard implements CanActivate {
  constructor(private authService: AuthService, private route: Router) {}

  userId;
  role;
  canActivate(  // metoda can activate uzyjemy w routingu
    ): boolean {  // zwraca boola
      this.userId = this.authService.decodedToken.nameid; // przypisuje id uzytkownika
      this.role = this.authService.decodedToken.role; // pobiera role z tokenu

      if (this.role === 'admin') {  // i jezeli rola to admin to pozwala wyswietlic zakladke
        return true;  // to ta metoda zwraca true
      }

        return false;
  }

}
