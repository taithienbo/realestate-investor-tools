import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-menu-tab',
  templateUrl: './menu-tab.component.html',
  styleUrls: ['./menu-tab.component.scss'],
})
export class MenuTabComponent {
  tabs: Tab[];

  constructor(private router: Router) {
    this.tabs = [
      new Tab('Summary', true),
      new Tab('Listing', false),
      new Tab('Expenses', false),
      new Tab('Incomes', false),
      new Tab('Analysis', false),
      new Tab('Future Projections', false),
    ];
  }

  selectTab(tab: Tab) {
    this.tabs.forEach((tab) => (tab.isActive = false));
    tab.isActive = true;
    this.displayTab(tab.title);
  }

  private displayTab(title: string) {
    if (title === 'Summary') {
      this.router.navigate(['/summary']);
    } else if (title === 'Listing') {
      this.router.navigate(['/listing']);
    }
  }
}

export class Tab {
  constructor(public title: string, public isActive: boolean) {}
}
