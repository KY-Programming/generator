using System;

namespace KY.Generator.Templates
{
    public class TypeTemplate : CodeFragment
    {
        public string Name { get; }
        public bool IsInterface { get; }
        public bool IsNullable { get; }

        public TypeTemplate(string name, bool isInterface = false, bool isNullable = false)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.IsInterface = isInterface;
            this.IsNullable = isNullable;
        }

        public override bool Equals(object obj)
        {
            var type = obj as TypeTemplate;
            return type != null && this.Name.Equals(type.Name);
        }

        public static bool operator ==(TypeTemplate left, TypeTemplate right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TypeTemplate left, TypeTemplate right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}