import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { DashboardModule } from './dashboard/dashboard.module';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  standalone: true,
  imports: [CommonModule, DashboardModule, RouterModule]
})
export class AppComponent {
  title = 'assessment-ui';
}
