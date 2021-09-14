using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Generator.Languages;
using KY.Generator.Templates;

namespace KY.Generator.Transfer
{
    public class GenericModelTransferObject : ModelTransferObject, ICloneable
    {
        public ModelTransferObject Template { get; }

        public override string Name
        {
            get => this.Template.Name;
            set => this.Template.Name = value;
        }

        public override string FileName
        {
            get => this.Template.FileName;
            set => this.Template.FileName = value;
        }

        public override string OriginalName
        {
            get => this.Template.OriginalName;
            set => this.Template.OriginalName = value;
        }

        public override string Namespace
        {
            get => this.Template.Namespace;
            set => this.Template.Namespace = value;
        }

        public override bool FromSystem
        {
            get => this.Template.FromSystem;
            set => this.Template.FromSystem = value;
        }

        public override bool IsNullable
        {
            get => this.Template.IsNullable;
            set => this.Template.IsNullable = value;
        }

        public override TypeTransferObject Original
        {
            get => this.Template.Original;
            set => this.Template.Original = value;
        }

        public override ICodeFragment Default
        {
            get => this.Template.Default;
            set => this.Template.Default = value;
        }

        public override bool IsEnum
        {
            get => this.Template.IsEnum;
            set => this.Template.IsEnum = value;
        }

        public override bool IsInterface
        {
            get => this.Template.IsInterface;
            set => this.Template.IsInterface = value;
        }

        public override bool IsAbstract
        {
            get => this.Template.IsAbstract;
            set => this.Template.IsAbstract = value;
        }

        public override bool IsGeneric
        {
            get => this.Template.IsGeneric;
            set => this.Template.IsGeneric = value;
        }

        public override bool IsGenericParameter
        {
            get => this.Template.IsGenericParameter;
            set => this.Template.IsGenericParameter = value;
        }

        public override Dictionary<string, int> EnumValues
        {
            get => this.Template.EnumValues;
            set => this.Template.EnumValues = value;
        }

        public override ModelTransferObject BasedOn
        {
            get => this.Template.BasedOn;
            set => this.Template.BasedOn = value;
        }

        public override ILanguage Language
        {
            get => this.Template.Language;
            set => this.Template.Language = value;
        }

        public override List<TypeTransferObject> Interfaces => this.Template.Interfaces;
        public override List<MethodTransferObject> Methods => this.Template.Methods;
        public override List<string> Usings => this.Template.Usings;

        public override string Comment
        {
            get => this.Template.Comment;
            set => this.Template.Comment = value;
        }

        public GenericModelTransferObject(ModelTransferObject template)
        {
            this.Template = template is GenericModelTransferObject genericTemplate ? genericTemplate.Template : template;
        }

        object ICloneable.Clone()
        {
            GenericModelTransferObject clone = new(this.Template);
            this.Generics.Select(x => new GenericAliasTransferObject { Alias = x.Alias.Clone(), Type = x.Type }).ForEach(clone.Generics.Add);
            return clone;
        }
    }
}
