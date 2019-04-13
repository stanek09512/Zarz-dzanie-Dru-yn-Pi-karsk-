import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AlertifyServicesService } from './../mainServices/alertify-services.service';
import { AuthService } from './../mainServices/auth.service';

@Component({
  selector: 'app-navigation-bar',
  templateUrl: './navigation-bar.component.html',
  styleUrls: ['./navigation-bar.component.css']
})
export class NavigationBarComponent implements OnInit {
  model: any = {}; // przetrzyma wartosci naszych pol z html
  constructor(public authService: AuthService, private route: Router, private alertifyService: AlertifyServicesService) { }

  ngOnInit() {

  }

  login() {
    console.log(this.model);
    this.authService.login(this.model).subscribe( x => {
      console.log('loggin succesful');
      this.alertifyService.success('Logowanie przebiegło pomylnie.');
    }, error => {
      console.log('Failde to login');
      this.alertifyService.error('Logowanie nie powiodło się, sprobuj ponownie.');
    }, () => {  // i przeladowuje na zakladke home
      this.route.navigate(['/home/main']);
    });
  }

  loggedIn() {
   return this.authService.leggedIn();  // sprawdza czy jestesmy zalogowani

    // const token = localStorage.getItem('token');  // pobiera item o nazwie token z'lokalnego magazynu'
  //  return !!token;
    // te dwa wykrzykniki znacza ze jak token nie jest pusty to zwraca true a jak pusty to zwraca false
  }



}
