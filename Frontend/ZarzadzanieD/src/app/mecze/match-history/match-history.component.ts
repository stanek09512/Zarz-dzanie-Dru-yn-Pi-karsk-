import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';

import { HttpMatchService } from 'src/app/mainServices/http-match.service';
import { Router } from '@angular/router';
import { Match } from 'src/app/Domain/Match';


@Component({
  selector: 'app-match-history',
  templateUrl: './match-history.component.html',
  styleUrls: ['./match-history.component.css']
})
export class MatchHistoryComponent implements OnInit {

  allMatchesPast$: Observable<Array<Match>>;  // pobrane mecze


  WyszukiwarkaPrzeciwnika = '';

  constructor( private httpMatchService: HttpMatchService,  private route: Router ) { }

  ngOnInit() {
    this.allMatchesPast$ = this.httpMatchService.getMatchesInPast();
  }

  redirectToMatchDetails(id: number) { // przeadresuj na players/:id po stronie HTML juz przekazujemy indywidualne id zawodnika
    this.route.navigate(['home/matchHistory', id]);  // przeadresowuje do detale

    }

}
