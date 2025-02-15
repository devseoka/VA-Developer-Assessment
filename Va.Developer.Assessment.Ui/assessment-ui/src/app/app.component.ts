import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { DashboardModule } from './dashboard/dashboard.module';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  standalone: true,
  imports: [CommonModule, DashboardModule, RouterModule, HttpClientModule]
})
export class AppComponent {
  title = 'Seoka Moshele | Virtual Agent | Developer Assessment';
}
