/**
 * Stands in for a type owned by a shared model library that the app consumes as
 * "@my-lib/models". The generator is told about it with [GenerateImport] on the controller,
 * so the generated service imports this type rather than emitting its own copy.
 */
export class SpecialWeatherForecast {
  public date: Date = new Date(0);
  public temperatureC: number = 0;
  public temperatureF: number = 0;
  public summary: string = '';
  public warning: string = '';
}
