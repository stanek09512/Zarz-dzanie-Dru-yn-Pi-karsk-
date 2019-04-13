import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../mainServices/auth.service';

@Injectable({
  providedIn: 'root'
})
export class NotAuthGuard implements CanActivate {
  constructor(private authService: AuthService, private route: Router) {}

  canActivate(  // metoda can activate uzyjemy w routingu
    ): boolean {  // zwraca boola
      if (!this.authService.leggedIn()) {  // i jezeli metoda ktora sprawdza czy jestes zalogowany zwraca true
        return true;  // to ta metoda zwraca true
      }

      this.route.navigate(['**']); // a jezeli metoda zwroci zwroci false czyli
      // ze niezalogowany to przekierowuje ponownie na register i zwraca false
      return false;

  }
}
