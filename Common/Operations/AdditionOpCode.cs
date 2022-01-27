namespace Common.Operations
{
    public sealed class AdditionOpCode : BinaryExpressionOpCode
    {
        public AdditionOpCode() : base(1, (a,b) => a + b) {}
    }
}