import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-menu2',
  templateUrl: './menu2.component.html',
  styleUrls: ['./menu2.component.css']
})
export class Menu2Component  {


  isOpenb1 = false; // czy rozwinieta pierwsza czesc menu
  isOpenb2 = false;
  isOpenb3 = false;



  constructor(private router: Router) {

  }


  onSelect1() { // ustawia na zwinieta i rozwinieta
    this.isOpenb1 = !this.isOpenb1;
  }

  onSelect2() {
    this.isOpenb2 = !this.isOpenb2;
  }

  onSelect3() {
    this.isOpenb3 = !this.isOpenb3;
  }



  OpenMainPage() {
    this.router.navigate(['/home/main']);
  }


  OpenZawodnicy() {
    this.router.navigate(['/home/players']); // przerzuca na ten adres
  }

  AddPlayerRoute() {
    this.router.navigate(['/home/AddPlayer']);
  }

  OpenPlrListStats() {
    this.router.navigate(['/home/StatsList']);
  }
  OpenCalendar() {
    this.router.navigate(['/home/Calendar']);
  }
  OpenAddMatch() {
    this.router.navigate(['/home/AddMatch']);
  }
  OpenMatchHistory() {
    this.router.navigate(['/home/matchHistory']);
  }




}
