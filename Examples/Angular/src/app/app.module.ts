import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { WebApiComponent } from './components/web-api/web-api.component';
import { ValuesService } from './services/values-service';


@NgModule({
  declarations: [
    AppComponent,
    WebApiComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
  public constructor(
    valuesService: ValuesService
  ) {
    valuesService.serviceUrl = environment.serviceUrl;
  }
}
