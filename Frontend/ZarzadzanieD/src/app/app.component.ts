import { Component, OnInit } from '@angular/core';

import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthService } from './mainServices/auth.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  jwtHelper = new JwtHelperService(); // obiekt biblioteki

  constructor(private authService: AuthService) { }
  ngOnInit() {
    const token = localStorage.getItem('token');  // pobieram token z kontenera
    if (token) { // jezeli token istnieje
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);  // zdekoduj token
    }
  }
}

