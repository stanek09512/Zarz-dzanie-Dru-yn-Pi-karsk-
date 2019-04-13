
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

import { HttpErrorResponse } from '@angular/common/http';
import { Player } from 'src/app/Domain/Player';
import { HttpPlayerService } from 'src/app/mainServices/http-player.service';


@Component({
  selector: 'app-add-player',
  templateUrl: './add-player.component.html',
  styleUrls: ['./add-player.component.css']
})
export class AddPlayerComponent implements OnInit {


  contactForm: FormGroup;
  positions: string[];
  InfoShow = false;

  constructor(private httpPlayerService: HttpPlayerService) { }

  ngOnInit() {
    this.positions = [
      'Bramkarz',
      'Obronca',
      'Pomocnik',
      'Napastnik'
    ];
    this.contactForm = new FormGroup( {
      name: new FormControl(null, Validators.required), // tu w nawiasie jest ustawiona wartosc poczatkowa i validacja na pole
      surname: new FormControl(null, Validators.required),
      age: new FormControl(null, Validators.required),
      selectPosition: new FormControl ( this.positions[0] , Validators.required)

    });
  }



  onSubmit() {  // metoda
    // console.log(this.contactForm);  // wypisuje na konsole obiekt z naszymi polami
    const plr: Player = ({  // przypisuje wartosci na obiekt  z HTML
      id: null,
      name: this.contactForm.value.name,
      surname: this.contactForm.value.surname,
      position: this.contactForm.value.selectPosition,
      age: this.contactForm.value.age
     });
      console.log(plr);
     if (plr.name !== null && plr.surname !== null && plr.age !== null) {
      this.httpPlayerService.addPlayer(plr).subscribe(p => { // wykonuje zapytanie http, dodaje zawodnika do bazy
        console.log(p); // wyswietla wynik tego dzialania

        this.InfoShow = true; // pokazuje komunikat
        setTimeout(function() {
          this.InfoShow = false;  // czyli po 3 sec ponownie ustawia na niewidoczny komunikat
        }.bind(this), 3000);  // tu po 3 sec uruchamia metode w setFunction
       },
       (error: HttpErrorResponse) => {  // lub wyswietla ewentualne bledy
         console.log(error.status);
       });
       this.onReset();  // a jak spelni swoje zalozenia to czysci pola przez wywolanie metody
     }

  }

  onReset() { // czysci pola
    this.contactForm.patchValue({
      name: null,
      surname: null,
      position: null,
      age: null
    });
  }
}
