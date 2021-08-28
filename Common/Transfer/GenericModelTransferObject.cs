using System;
using System.Collections.Generic;
using KY.Generator.Languages;
using KY.Generator.Templates;

namespace KY.Generator.Transfer
{
    public class GenericModelTransferObject : ModelTransferObject
    {
        private readonly ModelTransferObject target;

        public override string Name
        {
            get => this.target.Name;
            set => throw new InvalidOperationException();
        }

        public override string FileName
        {
            get => this.target.FileName;
            set => throw new InvalidOperationException();
        }

        public override string OriginalName
        {
            get => this.target.OriginalName;
            set => throw new InvalidOperationException();
        }

        public override string Namespace
        {
            get => this.target.Namespace;
            set => throw new InvalidOperationException();
        }

        public override bool FromSystem
        {
            get => this.target.FromSystem;
            set => throw new InvalidOperationException();
        }

        public override bool IsNullable
        {
            get => this.target.IsNullable;
            set => throw new InvalidOperationException();
        }

        public override List<GenericAliasTransferObject> Generics { get; } = new();

        public override TypeTransferObject Original
        {
            get => this.target.Original;
            set => throw new InvalidOperationException();
        }

        public override ICodeFragment Default
        {
            get => this.target.Default;
            set => throw new InvalidOperationException();
        }

        public override bool HasUsing
        {
            get => this.target.HasUsing;
            set => throw new InvalidOperationException();
        }

        public override bool IsEnum
        {
            get => this.target.IsEnum;
            set => throw new InvalidOperationException();
        }

        public override bool IsInterface
        {
            get => this.target.IsInterface;
            set => throw new InvalidOperationException();
        }

        public override bool IsAbstract
        {
            get => this.target.IsAbstract;
            set => throw new InvalidOperationException();
        }

        public override bool IsGeneric
        {
            get => this.target.IsGeneric;
            set => throw new InvalidOperationException();
        }

        public override Dictionary<string, int> EnumValues
        {
            get => this.target.EnumValues;
            set => throw new InvalidOperationException();
        }

        public override ModelTransferObject BasedOn
        {
            get => this.target.BasedOn;
            set => throw new InvalidOperationException();
        }

        public override ILanguage Language
        {
            get => this.target.Language;
            set => throw new InvalidOperationException();
        }

        public override List<TypeTransferObject> Interfaces => this.target.Interfaces;
        public override List<FieldTransferObject> Constants => this.target.Constants;
        public override List<FieldTransferObject> Fields => this.target.Fields;
        public override List<PropertyTransferObject> Properties => this.target.Properties;
        public override List<MethodTransferObject> Methods => this.target.Methods;
        public override List<string> Usings => this.target.Usings;

        public override string Comment
        {
            get => this.target.Comment;
            set => throw new InvalidOperationException();
        }

        public GenericModelTransferObject(ModelTransferObject target)
        {
            this.target = target;
        }
    }
}
