import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { Router } from '@angular/router';
import { AlertifyServicesService } from 'src/app/mainServices/alertify-services.service';
import { AuthService } from 'src/app/mainServices/auth.service';

@Component({
  selector: 'app-remove-accout',
  templateUrl: './remove-accout.component.html',
  styleUrls: ['./remove-accout.component.css']
})
export class RemoveAccoutComponent implements OnInit {

  userId;
  constructor(private alertifyService: AlertifyServicesService, public dialogRef: MatDialogRef<RemoveAccoutComponent>,
    // wstrzykniecie biblioteki z deklaracja obiektu okna dynamicznego jako
    // tego komponentu w ktorym wlasnie to deklarujemy
     private authService: AuthService, private route: Router) {}

  ngOnInit() {
    this.userId = this.authService.decodedToken.nameid;
    console.log('pobieram id konta: ' + this.userId);
  }
  onNoClick(): void {
    this.dialogRef.close();
  }

  deleteAccount() {
    this.authService.deleteUserAccount(this.userId).subscribe( p => {
      console.log(p);
      this.alertifyService.success('Operacja powiodła się usunąłe konto.');
    }, (error) => {
      this.alertifyService.error('Operacja nie powiodłą się, sprobuj ponownie');
    }); // usuwa konta z bazy
    localStorage.removeItem('token'); // usuwa token z 'lokalnego magazynu'
    console.log('logged out');
    // i przeladowuje na zakladke register
    this.route.navigate(['/register']);
    this.onNoClick();
  }
}







// public dialogRef: MatDialogRef<RemoveAccoutComponent>,
// @Inject(MAT_DIALOG_DATA) public data: DialogData) {}
