namespace Common.Operations
{
    public sealed class LessThanOpCode : BinaryExpressionOpCode
    {
        public LessThanOpCode() : base(7, (a,b) => (a < b ? 1 : 0)) {}
    }
}