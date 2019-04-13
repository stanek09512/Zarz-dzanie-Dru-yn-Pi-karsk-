import { HttpMatchService } from './../../mainServices/http-match.service';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { Router } from '@angular/router';
import { Match } from 'src/app/Domain/Match';

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.css']
})
export class CalendarComponent implements OnInit {

  allMatches$: Observable<Array<Match>>; // Obs widoczna lista z poziomu html

  WyszukiwarkaPrzeciwnika = ''; // tu przetrzymuje wartosc z input w wyszukiwarce two way data binding


  constructor(private httpMatchService: HttpMatchService, private route: Router) { }

  ngOnInit() {
    this.getMatches();  // wywoluje te dzialania przy przejsciu na wybrany adres
  }

  getMatches() {
    this.allMatches$ = this.httpMatchService.getMatchesInFeature();  // pobiera przez serwis mecze przypisujedo naszej zmiennej
    this.allMatches$.subscribe( x => {  // tu wypisuje na konsole wszystko
      console.log(x);
    });
  }

  redirectToMatchDetails(id: number) { // przeadresuj na players/:id po stronie HTML juz przekazujemy indywidualne id zawodnika
    this.route.navigate(['home/Calendar', id]);  // przeadresowuje do detale

    }

}

