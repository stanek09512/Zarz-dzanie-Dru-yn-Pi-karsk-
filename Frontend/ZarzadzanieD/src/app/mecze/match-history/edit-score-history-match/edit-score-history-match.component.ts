import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs/internal/Observable';
import { HttpMatchService } from 'src/app/mainServices/http-match.service';
import { AlertifyServicesService } from 'src/app/mainServices/alertify-services.service';
import { Match } from 'src/app/Domain/Match';
import { UpdateStatisticPlayersInMatch, playersStatisticInMatch } from 'src/app/Domain/PlayersStatistics';

@Component({
  selector: 'app-edit-score-history-match',
  templateUrl: './edit-score-history-match.component.html',
  styleUrls: ['./edit-score-history-match.component.css']
})
export class EditScoreHistoryMatchComponent implements OnInit {

  allPlrsStatsInMatch$: Observable<Array<playersStatisticInMatch>>;  // pobrani zawodnicy i ich statystyki w tym meczu

  contactForm: FormGroup;
  // pola w HTML głwne
  nazwaPrzeciwnika;
  DataMeczu;
  miejsceMeczu;
  wynikPierwszejDruzyny;
  wynkDrugiejDruzyny;

  // pola w HTML w tabeli statystyk
  gole: number;
  asysty: number;
  zolteKartki: number;
  czerwoneKartki: number;
  CzyGral;

  positions: string[];

  id; // id meczu pobrane z parametru
  match: Match; // pobrany mecz po id do wyswietlenia na starcie zakladki


  // widocznosc
  ButtonSave = false; // okresla czy guzik zapisu jest mozliwy do uzycia czy nie jest szary
  buttonSaveVisible: boolean; // decyduje czy guzik jest w ogole widoczny
  InfoShow = false; // widocznosc komunikatu po usunieciu




  playersStat: playersStatisticInMatch[] = []; // lista obiektow/ zaowdnikow ze statystyka przypisanych do meczu
  plrStats: Array<playersStatisticInMatch> = []; // lista do ktorej wrzucam wybrany obiekt zeby go wyslac do edycji uzyte w SaveChangePlr()


  constructor(private httpMatchService: HttpMatchService, private route: ActivatedRoute,
    private router: Router, private alertifyService: AlertifyServicesService) { }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');  // pobiera parametr z adresu strony
    this.httpMatchService.getMatch(this.id).subscribe(x => {
      this.match = x;

      this.SetValues(); // ustawiam poczatkowe wartosci dla inputow
    });

    this.allPlrsStatsInMatch$ = this.httpMatchService.getPlrsStatsInMatch(this.id);
    this.allPlrsStatsInMatch$.subscribe(x => {
      this.rewriteToObjectList(x);  // przepisuje obiekty do mojej prywatnej listy zebym mogl na nich robic operacje
    });

    this.contactForm = new FormGroup( {  // lacze pola formularzem aby je zablokowywac i odblokowywac
    nameOpponent: new FormControl(), // tu w nawiasie jest ustawiona wartosc poczatkowa i validacja na pole
    dateMatch: new FormControl(  Validators.required),
    placeMatch: new FormControl( Validators.required ),
    scoreFirstTeam: new FormControl (  Validators.required),
    scoreSecondTeam: new FormControl (  Validators.required)
  });
  this.setDisableFields();  // blokuje inputy

  this.positions = [
    'true',
    'false'
  ];


  }

  rewriteToObjectList(x) {
    this.playersStat = x;
  }

  DisableSaveButton(event) {  // metoda jest wywolywana po kazdej zmianie w inputach  i decyduje czy dostepny jest guzik zapisu
    if (this.wynikPierwszejDruzyny === null || this.wynkDrugiejDruzyny  === null ) {  // jezeli ktorys imput jest pusty
      this.ButtonSave = true; // to zablokuj button
    } else {  // w przeciwnym wypadku odblokowany ma byc
      this.ButtonSave = false;
    }
  }

  BackToParentPage() {
    this.router.navigate(['home/matchHistory']); // przerzuca na ten adres
  }

  setEndableFields() {  // odblokowuje wszystkie inputy
    this.contactForm.get('scoreFirstTeam').enable({onlySelf: true});
    this.contactForm.get('scoreSecondTeam').enable({onlySelf: true});
    this.buttonSaveVisible = true;
  }

  setDisableFields() {  // blokuje wszystkie inputy
    this.contactForm.get('nameOpponent').disable({onlySelf: true});
    this.contactForm.get('dateMatch').disable({onlySelf: true});
    this.contactForm.get('placeMatch').disable({onlySelf: true});
    this.contactForm.get('scoreFirstTeam').disable({onlySelf: true});
    this.contactForm.get('scoreSecondTeam').disable({onlySelf: true});
    this.buttonSaveVisible = false;
  }

  SetValues() { // ustawia wartosci do wywolania w widoku HTML
    this.nazwaPrzeciwnika = this.match.opponentTeam;
    this.DataMeczu = this.match.matchDate;
    this.miejsceMeczu = this.match.place;
    this.wynikPierwszejDruzyny = this.match.scoreFirstTeam;
    this.wynkDrugiejDruzyny = this.match.scoreSecondTeam;
   console.log(this.contactForm);
  }

  EditMatch() {  // obsluga guzika 'edytuj zawodnika'
  this.setEndableFields();  // odblokowuje inputy
}

