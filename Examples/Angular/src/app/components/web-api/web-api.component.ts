import { Component } from '@angular/core';
import { ValuesService } from 'src/app/services/values.service';

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

  public get2(): void {
    this.result += 'Execute get2...\r\n';
    this.valuesService.get2(5).subscribe(result => {
      this.result += 'Result from get2: ' + JSON.stringify(result) + '\r\n';
    }, error => {
      this.result += 'Error from get2: ' + JSON.stringify(error) + '\r\n';
    });
  }

  public post(): void {
    this.result += 'Execute post...\r\n';
    this.valuesService.post({ id: 5, text: 'test-1' }).subscribe(() => {
      this.result += 'Result from post: void\r\n';
    }, error => {
      this.result += 'Error from post: ' + JSON.stringify(error) + '\r\n';
    });
  }

  public put(): void {
    this.result += 'Execute put...\r\n';
    this.valuesService.put(5, { id: 5, text: 'test-1' }).subscribe(() => {
      this.result += 'Result from put: void\r\n';
    }, error => {
      this.result += 'Error from put: ' + JSON.stringify(error) + '\r\n';
    });
  }

  public delete(): void {
    this.result += 'Execute delete...\r\n';
    this.valuesService.delete(5).subscribe(() => {
      this.result += 'Result from delete: void\r\n';
    }, error => {
      this.result += 'Error from delete: ' + JSON.stringify(error) + '\r\n';
    });
  }
}
