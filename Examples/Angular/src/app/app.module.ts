import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreApiComponent } from './components/core-api/core-api.component';
import { WebApiComponent } from './components/web-api/web-api.component';
import { ValuesCoreService } from './services/values-core-service';
import { ValuesService } from './services/values-service';


@NgModule({
  declarations: [
    AppComponent,
    WebApiComponent,
    CoreApiComponent
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
    valuesService: ValuesService,
    valuesCoreService: ValuesCoreService
  ) {
    valuesService.serviceUrl = environment.serviceUrl;
    valuesCoreService.serviceUrl = environment.serviceCoreUrl;
  }
}
