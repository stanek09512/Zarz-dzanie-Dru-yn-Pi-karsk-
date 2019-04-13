
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';


import { HttpClientModule } from '@angular/common/http';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { FilterSurnamePipe } from './shared/filter-surname.pipe';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatMenuModule, MatButtonModule, MatIconModule, MatCardModule,
  MatFormFieldModule, MatListModule, MatInputModule, MatSelectModule } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { HttpPlayerService } from './mainServices/http-player.service';

import { CalendarComponent } from './mecze/calendar/calendar.component';
import { HttpMatchService } from './mainServices/http-match.service';
import { FilterMatchPipe } from './shared/filter-match.pipe';
import { AddMatchComponent } from './mecze/add-match/add-match.component';
import {MatStepperModule} from '@angular/material/stepper';
import { MatchDetailsComponent } from './mecze/calendar/match-details/match-details.component';
import { MainPageComponent } from './main-page/main-page.component';
import { MatchHistoryComponent } from './mecze/match-history/match-history.component';
import { EditScoreHistoryMatchComponent } from './mecze/match-history/edit-score-history-match/edit-score-history-match.component';
import {MatTreeModule} from '@angular/material/tree';
import { SettingMenuComponent } from './setting-menu/setting-menu.component';
import { ProgressBarComponent } from './progress-bar/progress-bar.component';
import { Menu2Component } from './menu2/menu2.component';
import { NavigationBarComponent } from './navigation-bar/navigation-bar.component';
import { AuthService } from './mainServices/auth.service';
import { RegisterComponent } from './register/register.component';
import { ApiHomePageComponent } from './api-home-page/api-home-page.component';
import { AuthGuard } from './_guards/auth.guard';
import { NotAuthGuard } from './_guards/not-auth.guard';
import { ProfileComponent } from './setting-menu/profile/profile.component';

import {MatDialogModule} from '@angular/material/dialog';
import { RemoveAccoutComponent } from './setting-menu/profile/remove-accout/remove-accout.component';

import { AdminAccountGuard } from './_guards/admin-account.guard';
import { AccountPanelComponent } from './setting-menu/account-panel/account-panel.component';
import { RomoveAccountAdminComponent } from './setting-menu/account-panel/romove-account-admin/romove-account-admin.component';
import { AlertifyServicesService } from './mainServices/alertify-services.service';
import { PlayersComponent } from './team/players/players.component';
import { PlayerDetailsComponent } from './team/players/player-details/player-details.component';
import { AddPlayerComponent } from './team/add-player/add-player.component';
import { PlrStatsListComponent } from './team/playerStats/plr-stats-list/plr-stats-list.component';







@NgModule({
  declarations: [
    AppComponent,
    PlayersComponent,
    PageNotFoundComponent,
    FilterSurnamePipe,
    PlayerDetailsComponent,
    AddPlayerComponent,
    PlrStatsListComponent,
    CalendarComponent,
    FilterMatchPipe,
    AddMatchComponent,
    MatchDetailsComponent,
    MainPageComponent,
    MatchHistoryComponent,
    EditScoreHistoryMatchComponent,
    SettingMenuComponent,
    ProgressBarComponent,
    Menu2Component,
    NavigationBarComponent,
    RegisterComponent,
    ApiHomePageComponent,
    ProfileComponent,
    AccountPanelComponent,
    RemoveAccoutComponent,
    RomoveAccountAdminComponent

    ],
    imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    MatMenuModule,
    MatButtonModule,
    MatCardModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatListModule,
    MatStepperModule,
    MatInputModule,
    MatTreeModule,
    MatIconModule,
    MatSelectModule,
    MatDialogModule,


  ],
  entryComponents: [ RemoveAccoutComponent, RomoveAccountAdminComponent],
  providers: [HttpPlayerService,
  HttpMatchService,
  AuthService,
  AuthGuard,
  NotAuthGuard,
  AdminAccountGuard,
  AlertifyServicesService],
  bootstrap: [AppComponent]
})
export class AppModule { }
