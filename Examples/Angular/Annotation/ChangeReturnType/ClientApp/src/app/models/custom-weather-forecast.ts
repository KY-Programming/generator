/**
 * Hand written model. The controller's [GenerateImport] points the generated service at
 * "../models" for this type, so the generator imports it instead of emitting its own copy -
 * which is what lets the app add fields the API contract does not have.
 */
export class CustomWeatherForecast {
  public date: Date = new Date(0);
  public temperatureC: number = 0;
  public temperatureF: number = 0;
  public summary: string = '';
  public additionalProperty: string = '';
}
