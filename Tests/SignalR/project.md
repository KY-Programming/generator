# SignalR

Tests SignalR hub-to-Angular service generation with [GenerateAngularHub] and [GenerateWithRetry] annotations, demonstrating hub client interface generation and retry policy configuration.

## Output

- ClientApp/src/app/models/
    - connection-status.ts #5e71a874
    - weather-forecast.ts #33a9d8bf
- ClientApp/src/app/multiple/models-1/
    - connection-status.ts #5e71a874
- ClientApp/src/app/multiple/models-2/
    - connection-status.ts #5e71a874
- ClientApp/src/app/multiple/services-1/
    - multiple-output-hub.service.ts #a0e51fda
- ClientApp/src/app/multiple/services-2/
    - multiple-output-hub.service.ts #b42a875f
- ClientApp/src/app/services/
    - weather-forecast-hub.service.ts #6051b256

## Status

Last Build: 2026-05-09 06:12:46
Status: Failure
Info: Build failed
Generator: 10.0.0-preview.39
