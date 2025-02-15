import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { DashboardModule } from './dashboard/dashboard.module';
import { RouterModule } from '@angular/router';
import { HomeComponent } from './static/home/home.component';
import { AboutComponent } from './static/about/about.component';
import { ContactComponent } from './static/contact/contact.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  standalone: true,
  imports: [CommonModule, DashboardModule, RouterModule, HomeComponent, AboutComponent, ContactComponent]
})
export class AppComponent {
  title = 'assessment-ui';
}
