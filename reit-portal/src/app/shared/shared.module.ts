import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuTabComponent } from './menu-tab/menu-tab.component';

@NgModule({
  declarations: [MenuTabComponent],
  imports: [CommonModule],
  exports: [MenuTabComponent],
})
export class SharedModule {}
