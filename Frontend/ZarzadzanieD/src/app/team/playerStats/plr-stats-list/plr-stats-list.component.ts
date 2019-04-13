import { Component, OnInit } from '@angular/core';
import { HttpPlayerService } from 'src/app/mainServices/http-player.service';
import { Observable } from 'rxjs/internal/Observable';
import { playersStatisticInMatch } from 'src/app/Domain/PlayersStatistics';



@Component({
  selector: 'app-plr-stats-list',
  templateUrl: './plr-stats-list.component.html',
  styleUrls: ['./plr-stats-list.component.css']
})
export class PlrStatsListComponent implements OnInit {

  PlayersStatsList$: Observable<Array<playersStatisticInMatch>>;
  WyszukiwarkaNazwisko = '';



  constructor(private httpPlayerService: HttpPlayerService) {
  }

  ngOnInit() {
    this.PlayersStatsList$ = this.httpPlayerService.getPlayersStatsList();
  }


}


// tslint:disable-next-line:class-name


