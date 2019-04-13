import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from './auth.service';
import { Match } from '../Domain/Match';
import { Player } from '../Domain/Player';
import { Plr2Match } from '../Domain/PlayersInMatch';
import { UpdateStatisticPlayersInMatch, playersStatisticInMatch } from '../Domain/PlayersStatistics';



@Injectable({
  providedIn: 'root'
})
export class HttpMatchService {


  constructor(private http: HttpClient, private authService: AuthService) {

  }

  userId = this.authService.decodedToken.nameid;



  getMatch(id): Observable<Match> {
    return this.http.get<Match>('https://localhost:44356/Match/GetMatch/' + id);  // metoda pobierajaca jeden mecz
  }

  getMatchesInPast(): Observable<Array<Match>> { // metoda pobierajaca liste wszystkich meczow w przeszlosci
    return this.http.get<Array<Match>>('https://localhost:44356/Match/GetAllMatchesPast/' + this.userId);
  }

  getMatchesInFeature(): Observable<Array<Match>> { // metoda pobierajaca liste wszystkich meczow w przyszlosci
    return this.http.get<Array<Match>>('https://localhost:44356/Match/GetAllMatchcesFuture/' + this.userId);
  }

  getPlayersInMatch(id: number): Observable<Array<Player>> { // metoda pobierajaca liste wszystkich meczow
    return this.http.get<Array<Player>>('https://localhost:44356/Match/GetPlayersInMatch/' + id );
  }

  DeletePlayersInMatch(id, idPlrList: Plr2Match) { // metoda usuwa podana z selectow liste zawodnikow
    return this.http.request('delete', 'https://localhost:44356/Match/DeletePlayersInMatch/' + id ,
     { body: idPlrList, observe: 'response' });
     // lista jest zapakowana do body bo w agularze delete nie przyjmuje bezposrednio argumentu body
  }

  getPlayersOutMatch(id: number): Observable<Array<Player>> { // metoda pobierajaca liste wszystkich meczow
    return this.http.get<Array<Player>>('https://localhost:44356/Match/' + this.userId + '/GetPlayersOutMatch/' + id  );
  }

  addMatch(match: Match) { // dodanie meczu do bazy
    return this.http.post('https://localhost:44356/Match/PostMatch/' + this.userId, match, {  observe: 'response' });
  }

  addPlayersToMatch(id: number, plrIdList: Plr2Match) {
    return this.http.post('https://localhost:44356/Match/AddPlayersToMatch/' + id , plrIdList, {  observe: 'response' });
      // metoda pobierajaca jeden mecz
  }

  deleteMatch(id: number) {  // usuwanie meczu
    return this.http.delete('https://localhost:44356/Match/DeleteMatch/' + id, { observe: 'response' }); //  responseType: 'text',
  }

  updateMatch(match: Match) {  // edycja zawodnika
    return this.http.put('https://localhost:44356/Match/PutMatch/' + match.id, match, { observe: 'response' });
    // przekazujemy adres zawodnika, do linku doklejamy po id ktore wyciagamy z danych zawodnika i dane zawodnika
  }

  getNearestMatch(): Observable<Array<Match>> {  // pobiera najblizszy mecz lub mecze jezeli jest ich wiecej w jednym czasie
    return this.http.get<Array<Match>>('https://localhost:44356/Match/GetNearestMatch/' + this.userId);
    // metoda pobierajaca najblizszy lub najblizsze mecz
  }
  // tslint:disable-next-line:max-line-length
  getPlrsStatsInMatch(id: number): Observable<Array<playersStatisticInMatch>> {  // pobiera liste zawodnikow i ich statystyke w konkretnym meczu
    return this.http.get<Array<playersStatisticInMatch>>('https://localhost:44356/Match/GetPlayersStatisticInMatch/' + id );
    // metoda pobierajaca najblizszy lub najblizsze mecz
  }

  // tslint:disable-next-line:max-line-length
  updateStatsPlayer(id: number, listPlrs: UpdateStatisticPlayersInMatch) {  // pobiera liste zawodnikow i ich nowych statystyk konkretnego meczu
    return this.http.put('https://localhost:44356/Match/UpdateStatisticPlayersInMatch/' + id , listPlrs, { observe: 'response' });

  }

  getTimesToNearstmatch(): Observable<Array<number>> {
    return this.http.get<Array<number>>('https://localhost:44356/Match/GetTimeToMatch/' + this.userId);
  }


}

