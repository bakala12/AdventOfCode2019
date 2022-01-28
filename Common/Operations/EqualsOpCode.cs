namespace Common.Operations
{
    public sealed class EqualsOpCode : BinaryExpressionOpCode
    {
        public EqualsOpCode() : base(8, (a,b) => (a == b ? 1 : 0)) {}
    }
}