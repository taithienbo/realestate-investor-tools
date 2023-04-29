import { Component } from '@angular/core';

@Component({
  selector: 'app-menu-tab',
  templateUrl: './menu-tab.component.html',
  styleUrls: ['./menu-tab.component.scss'],
})
export class MenuTabComponent {
  tabs: Tab[];
  constructor() {
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
  }
}

export class Tab {
  constructor(public title: string, public isActive: boolean) {}
}
