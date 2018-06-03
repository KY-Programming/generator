using System.Linq;
using KY.Generator.Mappings;
using KY.Generator.Templates;
using KY.Generator.Tsql.Entity;

namespace KY.Generator.Tsql.Extensions
{
    public static class TsqlODataExtension
    {
        public static string GetModelName(this TsqlEntity oData)
        {
            return oData.Configuration.ClassMapping.Get(oData.Name);
        }

        public static TypeTemplate GetModelType(this TsqlEntity oData)
        {
            return KY.Generator.Code.Type(oData.GetModelName());
        }

        public static string GetRepositoryName(this TsqlEntity oData)
        {
            return oData.GetModelName() + "Repository";
        }

        public static TypeTemplate GetRepositoryType(this TsqlEntity oData)
        {
            return KY.Generator.Code.Type(oData.GetRepositoryName());
        }

        public static string GetControllerName(this TsqlEntity oData)
        {
            return oData.GetModelName() + "Controller";
        }

        public static TypeTemplate GetControllerType(this TsqlEntity oData)
        {
            return KY.Generator.Code.Type(oData.GetControllerName());
        }

        public static string GetDataContextName(this TsqlEntity oData)
        {
            return oData.DataContext?.Name ?? oData.Configuration.Entities.FirstOrDefault(x => x.DataContext != null)?.DataContext.Name ?? "DataContext";
        }

        public static TypeTemplate GetDataContextType(this TsqlEntity oData)
        {
            return KY.Generator.Code.Type(oData.GetDataContextName());
        }
    }
}