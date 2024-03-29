import { Component, OnInit } from '@angular/core';
import { CasingModel } from './models/casing-model';
import { FixCasingService } from './services/fix-casing.service';
import { environment } from '../../environments/environment';
import { CasingWithMappingModel } from './models/casing-with-mapping-model';

@Component({
  selector: 'app-fix-casing',
  templateUrl: './fix-casing.component.html',
  styleUrls: ['./fix-casing.component.css']
})
export class FixCasingComponent implements OnInit {
  public model: CasingModel;
  public modelWithMapping: CasingWithMappingModel;

  constructor(
    private readonly service: FixCasingService
  ) {
    this.service.serviceUrl = environment.baseUrl;
  }

  ngOnInit() {
    this.service.get().subscribe(model => this.model = model);
    this.service.getWithMapping().subscribe(model => this.modelWithMapping = model);
  }

}
