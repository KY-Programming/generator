using System.Collections.Generic;

namespace KY.Generator.EntityFramework.Configurations
{
    public class EntityFrameworkWriteConfiguration
    {
        public string Namespace { get; set; }
        public string RelativePath { get; set; }
        public List<EntityFrameworkWriteRepositoryConfiguration> Repositories { get; set; }
        public virtual bool IsCore => false;
        public List<string> Usings { get; set; }

        //public List<IFluentLanguageElement> Fluent { get; private set; }
        public EntityFrameworkDataContextConfiguration DataContext { get; set; }

        public EntityFrameworkWriteConfiguration()
        {
            // this.Language = CsharpLanguage.Instance;
            this.Repositories = new List<EntityFrameworkWriteRepositoryConfiguration>();
            this.Usings = new List<string>();
            //this.Fluent = new List<IFluentLanguageElement>();
            this.DataContext = new EntityFrameworkDataContextConfiguration();
        }
    }
}
