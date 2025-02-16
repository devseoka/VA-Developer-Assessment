import { Component } from '@angular/core';
import { SharedModule } from './shared/shared.module';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app.routes';
import { HttpClientModule } from '@angular/common/http'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  standalone: true,
  imports: [SharedModule, BrowserModule, AppRoutingModule, HttpClientModule ]
})
export class AppComponent {
  title = 'VA Developer Assessment | Seoka Moshele';
}
