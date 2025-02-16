import { Component } from '@angular/core';
import { SharedModule } from './shared/shared.module';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app.routes';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  standalone: true,
  imports: [SharedModule, BrowserModule, AppRoutingModule ]
})
export class AppComponent {
  title = 'VA Developer Assessment | Seoka Moshele';
}
