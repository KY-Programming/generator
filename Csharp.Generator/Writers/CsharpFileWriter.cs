using System.Linq;
using KY.Generator.Output;
using KY.Generator.Templates;
using KY.Generator.Writers;

namespace KY.Generator.Csharp.Writers
{
    public class CsharpFileWriter : FileWriter
    {
        public override void Write(ICodeFragment fragment, IOutputCache output)
        {
            FileTemplate template = (FileTemplate)fragment;
            INamespaceChildren children = template.Namespaces.FirstOrDefault()?.Children.FirstOrDefault();
            UsingTemplate usingTemplate = new("System.CodeDom.Compiler", null, null);
            children?.Usings.Add(usingTemplate);
            base.Write(template, output);
            children?.Usings.Remove(usingTemplate);
        }

        protected override string DefaultLintSuppression => "// ReSharper disable All";

        protected override void WriteHeader(FileTemplate fileTemplate, IOutputCache output, bool appendBlankLine = true)
        {
            base.WriteHeader(fileTemplate, output, appendBlankLine);
            // A nullable annotation is only legal inside a nullable context, and auto-generated code does not
            // inherit the one of the project - the compiler reports CS8669 and demands an explicit directive. Only
            // written when the file actually carries an annotation, so files without one stay as they were.
            if (ContainsNullableAnnotation(fileTemplate))
            {
                output.Add("#nullable enable").BreakLine()
                      // The nullable context also demands that every non nullable member is assigned before the
                      // constructor returns (CS8618). These are data holders - whatever reads the database or
                      // deserializes the payload fills them - so the check is switched off rather than answered
                      // with a default value that would only hide a missing one.
                      .Add("#pragma warning disable CS8618").BreakLine().BreakLine();
            }
        }

        private static bool ContainsNullableAnnotation(FileTemplate fileTemplate)
        {
            return fileTemplate.Namespaces.SelectMany(nameSpace => nameSpace.Children).OfType<ClassTemplate>().Any(ContainsNullableAnnotation);
        }

        private static bool ContainsNullableAnnotation(ClassTemplate classTemplate)
        {
            return classTemplate.Properties.Any(property => property.Type is { IsNullable: true })
                   || classTemplate.Fields.Any(field => field.Type is { IsNullable: true })
                   || classTemplate.Classes.Any(ContainsNullableAnnotation);
        }
    }
}
