# WebApiController

Tests comprehensive Angular service generation from ASP.NET Core WebAPI controllers with various return types, complex models, generics, derived controllers, custom casing, and multiple output scenarios.

## Output

- ClientApp/src/app/convert-to-interface/models/
    - convert-me-optional.ts #0ed1c155
    - convert-me.ts #1eda9a2b
    - index.ts #add6d6c2
- ClientApp/src/app/convert-to-interface/services/
    - convert-to-interface-optional.service.ts #b1683e73
    - convert-to-interface.service.ts #38c0ca0a
    - index.ts #d5adf9a6
- ClientApp/src/app/date/models/
    - date-array-wrapper.ts #3a344ce4
    - date-model-array-wrapper.ts #76c7ed9c
    - date-model-wrapper-list-wrapper.ts #76950e08
    - date-model-wrapper-with-date.ts #adcc533c
    - date-model-wrapper.ts #90dce6ee
    - date-model.ts #eb83273d
    - generic-result.ts #0dfc9ad6
    - index.ts #a68528db
    - optional-properties-model.ts #789338a9
- ClientApp/src/app/date/services/
    - date.service.ts #35dc9d86
    - index.ts #bf45753f
    - optional-property.service.ts #5bb37698
- ClientApp/src/app/derived/services/
    - derived.service.ts #30580382
- ClientApp/src/app/duplicate-name/services/
    - duplicate-name.service.ts #5c8e714a
- ClientApp/src/app/edge-cases/models/
    - date-model.ts #eb83273d
    - exclusive-generic-complex-result.ts #86a6ce5a
    - generic-result.ts #0dfc9ad6
    - index.ts #9e36b4ac
    - self-referencing-model.ts #0076bab5
- ClientApp/src/app/edge-cases/services/
    - edge-cases.service.ts #ab16d38f
- ClientApp/src/app/fix-casing/models/
    - casing-model.ts #e528fe41
    - casing-with-mapping-model.ts #10f64156
    - index.ts #8fa6976e
- ClientApp/src/app/fix-casing/services/
    - fix-casing.service.ts #b628ee61
- ClientApp/src/app/get-complex/models/
    - get-complex-model-service.ts #79510a2d
    - get-complex-model.ts #157ca97f
    - index.ts #308d1660
- ClientApp/src/app/get-complex/services/
    - get-complex.service.ts #0bfe9dc6
- ClientApp/src/app/http-types/services/
    - http-types.service.ts #08808695
- ClientApp/src/app/invalid-words/services/
    - invalid-words.service.ts #6ce41fb6
- ClientApp/src/app/keep-casing/models/
    - keep-casing-model.ts #2b3de504
- ClientApp/src/app/keep-casing/services/
    - keep-casing.service.ts #25b12b19
- ClientApp/src/app/parameter-on-controller/services/
    - parameter-on.service.ts #1e2cf31a
- ClientApp/src/app/post/models/
    - post-model.ts #7aeb96c2
- ClientApp/src/app/post/services/
    - post.service.ts #188386f0
- ClientApp/src/app/produces/models/
    - weather-forecast.ts #1abd289c
- ClientApp/src/app/produces/services/
    - produces.service.ts #3b5b7271
- ClientApp/src/app/rename/models/
    - data.ts #2d84f58a
    - index.ts #8a029a87
    - rename-model.ts #8b3f531c
- ClientApp/src/app/rename/services/
    - rename.service.ts #0510c024
- ClientApp/src/app/routed/models/
    - weather-forecast.ts #1abd289c
- ClientApp/src/app/routed/services/
    - routed.service.ts #36cfcc7f
- ClientApp/src/app/versioned-api/models/
    - weather-forecast.ts #1abd289c
- ClientApp/src/app/versioned-api/services/
    - versioned-api.service.ts #6aada28a
- ClientApp/src/app/warnings/models/
    - weather-forecast.ts #1abd289c
- ClientApp/src/app/warnings/services/
    - warning.service.ts #0ed14252

## Status

Last Build: 2026-05-09 06:12:47
Status: Failure
Info: Build failed
Generator: 10.0.0-preview.39
