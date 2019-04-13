import { Pipe, PipeTransform } from '@angular/core';
import { Match } from '../Domain/Match';

@Pipe({
  name: 'filterMatch'
})
export class FilterMatchPipe implements PipeTransform {

  transform(value?: Array<Match>, surArg?: string): any {

    const aa = surArg;
    if ( aa == null || aa === undefined || aa === '' ) {
      return value;

    } else if ( value !== undefined || value !== null ) {
      return value.filter(elem => elem.opponentTeam.toLowerCase().includes(surArg.toLowerCase()));
    }
    return null;

  }
}
