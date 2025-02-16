import { Component } from '@angular/core';
import { HttpClientModule } from '@angular/common/http'
import { SharedModule } from '@shared/shared.module';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { DashboardModule } from '@dashboard/dashboard.module';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    HttpClientModule,
    SharedModule,
    DashboardModule]
})
export class AppComponent {
  title = 'VA Developer Assessment | Seoka Moshele';
}
