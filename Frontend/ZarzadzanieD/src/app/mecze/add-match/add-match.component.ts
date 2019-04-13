import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpPlayerService } from 'src/app/mainServices/http-player.service';
import { Observable } from 'rxjs/internal/Observable';
import { HttpMatchService } from 'src/app/mainServices/http-match.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { Player } from 'src/app/Domain/Player';
import { Plr2Match } from 'src/app/Domain/PlayersInMatch';
import { Match } from 'src/app/Domain/Match';


@Component({
  selector: 'app-add-match',
  templateUrl: './add-match.component.html',
  styleUrls: ['./add-match.component.css']
})
export class AddMatchComponent implements OnInit {

  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  thirdFormGroup: FormGroup;
  fourthFormGroup: FormGroup;
// pole nameOpponent
  nameOpponentFieldFlag = true; // ustawia flage na dostepnosc buttonu
  nameOpponentField = null; // a flage ustawia na podstawie tego ze metoda sprawdza czy jest jakas wartosc w polu HTML i tu napisana

  // pole dateMatch
  dateMatchFieldFlag = true; // ustawia flage na dostepnosc buttonu
  dateMatchField = null;

   // pole place
   placeFieldFlag = true; // ustawia flage na dostepnosc buttonu
   placeField = null;

   visibleAlertDoMatch = false;  // ustawia na widoczny komunikat ze udalo sie stworzyc mecz i mozna dodac zawodnikow/ flaga

  playerList$: Observable<Array<Player>>;
  playerList: Array<Player>;
  selectedOptions = [];
  selectedOption;
  idMatch = null; // id meczu ktory dodalismy. jezeli jest dodany to to pole sie wypelnia i w html pokazuje liste zawodnikow do dodania

  constructor(private _formBuilder: FormBuilder, private httpPlayerService: HttpPlayerService,
     private httpMatchService: HttpMatchService, private router: Router) {}


  ngOnInit() {

    this.firstFormGroup = this._formBuilder.group({ // pola kazdego przejscia formularza
      nameOpponent: ['', Validators.required]
    });
    this.secondFormGroup = this._formBuilder.group({
      dateMatch: ['', Validators.required]
    });
    this.thirdFormGroup = this._formBuilder.group({
      place: ['', Validators.required]
    });

    this.playerList$ = this.httpPlayerService.getPlayers(); // pobiera na wejsciu liste wszystkich zawodnikow
    this.playerList$.subscribe( plr => {
      this.playerList = plr;
    });
  }

  getValueNameOpponentfield(event) {  // pobiera wartosc z inputu nameOpponent
    this.nameOpponentField = event.target.value;
    if ( this.nameOpponentField !== null) {
      this.nameOpponentFieldFlag = false; // ustawia flage
    }
    if ( this.nameOpponentField === null || this.nameOpponentField === '') {
      this.nameOpponentFieldFlag = true;
    }
  }

  getValueDateMatchfield(event) { // pobiera wartosc z inputu dateMatch
    this.dateMatchField = event.target.value;
    if ( this.dateMatchField !== null && this.nameOpponentFieldFlag === false) {
      this.dateMatchFieldFlag = false;
    }
    if ( this.dateMatchField === null ) {
      this.dateMatchFieldFlag = true;
    }
  }

  getValuePlacefield(event) {  // pobiera wartosc z inputu place
    this.placeField = event.target.value;
    if ( this.placeField !== null && this.dateMatchFieldFlag === false && this.nameOpponentFieldFlag === false) {
      this.placeFieldFlag = false; // ustawia flage
    }
    if ( this.placeField === null || this.placeField === '') {
      this.placeFieldFlag = true;
    }
  }

  onNgModelChange($event) { // metoda nadpisuje za kazda zmiana liste zaznaczonych zawodnikow z listy select
    console.log('event:' + $event);
    this.selectedOption = $event;
    console.log('wsio ' + this.selectedOption);

  }
  SaveMatch() {
    const match: Match = ({  // przypisuje wartosci na obiekt  z HTML
      id: null,
      opponentTeam: this.firstFormGroup.value.nameOpponent,
      matchDate: this.secondFormGroup.value.dateMatch,  // 2020-06-11T00:00:00
      place: this.thirdFormGroup.value.place,
     });
     console.log(match);
     if (match.opponentTeam !== null && match.matchDate !== null && match.place !== null) {
      this.httpMatchService.addMatch(match).subscribe(p => { // wykonuje zapytanie http, dodaje mecz do bazy i zwraca jego id
        console.log(p); // wyswietla wynik tego dzialania
        // this.idMatch = p.body;
        // this.reloadComponent();
        this.visibleAlertDoMatch = true; // pokazuje komunikat
        setTimeout(function() {
          this.visibleAlertDoMatch = false;  // czyli po 3 sec ponownie ustawia na niewidoczny komunikat
        }.bind(this), 4000);  // tu po 3 sec uruchamia metode w setFunction
        this.zmienId(p.body); // metoda zapisuje id ze zwrotki dodania meczu do prywatnego pola w komponencie
       },
       (error: HttpErrorResponse) => {  // lub wyswietla ewentualne bledy
         console.log(error.status);
       });

     }


  }

  zmienId(elem) {
    this.idMatch = elem;
    console.log('Pozdro z metody ' + this.idMatch);
  }

  AddPlayersToMatch() {
    console.log('przed ' + this.idMatch);
    console.log('przed ' + this.selectedOption);
    const plrsList2Match: Plr2Match = ({  // przypisuje wartosci na obiekt
      playerListId: this.selectedOption,
     });
     console.log(plrsList2Match);
      this.httpMatchService.addPlayersToMatch(this.idMatch, plrsList2Match).subscribe( x => {
      console.log(x);
    },
      (error: HttpErrorResponse) => {  // lub wyswietla ewentualne bledy
        console.log(error.status);
      });
  }


  resetIdMatch() {  // resetuje id meczu- ponownie ustawia je na null
    this.idMatch = null;
    this.dateMatchFieldFlag = true;
    this.placeFieldFlag = true;
    this.nameOpponentFieldFlag = true;
  }

  reloadComponent() {
    this.router.navigateByUrl('/Calendar', {skipLocationChange: true}).then(() => // najpierw przeskakuje na ten komponent
    this.router.navigate(['home/AddMatch']));  // a tu wraca na poprzedni komponent
  }


}