DeleteMatch() {  // usuwam zawodnika
  this.httpMatchService.deleteMatch(this.match.id).subscribe( p => {
    console.log(p);
    this.alertifyService.success('Operacja powiodła się, usunąłes mecz.');
  },
  (error: HttpErrorResponse) => {
    console.log(error.status);
  });
  this.alertifyService.error('Operacja nie powiodła się');
  this.router.navigate(['home/matchHistory']); // przerzuca na ten adres
  this.reloadComponent();
}

reloadComponent() {
  this.router.navigateByUrl('home/Calendar', {skipLocationChange: true}).then(() => // najpierw przeskakuje na ten komponent
  this.router.navigate(['home/matchHistory']));  // a tu wraca na poprzedni komponent
}

SaveChanges() { // obiekt skladam z id z parametru i pol tu z pliku ts poniewaz uzywam ngmodel-komunikacji w dwie strony

      if (this.wynikPierwszejDruzyny !== null && this.wynkDrugiejDruzyny !== null) {
        const matchObj: Match = ({
        id: this.id,
        opponentTeam: this.nazwaPrzeciwnika,
        matchDate: this.DataMeczu,
        place: this.miejsceMeczu,
        scoreFirstTeam: this.wynikPierwszejDruzyny,
        scoreSecondTeam: this.wynkDrugiejDruzyny
        });
        console.log(matchObj);
        this.insideUpdateMethod(matchObj);
        }
      if (this.wynkDrugiejDruzyny !== null) {
        const matchObj: Match = ({
        id: this.id,
        opponentTeam: this.nazwaPrzeciwnika,
        matchDate: this.DataMeczu,
        place: this.miejsceMeczu,
        scoreSecondTeam: this.wynkDrugiejDruzyny
        });
        this.insideUpdateMethod(matchObj);
        }
      if (this.wynikPierwszejDruzyny !== null) {
        const matchObj: Match = ({
        id: this.id,
        opponentTeam: this.nazwaPrzeciwnika,
        matchDate: this.DataMeczu,
        place: this.miejsceMeczu,
        scoreFirstTeam: this.wynikPierwszejDruzyny,
        });
        this.insideUpdateMethod(matchObj);
        }
    this.InfoShow = true;
    this.setDisableFields(); // wylacza pola
    this.buttonSaveVisible = false; // i ustawia guzik zapisu na niewidoczny
    setTimeout(function() {
      this.InfoShow = false;  // czyli po 3 sec ponownie ustawia na niewidoczny komunikat
    }.bind(this), 3000);  // tu po 3 sec uruchamia metode w setFunction

  }


  insideUpdateMethod(matchObj: Match) {
    console.log(matchObj);
    // wewnetrzna metoda edycji zrobiana ze wzgledu ze trzeba deklarowac obiekt inny jezeli odpowiednie pole jest wypelnione lub nie
    this.httpMatchService.updateMatch(matchObj).subscribe( p => {  // wysylam zapytanie z edycja
      console.log(p);
      this.InfoShow = true; // pokazuje komunikat
    },
    (error: HttpErrorResponse) => {
      console.log(error.status);
    });
  }

  redirectToMatchDetails( idPlr) {
    console.log(idPlr);
    // przeadresuj na players/:id po stronie HTML juz przekazujemy indywidualne id zawodnika
    this.router.navigate(['home/matchHistory', this.id, 'home/editPlrStat/', idPlr]);  // przeadresowuje do detale

    }

    SaveChangePlr(plr: playersStatisticInMatch) {
      this.plrStats.push(plr);  // dorzucam obiekt do listy
      console.log(plr);
      const list: UpdateStatisticPlayersInMatch = ({
        playersStatisticInMatch: this.plrStats // tu ta liste wrzucam do obiektu
      });
      console.log('lista1: ' + list.playersStatisticInMatch);
      // a ponizej juz wysylam ten obiekt z lista
      this.httpMatchService.updateStatsPlayer(this.id, list).subscribe(x => {
        console.log('lista2: ' + list);
      },
        (error: HttpErrorResponse) => {
          console.log(error.status);
      });
    }
}


