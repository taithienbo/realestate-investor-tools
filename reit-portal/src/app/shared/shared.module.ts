import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToolbarComponent } from './tool-bar/tool-bar.component';

@NgModule({
  declarations: [ToolbarComponent],
  imports: [CommonModule],
  exports: [ToolbarComponent],
})
export class SharedModule {}
