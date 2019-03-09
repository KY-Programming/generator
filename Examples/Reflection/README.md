# Reflection Examples

## KY.Generator.Examples.Reflection.Attributes
Generate C# or TypeScript classes from object decorated with Generate attribute.  
FirstType and SecondType are only decorated with Generate attribute. Default configuration from Post-build event is used.  
ForceTypeScript overrides default value with special configuration for TypeScript.

## KY.Generator.Examples.Reflection.Console
Generate C# or TypeScript classes from an assembly. Select an type via -namespace and -name parameter.
Command in Post-build event triggers the generation

## KY.Generator.Examples.Reflection.Web
Generate TypeScript models for an angular frontend by reading the backend models and convert them.  
Command in Post-build event triggers the generation
