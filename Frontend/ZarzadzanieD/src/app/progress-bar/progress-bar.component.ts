import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { HttpMatchService } from '../mainServices/http-match.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-progress-bar',
  templateUrl: './progress-bar.component.html',
  styleUrls: ['./progress-bar.component.css']
})
export class ProgressBarComponent implements OnInit {

  TimesList$: Observable<Array<number>>;

  progressValue;
  progressValueToHTML =  this.progressValue + '%';
  isAvailable = true; // jezeli dalo sie wuliczyc to wyswietl jezeli np nie
  // ma zaplanowanego meczu i nie dalo sie wyswietlic to nie wyswietlaj calego komunikatu
  timeToMatch;
  constructor(private httpMatchService: HttpMatchService, private router: Router ) { }

  ngOnInit() {
    this.TimesList$ = this.httpMatchService.getTimesToNearstmatch();
    this.TimesList$.subscribe( x => {
      if (x == null) {
        console.log('x:' + x);
        this.isAvailable = false;
      } else {
        this.progressValue = x[3];  // przypisuje ile procent pozostalo do nastepnego meczu liczac od ostatniego do nastepnego
        console.log(x);
        this.progressValueToHTML =  this.progressValue + '%'; // łaczy ta wartosc z procentem aby uzyc tego w kodzie HTML
        this.timeToMatch = x[0];
        }
    });

  }

}
