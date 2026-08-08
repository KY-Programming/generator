import { WeatherForecast } from './weather-forecast';

/**
 * Hand written model. SetMember(...).ImportFile("custom-weather-forecast", "CustomWeatherForecast")
 * points the generated service at this type instead of the generated WeatherForecast - which is
 * what lets the app add fields the API contract does not have.
 */
export class CustomWeatherForecast extends WeatherForecast {
  public additionalProperty: string = '';
}
