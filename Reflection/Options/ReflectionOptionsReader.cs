using System.Reflection;

namespace KY.Generator.Reflection
{
    public class ReflectionOptionsReader : IGlobalOptionsReader
    {
        public void Read(object key, OptionsSet entry)
        {
            if (key is not ICustomAttributeProvider attributeProvider)
            {
                return;
            }
            foreach (object attribute in attributeProvider.GetCustomAttributes(true))
            {
                switch (attribute)
                {
                    case GenerateIgnoreAttribute:
                        entry.Part.Ignore = true;
                        break;
                    case GeneratePreferInterfacesAttribute:
                        entry.Part.PreferInterfaces = true;
                        break;
                    case GenerateStrictAttribute:
                        entry.Part.Strict = true;
                        break;
                    case GenerateRenameAttribute renameAttribute:
                        entry.Part.ReplaceName[renameAttribute.Replace] = renameAttribute.With;
                        break;
                }
            }
        }
    }
}
