import { AuthService } from './auth.service';
import { Observable } from 'rxjs/internal/Observable';
import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { Player } from '../Domain/Player';
import { playersStatisticInMatch } from '../Domain/PlayersStatistics';



@Injectable({
  providedIn: 'root'
})

export class HttpPlayerService {

  constructor(private http: HttpClient, private authService: AuthService) {

  }

  userId = this.authService.decodedToken.nameid;

  getPlayers(): Observable<Array<Player>> { // metoda pobierajaca liste wszystkich zawodnikow
    console.log('user nr ' + this.userId);
    return this.http.get<Array<Player>>('https://localhost:44356/Players/GetAllPlayers/' + this.userId);

  }

  getPlayer(id): Observable<Player> {
    return this.http.get<Player>('https://localhost:44356/Players/getPlayer/' + id);  // metoda pobierajaca jednego zawodnika
  }

  addPlayer(player: Player) { // dodanie zawodnika do bazy
    return this.http.post('https://localhost:44356/Players/PostPlayer/' + this.userId, player, {  observe: 'response' });
  }

  updatePlayer(player: Player) {  // edycja zawodnika
    return this.http.put('https://localhost:44356/Players/PutPlayer/' + player.id, player, { observe: 'response' });
    // przekazujemy adres zawodnika, do linku doklejamy po id ktore wyciagamy z danych zawodnika i dane zawodnika
  }

  deletePlayer(id: number) {  // usuwanie zawodnika
    return this.http.delete('https://localhost:44356/Players/DeletePlayer/' + id, { observe: 'response' }); //  responseType: 'text',
  }

  getPlayersStatsList(): Observable<Array<playersStatisticInMatch>> { // metoda pobierajaca liste statystiki wszystkich zawodnikow
    console.log('pobieram statystyki wszystkich zawodnikow');
    return this.http.get<Array<playersStatisticInMatch>>('https://localhost:44356/Players/GetAllPlayersStatistics/' + this.userId);
  }

}
