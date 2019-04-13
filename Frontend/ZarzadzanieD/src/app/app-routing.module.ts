
import { NgModule } from '@angular/core';
import { RouterModule, Routes, PreloadAllModules } from '@angular/router';

import { PageNotFoundComponent } from './page-not-found/page-not-found.component';

import { CalendarComponent } from './mecze/calendar/calendar.component';
import { AddMatchComponent } from './mecze/add-match/add-match.component';
import { MatchDetailsComponent } from './mecze/calendar/match-details/match-details.component';
import { MainPageComponent } from './main-page/main-page.component';
import { MatchHistoryComponent } from './mecze/match-history/match-history.component';
import { EditScoreHistoryMatchComponent } from './mecze/match-history/edit-score-history-match/edit-score-history-match.component';
import { RegisterComponent } from './register/register.component';
import { ApiHomePageComponent } from './api-home-page/api-home-page.component';
import { AuthGuard } from './_guards/auth.guard';
import { NotAuthGuard } from './_guards/not-auth.guard';
import { ProfileComponent } from './setting-menu/profile/profile.component';
import { AdminAccountGuard } from './_guards/admin-account.guard';
import { AccountPanelComponent } from './setting-menu/account-panel/account-panel.component';
import { PlayersComponent } from './team/players/players.component';
import { PlayerDetailsComponent } from './team/players/player-details/player-details.component';
import { AddPlayerComponent } from './team/add-player/add-player.component';
import { PlrStatsListComponent } from './team/playerStats/plr-stats-list/plr-stats-list.component';




const appRoutes: Routes = [
{
  path: '',
  redirectTo: '/register',
  pathMatch: 'full' // tu ustawia ze przekieruje tu ale tylko jezeli CAŁĄ sciezka pasuej co do znaku
},
{
  path: 'register',
  canActivate: [NotAuthGuard],
  component: RegisterComponent // tu powinna  byc strona głwna
},
{
  path: 'home',
  component: ApiHomePageComponent,
  canActivate: [AuthGuard], // uzywamy naszego straznika ktory sprawdza czy mozna aktywowac zakladke
  children: [
    {
      path: 'main',
      component: MainPageComponent
    },
    {
      path: 'players',
      component: PlayersComponent
    },
    {
      path: 'players/:id',
      component: PlayerDetailsComponent
    },
    {
      path: 'matchHistory',
      component: MatchHistoryComponent
    },
    {
      path: 'matchHistory/:id',
      component: EditScoreHistoryMatchComponent
    },
    {
      path: 'AddPlayer',
      component: AddPlayerComponent
    },
    {
      path: 'StatsList',
      component: PlrStatsListComponent
    },
    {
      path: 'Calendar',
      component: CalendarComponent
    },
    {
      path: 'Calendar/:id',
      component: MatchDetailsComponent
    },
    {
      path: 'AddMatch',
      component: AddMatchComponent
    },
  ]
},
{
  path: 'Profile',
  canActivate: [AuthGuard],
  component: ProfileComponent
},
{
  path: 'AccountPanel',
  canActivate: [AdminAccountGuard, AuthGuard], // musi byc adminem i zalogowany
  component: AccountPanelComponent
},
{
  path: '**', // jezeli nie znajdzie powyzszych adresow to wysweitli to
  component: PageNotFoundComponent
}

];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
