import { AuthService } from './../mainServices/auth.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertifyServicesService } from '../mainServices/alertify-services.service';

@Component({
  selector: 'app-setting-menu',
  templateUrl: './setting-menu.component.html',
  styleUrls: ['./setting-menu.component.css']
})
export class SettingMenuComponent implements OnInit {

  constructor(private alertifyService: AlertifyServicesService, private route: Router, private authService: AuthService) { }
  isAdmin: boolean;
  ngOnInit() {
  }

  logout() {  // wylogowanie
    localStorage.removeItem('token'); // usuwa token z 'lokalnego magazynu'
    console.log('logged out');
    // i przeladowuje na zakladke register
    this.alertifyService.message('Jestes wylogowany.');
    this.route.navigate(['/register']);
  }

  OpenProfile() {
    this.route.navigate(['Profile']);
  }

  OpenAccountPanel() {
    this.route.navigate(['AccountPanel']);
  }

  OpenMainPage() {
    this.route.navigate(['home/main']);
  }

  checkRole() {
    const role = this.authService.decodedToken.role;  // pobiera role usera
    if ( role === 'admin') {  // jezeli admin to pokaz panel administrator
      this.isAdmin = true;
    } else {
      this.isAdmin = false;
    }
  }

}
