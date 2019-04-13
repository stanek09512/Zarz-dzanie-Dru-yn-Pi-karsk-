import { Component, OnInit } from '@angular/core';
import { HttpMatchService } from '../mainServices/http-match.service';
import { Observable } from 'rxjs/internal/Observable';

import { Router } from '@angular/router';
import { Match } from '../Domain/Match';


@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent implements OnInit {

  nearestsMatches$: Observable<Array<Match>>;
  // nearestsMatches: Array<Match>;

  nearestMatchExist = true;

  constructor(private httpMatchService: HttpMatchService, private router: Router ) {
   }

  ngOnInit() {
    this.nearestsMatches$ = this.httpMatchService.getNearestMatch();
    // i jak chcemy wyswietlic bezposrednio to w forze w HTML musimy dodac ASYNC pipe
    if ( this.nearestsMatches$ === null) {
        this.nearestMatchExist = false;
        console.log(this.nearestMatchExist);
    }
    console.log(this.nearestMatchExist);
    console.log(this.nearestsMatches$);
  }


  OpenMatch(id) {
    this.router.navigate(['home/Calendar', id]);
  }
}
