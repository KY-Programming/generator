import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CoreApiComponent } from './components/core-api/core-api.component';
import { WebApiComponent } from './components/web-api/web-api.component';


const routes: Routes = [
  { path: '', redirectTo: 'web-api', pathMatch: 'full' },
  { path: 'web-api', component: WebApiComponent },
  { path: 'core-api', component: CoreApiComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
