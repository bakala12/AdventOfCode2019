namespace Common.Operations
{
    public sealed class MultiplicationOpCode : BinaryExpressionOpCode
    {
        public MultiplicationOpCode() : base(2, (a,b) => a * b) {}
    }
}