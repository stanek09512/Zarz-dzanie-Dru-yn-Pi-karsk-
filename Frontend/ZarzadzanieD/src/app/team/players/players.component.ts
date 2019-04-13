import { Component, OnInit} from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import {Router } from '@angular/router';
import { HttpPlayerService } from 'src/app/mainServices/http-player.service';
import { Player } from 'src/app/Domain/Player';




@Component({
  selector: 'app-players',
  templateUrl: './players.component.html',
  styleUrls: ['./players.component.css']
})
export class PlayersComponent implements OnInit {

  allPlayers$: Observable<Array<Player>>; // Obs widoczna lista z poziomu html
  listaZawodnikow: Player[] = []; //

  WyszukiwarkaNazwisko = ''; // tu przetrzymuje wartosc z input w wyszukiwarce two way data binding


  constructor(private httpPlayerService: HttpPlayerService, private route: Router) {
  }

  ngOnInit() {
    this.getPlayers();  // zaczytuje lliste zawodnikow z serwera i od razu przeklada ja na liste obiektow zebym mogl sie do nich dostac
  }



  Wyszukiwarka(event) { //  przypisuje wartosc z inputu do naszego pola a pozniej po nim przeszukuje listę
   this.WyszukiwarkaNazwisko = event.target.value;
    // this._serachSurname = event.target.value;
  }

  getPlayers() {
    this.allPlayers$ = this.httpPlayerService.getPlayers();
    this.allPlayers$.subscribe( elem => { // to jest tylko po to zeby pierwszy raz przeladowalo liste
      this.listaZawodnikow = elem; // pobieram liste i nakladam na prywatna tu w komponencie
     });
  }



 redirectToPlrDetails(id: number) { // przeadresuj na players/:id po stronie HTML juz przekazujemy indywidualne id zawodnika
  this.route.navigate(['home/players', id]);  // przeadresowuje do detale
  }
 }



