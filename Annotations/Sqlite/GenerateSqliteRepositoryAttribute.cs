using System;
using System.Collections.Generic;

namespace KY.Generator.Sqlite
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class GenerateSqliteRepositoryAttribute : Attribute, IGeneratorCommandAttribute
    {
        public IEnumerable<AttributeCommandConfiguration> Commands
        {
            get
            {
                return new[]
                       {
                           new AttributeCommandConfiguration("sqlite-repository", this.Parameters)
                       };
            }
        }

        public IEnumerable<string> Parameters
        {
            get
            {
                yield return "-assembly=$ASSEMBLY$";
                yield return "-namespace=$NAMESPACE$";
                yield return "-name=$NAME$";
                if (!string.IsNullOrEmpty(this.Table))
                {
                    yield return $"-table={this.Table}";
                }
                if (!string.IsNullOrEmpty(this.RelativePath))
                {
                    yield return $"-relativePath={this.RelativePath}";
                }
                if (!string.IsNullOrEmpty(this.ClassName))
                {
                    yield return $"-className={this.ClassName}";
                }
            }
        }

        public string RelativePath { get; }
        public string Table { get; }
        public string ClassName { get; }

        public GenerateSqliteRepositoryAttribute(string relativePath = null, string table = null, string className = null)
        {
            this.RelativePath = relativePath;
            this.Table = table;
            this.ClassName = className;
        }
    }
}
