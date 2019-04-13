import { Injectable } from '@angular/core';
declare let alertify: any;  // deklarujemy zmienna - chyba globalna

@Injectable({
  providedIn: 'root'
})
export class AlertifyServicesService {

  constructor() { }

  confirm(message: string, okCallback: () => any) { // confirm przyjmuje message i metode
    alertify.confirm(message, function(e) { // i jezeli funkcja przyjmujaca e czyli cokolwiek
      if (e) {  // dostanie true czyli ze sie powiodlo to zwraca okcallback,
        // cyzli ta metode ktora przekazalismy tu do confirm
        okCallback();
      } else {}
    });
  }

  success(message: string) {
    alertify.success(message);
  }
  error(message: string) {
    alertify.error(message);
  }
  warning(message: string) {
    alertify.warning(message);
  }
  message(message: string) {
    alertify.message(message);
  }

}
