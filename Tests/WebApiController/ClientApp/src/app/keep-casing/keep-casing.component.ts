import { Component, OnInit } from '@angular/core';
import { CasingModel } from './models/casing-model';
import { KeepCasingService } from './services/keep-casing.service';

@Component({
  selector: 'app-keep-casing',
  templateUrl: './keep-casing.component.html',
  styleUrls: ['./keep-casing.component.css']
})
export class KeepCasingComponent implements OnInit {
  public model: CasingModel;

  constructor(
    private readonly service: KeepCasingService
  ) { }

  ngOnInit() {
    this.service.get().subscribe(model => this.model = model);
  }

}
