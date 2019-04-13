import { Pipe, PipeTransform } from '@angular/core';
import { Player } from '../Domain/Player';


@Pipe({
  name: 'filterSurname'
})
export class FilterSurnamePipe implements PipeTransform {

  transform(value?: Array<Player>, surArg?: string): any {

    const aa = surArg;
    if (aa === '' || aa === null ) {  // musi być że tez w razie wypadku zwraca wartosci niezmienione bo bedzie error
      return value;
    }
    if ( value !== null) {
      return value.filter(elem => elem.surname.toLowerCase().includes(aa.toLowerCase()));
    }
      return null;
  }

}

