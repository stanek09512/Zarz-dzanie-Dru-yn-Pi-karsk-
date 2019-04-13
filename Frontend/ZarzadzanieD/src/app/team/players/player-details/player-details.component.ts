import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs/internal/Observable';
import { Player } from 'src/app/Domain/Player';
import { AlertifyServicesService } from 'src/app/mainServices/alertify-services.service';
import { HttpPlayerService } from 'src/app/mainServices/http-player.service';





@Component({
  selector: 'app-player-details',
  templateUrl: './player-details.component.html',
  styleUrls: ['./player-details.component.css']
})
export class PlayerDetailsComponent implements OnInit {

  Plr$: Observable<Player>;
  Plr: Player;


  imie: string;
  nazwisko: string;
  wiek: number;
  id;

 aa = 'Obronca';

  buttonSaveVisible: boolean;
  ButtonSave = false; // okresla czy guzik zapisu jest mozliwy do uzycia
  InfoShow = false;
  contactForm: FormGroup;

  positions: string[];

  constructor(private httpPlayerService: HttpPlayerService, private route: ActivatedRoute,
    private router: Router, private alertifyService: AlertifyServicesService) {

  }

  ngOnInit() {
    this.positions = [
      'Bramkarz',
      'Obronca',
      'Pomocnik',
      'Napastnik'
    ];


    this.id = this.route.snapshot.paramMap.get('id');  // pobiera parametr z adresu strony

    this.httpPlayerService.getPlayer(this.id).subscribe(x => {
      this.Plr = x;
      this.SetValues(); // ustawiam poczatkowe wartosci dla inputow
    });
   // console.log(this.id);

   this.contactForm = new FormGroup( {  // lacze pola formularzem aby je zablokowywac i odblokowywac
    name: new FormControl(), // tu w nawiasie jest ustawiona wartosc poczatkowa i validacja na pole
    surname: new FormControl(  Validators.required),
    age: new FormControl( Validators.required ),
    position: new FormControl (Validators.required),
    positionControl: new FormControl()
  });
  this.setDisableFields();  // blokuje inputy

  this.buttonSaveVisible = false; // ustawiam button zapisu na widoczny na poczatku bo pola sa wypelnionena poczatku
  }




  setDisableFields() {  // blokuje wszystkie inputy
    this.contactForm.get('name').disable({onlySelf: true});
    this.contactForm.get('surname').disable({onlySelf: true});
    this.contactForm.get('age').disable({onlySelf: true});
    this.contactForm.get('positionControl').disable({onlySelf: true});
    this.buttonSaveVisible = false;
  }

  setEndableFields() {  // odblokowuje wszystkie inputy
    this.contactForm.get('name').enable({onlySelf: true});
    this.contactForm.controls['name'].setValidators([Validators.required]);
    this.contactForm.get('surname').enable({onlySelf: true});
    this.contactForm.get('age').enable({onlySelf: true});
    this.contactForm.get('positionControl').enable({onlySelf: true});
    this.buttonSaveVisible = true;
  }

  SetValues() { // ustawia wartosci do wywolania w widoku HTML
    this.imie = this.Plr.name;
    this.nazwisko = this.Plr.surname;
    this.wiek = this.Plr.age;
    this.contactForm.get('positionControl').setValue( this.Plr.position);
   console.log(this.contactForm);
  }

  BackToParentPage() {
    this.router.navigate(['home/players']); // przerzuca na ten adres
  }

  DeletePlayer() {  // usuwam zawodnika
    this.httpPlayerService.deletePlayer(this.Plr.id).subscribe( p => {
      console.log(p);
      this.InfoShow = true; // pokazuje komunikat
      this.alertifyService.success('Operacja powiodła się, usunąłes zawodnika');
    },
    (error: HttpErrorResponse) => {
      console.log(error.status);
      this.alertifyService.error('Operacja nie powiodła się, sprobuj ponownie.');
    });
    this.router.navigate(['home/players']); // przerzuca na ten adres

  }

  EditPlayer() {  // obsluga guzika 'edytuj zawodnika'
    this.setEndableFields();  // odblokowuje inputy
  }

  SaveChanges() { // obiekt skladam z id z parametru i pol tu z pliku ts poniewaz uzywam ngmodel-komunikacji w dwie strony
      const PlrObj: Player = ({
      id: this.Plr.id,
      name:  this.imie,
      surname: this.nazwisko,
      position:  this.contactForm.get('positionControl').value,  // pobiera wartosc z pola
      age: this.wiek
     });
     console.log(PlrObj);
     this.httpPlayerService.updatePlayer(PlrObj).subscribe( p => {  // wysylam zapytanie z edycja
      console.log(p);
      this.InfoShow = true; // pokazuje komunikat
    },
    (error: HttpErrorResponse) => {
      console.log(error.status);
    });


    this.setDisableFields(); // wylacza pola
    this.buttonSaveVisible = false; // i ustawia guzik zapisu na niewidoczny
    setTimeout(function() {
      this.InfoShow = false;  // czyli po 3 sec ponownie ustawia na niewidoczny komunikat
    }.bind(this), 3000);  // tu po 3 sec uruchamia metode w setFunction
    }

    DisableSaveButton(event) {  // metoda jest wywolywana po kazdej zmianie w inputach
      if (this.imie === '' || this.nazwisko  === '' || this.wiek === null) {  // jezeli ktorys imput jest pusty
        this.ButtonSave = true; // to zablokuj button
      } else {  // w przeciwnym wypadku odblokowany ma byc
        this.ButtonSave = false;
      }
    }


}
