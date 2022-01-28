namespace Common.Operations
{
    public sealed class JumpIfFalseOpCode : JumpOpCodeBase
    {
        public override int Code => 6;

        public JumpIfFalseOpCode() : base(a => a == 0) {}
    }
}