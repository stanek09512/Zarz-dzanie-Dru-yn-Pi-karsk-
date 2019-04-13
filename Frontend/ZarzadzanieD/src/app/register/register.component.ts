import { Component, OnInit } from '@angular/core';

import { AlertifyServicesService } from './../mainServices/alertify-services.service';
import { AuthService } from './../mainServices/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  model: any = {};
  homeMode = true;
  constructor(private authService: AuthService, private alertifyService: AlertifyServicesService) { }

  ngOnInit() {
  }


  register() {
    if (this.model.password !== this.model.password2) {
      this.alertifyService.error('Podane hasła nie sa takie same.');
    } else {
      this.authService.register(this.model).subscribe( x => {
        console.log('register success');
        this.model.userName = '';
        this.model.password = '';
        this.alertifyService.success('Rejestracja powiodłą się, teraz możesz się zalogować.');
      }, error => {
        console.log(error);
        this.alertifyService.error('Rejestracja nie powiodłą się, sprobuj ponownie');
      });
    }

  }


  cancel() {  // to zmienia wartosc flagi co do wyslewilania czesci kodu html
    this.homeMode = true;
  }

  registerToggle() {
    this.homeMode = !this.homeMode;
  }
}
