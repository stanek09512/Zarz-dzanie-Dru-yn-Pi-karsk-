import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { Router } from '@angular/router';
import { User } from 'src/app/Domain/User';
import { AlertifyServicesService } from 'src/app/mainServices/alertify-services.service';
import { AuthService } from 'src/app/mainServices/auth.service';

@Component({
  selector: 'app-romove-account-admin',
  templateUrl: './romove-account-admin.component.html',
  styleUrls: ['./romove-account-admin.component.css']
})
export class RomoveAccountAdminComponent implements OnInit {

  constructor(private alertifyService: AlertifyServicesService, public dialogRef: MatDialogRef<RomoveAccountAdminComponent>,
    // wstrzykniecie biblioteki z deklaracja obiektu okna dynamicznego jako
    // tego komponentu w ktorym wlasnie to deklarujemy
     private authService: AuthService, private route: Router,
     @Inject(MAT_DIALOG_DATA) public data: User) {}

  ngOnInit() {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  deleteAccount() {
    console.log(this.data.id + ' ' + this.data.username);
    this.authService.deleteUserAccount(this.data.id).subscribe( p => {
      console.log(p);
      this.alertifyService.success('Operacja powiodła się, usunąłes konto');
    }); // usuwa konta z bazy
    this.alertifyService.error('Operacja nie powiodła się, sprobuj ponownie');
    this.onNoClick();
  }
}
