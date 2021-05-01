import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { FixCasingComponent } from './fix-casing/fix-casing.component';
import { HomeComponent } from './home/home.component';
import { KeepCasingComponent } from './keep-casing/keep-casing.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { VersionedApiComponent } from './versioned-api/versioned-api.component';
import { DateComponent } from './date/date.component';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    FixCasingComponent,
    KeepCasingComponent,
    VersionedApiComponent,
    DateComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'fix-casing', component: FixCasingComponent },
      { path: 'keep-casing', component: KeepCasingComponent },
      { path: 'versioned-api', component: VersionedApiComponent },
      { path: 'date', component: DateComponent },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
