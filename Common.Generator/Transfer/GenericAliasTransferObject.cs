namespace KY.Generator.Transfer
{
    public class GenericAliasTransferObject
    {
        public TypeTransferObject Alias { get; set; }
        public TypeTransferObject Type { get; set; }

        /// <summary>
        /// True if the generic argument itself is annotated as nullable (<c>List&lt;string?&gt;</c>). This is
        /// independent from the nullability of the type it is used on. It can not be stored on <see cref="Type"/>,
        /// because the read models are shared between all usages of a type
        /// </summary>
        public bool IsNullable { get; set; }
    }
}
