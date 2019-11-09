import { Component } from '@angular/core';
import { ValuesService } from 'src/app/services/values-service';

@Component({
  selector: 'app-web-api',
  templateUrl: './web-api.component.html',
  styleUrls: ['./web-api.component.scss']
})
export class WebApiComponent {
  public result = 'Ready...\r\n';

  public constructor(
    private readonly valuesService: ValuesService
  ) {
  }

  public get(): void {
    this.result += 'Execute get...\r\n';
    this.valuesService.get().subscribe(result => {
      this.result += 'Result from get: ' + JSON.stringify(result) + '\r\n';
    }, error => {
      this.result += 'Error from get: ' + JSON.stringify(error) + '\r\n';
    });
  }
}
