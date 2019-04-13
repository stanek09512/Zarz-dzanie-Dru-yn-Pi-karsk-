import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AlertifyServicesService } from './../../../mainServices/alertify-services.service';
import { HttpMatchService } from './../../../mainServices/http-match.service';
import { Plr2Match } from 'src/app/Domain/PlayersInMatch';
import { Match } from 'src/app/Domain/Match';


@Component({
  selector: 'app-match-details',
  templateUrl: './match-details.component.html',
  styleUrls: ['./match-details.component.css']
})
export class MatchDetailsComponent implements OnInit {


  contactForm: FormGroup;
  nazwaPrzeciwnika;
  DataMeczu;
  miejsceMeczu;
  match: Match; // pobrany mecz po id do wyswietlenia na starcie zakladki

  id; // id meczu pobrane z parametru



  // widocznosc
  ButtonSave = false; // okresla czy guzik zapisu jest mozliwy do uzycia czy nie jest szary
  buttonSaveVisible: boolean; // decyduje czy guzik jest w ogole widoczny
  InfoShow = false; // widocznosc komunikatu po usunieciu


  // dodawanie kolejnych zawodnikow
  selectedOptionsOutMatch = [];
  selectedOptionOutMatch;
  playersOutMatch$: Observable<Array<any>>;


  // zawodnicy juz w meczu i do usuniecia
  selectedOptionsInMatch = [];
  selectedOptionInMatch;
  playersInMatch$: Observable<Array<any>>;  // observable player dobiera ale jest any bo wyskakuja bledy
  // nie wiem dlaczego nie chce przerzucic z tego samego typou na takis am typ



  constructor(private httpMatchService: HttpMatchService, private route: ActivatedRoute,
    private router: Router, private alertifyService: AlertifyServicesService) { }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');  // pobiera parametr z adresu strony
    this.httpMatchService.getMatch(this.id).subscribe(x => {
      this.match = x;
      this.SetValues(); // ustawiam poczatkowe wartosci dla inputow
    });

    this.getPlayersInMatch(this.id);  // pobiera zawodnikow przypisanych do tego meczu
    this.getPlayersOutMatch(this.id); // pobieranie zawodnikow nie przypisanych do meczu


    this.contactForm = new FormGroup( {  // lacze pola formularzem aby je zablokowywac i odblokowywac
      nameOpponent: new FormControl(), // tu w nawiasie jest ustawiona wartosc poczatkowa i validacja na pole
    dateMatch: new FormControl(  Validators.required),
    placeMatch: new FormControl( Validators.required ),
    scoreFirstTeam: new FormControl (  Validators.required),
    scoreSecondTeam: new FormControl (  Validators.required)
  });

  this.setDisableFields();  // blokuje inputy
  }

  getPlayersInMatch(id: number) {
    this.playersInMatch$ = this.httpMatchService.getPlayersInMatch(id);
  }
  getPlayersOutMatch(id: number) {
    this.playersOutMatch$ = this.httpMatchService.getPlayersOutMatch(id);
  }

  DeletePlayersFromMatch() {  // usuwanie zawodnikow z meczu
    const plrsList2Match: Plr2Match = ({  // przypisuje wartosci na obiekt
      playerListId: this.selectedOptionInMatch,
     });
     console.log(plrsList2Match);
    this.httpMatchService.DeletePlayersInMatch(this.id, plrsList2Match).subscribe( x => {
      // usuwa liste podanych zaznaczonych zawodniow
      console.log(x);
      this.alertifyService.success('Operacja usuwania zawodnikow z kadry powiodła się.');
    },
      (error: HttpErrorResponse) => {  // lub wyswietla ewentualne bledy
        console.log(error.status);
        this.alertifyService.error('Operacja usuwania zawodnikow z kadry nie powiodła się.');
      });

  }

  onNgModelChangeOut($event) { // metoda nadpisuje za kazda zmiana liste zaznaczonych zawodnikow z listy select
    console.log('event:' + $event);
    this.selectedOptionOutMatch = $event;
    console.log('wsio ' + this.selectedOptionOutMatch);

  }

  onNgModelChangeIn($event) { // metoda nadpisuje za kazda zmiana liste zaznaczonych zawodnikow z listy select
    console.log('event:' + $event);
    this.selectedOptionInMatch = $event;
    console.log('wsio ' + this.selectedOptionInMatch);

  }

  AddPlayersToMatch() { // obsluguje dodanie zawodnikow do meczu

    const plrsList2Match: Plr2Match = ({  // przypisuje wartosci na obiekt
      playerListId: this.selectedOptionOutMatch,
     });
     console.log(plrsList2Match);
      this.httpMatchService.addPlayersToMatch(this.id, plrsList2Match).subscribe( x => {
      console.log(x);
      this.alertifyService.success('Operacja dodawania zawodnikow do meczu powiodła się.');
    },
      (error: HttpErrorResponse) => {  // lub wyswietla ewentualne bledy
        console.log(error.status);
        this.alertifyService.error('Operacja usuwania zawodnikow z meczu nie powiodła się.');
      });

      this.reloadComponent();

  }



  DisableSaveButton(event) {  // metoda jest wywolywana po kazdej zmianie w inputach  i decyduje czy dostepny jest guzik zapisu
    if (this.nazwaPrzeciwnika === '' || this.DataMeczu  === '' || this.miejsceMeczu === '' ) {  // jezeli ktorys imput jest pusty
      this.ButtonSave = true; // to zablokuj button
    } else {  // w przeciwnym wypadku odblokowany ma byc
      this.ButtonSave = false;
    }
  }

  BackToParentPage() {
    this.router.navigate(['home/Calendar']); // przerzuca na ten adres
  }

  setEndableFields() {  // odblokowuje wszystkie inputy
    this.contactForm.get('nameOpponent').enable({onlySelf: true});
    this.contactForm.controls['nameOpponent'].setValidators([Validators.required]);
    this.contactForm.get('dateMatch').enable({onlySelf: true});
    this.contactForm.get('placeMatch').enable({onlySelf: true});
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
   console.log(this.contactForm);
  }

  EditMatch() {  // obsluga guzika 'edytuj zawodnika'
  this.setEndableFields();  // odblokowuje inputy
}

DeleteMatch() {  // usuwam zawodnika
  this.httpMatchService.deleteMatch(this.match.id).subscribe( p => {
    console.log(p);
    this.alertifyService.success('Operacja powiodłą się, usunąłes mecz.');
  },
  (error: HttpErrorResponse) => {
    console.log(error.status);
    this.alertifyService.error('Operacja nie powiodłą się, sprbuj ponownie');
  });
  this.router.navigate(['home/Calendar']); // przerzuca na ten adres
}

SaveChanges() { // obiekt skladam z id z parametru i pol tu z pliku ts poniewaz uzywam ngmodel-komunikacji w dwie strony

    const matchObj: Match = ({
    id: this.id,
    opponentTeam: this.nazwaPrzeciwnika,
    matchDate: this.DataMeczu,
    place: this.miejsceMeczu,
   });
   this.insideUpdateMethod(matchObj);

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

  reloadComponent() {
    this.router.navigateByUrl('/AddMatch', {skipLocationChange: true}).then(() => // najpierw przeskakuje na ten komponent
    this.router.navigate(['home/Calendar', this.id]));  // a tu wraca na poprzedni komponent
  }

}

