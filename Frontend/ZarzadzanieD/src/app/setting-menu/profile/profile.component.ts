import { AlertifyServicesService } from 'src/app/mainServices/alertify-services.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/mainServices/auth.service';
import { UserNameDto, UserPasswordDto } from 'src/app/Domain/User';
import { MatDialog } from '@angular/material';
import { RemoveAccoutComponent } from './remove-accout/remove-accout.component';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  userName: any = {}; // obiekt z polem nazwy usera
  userPassword: any = {}; // obiekt z haslami usera
  userId;

  constructor(private alertifyService: AlertifyServicesService, private route: Router,
    private authService: AuthService, public dialog: MatDialog) { }

  ngOnInit() {
    this.userId = this.authService.decodedToken.nameid;
    console.log('pobieram id konta: ' + this.userId);

  }


  ChangeUserName() {
    const user: UserNameDto = ({
      id: this.userId,
      userName: this.userName.userName
    });
    this.authService.updateUserNameAccount(user).subscribe( p => {
      console.log(p);
      this.alertifyService.success('Operacja zmiany nazwy użytkownika powiodła się.');
    }, (error) => {
      this.alertifyService.error('Operacja zmiany nazwy użytkownika nie powiodła się.');
    });
  }

  ChangePasswordUser() {

    const user: UserPasswordDto = ({
      id: this.userId,
      Password1: this.userPassword.userPassword1,
      Password2: this.userPassword.userPassword2
    });
    this.authService.updatePasswordAccount(user).subscribe( p => {
      console.log(p);
      this.alertifyService.success('Operacja zmiany hasła powiodła się.');
    }, (error) => {
      this.alertifyService.error('Operacja zmiany hasła nie powiodła się.');
    });
  }

  OpenHome() {
    this.route.navigate(['/home/main']);
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(RemoveAccoutComponent, { // otwiera dynamiczny komponent
      // ktory jest przekazany do metody a z nim jeszcze przekazujemy oczekiwany rozmiar
      width: '30%'
    });
    // to nie potrzebne ale jakby chcial cos zrobic bo zamknieciu okna to tak
    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      console.log(result);
    });
  }

}
