import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { Observable } from 'rxjs/internal/Observable';
import { User, UserRoleDto } from 'src/app/Domain/User';
import { AuthService } from 'src/app/mainServices/auth.service';
import { RomoveAccountAdminComponent } from 'src/app/setting-menu/account-panel/romove-account-admin/romove-account-admin.component';
import { AlertifyServicesService } from 'src/app/mainServices/alertify-services.service';



@Component({
  selector: 'app-account-panel',
  templateUrl: './account-panel.component.html',
  styleUrls: ['./account-panel.component.css']
})
export class AccountPanelComponent implements OnInit {

  // accountId;
  accountList$: Observable<Array<User>>;
  pemissions: string[];
  constructor(public dialog: MatDialog, private authService: AuthService, private alertifyService: AlertifyServicesService) {}

  ngOnInit() {
    this.accountList$ = this.authService.getUsersAccounts();
    this.accountList$.subscribe( x => {
      console.log(x);
    });

    this.pemissions = [
      'admin',
      'guest',
    ];
  }

  openDialog(userId, userName): void {
    const dialogRef = this.dialog.open(RomoveAccountAdminComponent, {
      width: '350px',
      data: {id: userId, username: userName}
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }


  saveChangesRole(account: User) {
    const userToSave: UserRoleDto = ({
      id: account.id,
      Role: account.role
    });
    this.authService.updateUserRole(userToSave).subscribe( x => {
      // usuwa liste podanych zaznaczonych zawodniow
      console.log(x);
      this.alertifyService.success('Operacja zmiany uprawnień konta powiodła się.');
    },
      (error) => {  // lub wyswietla ewentualne bledy
        this.alertifyService.error('Operacja zmiany uprawnień konta nie powiodła się.');
      });

  }

}
