namespace Common.Operations
{
    public sealed class JumpIfTrueOpCode : JumpOpCodeBase
    {
        public override int Code => 5;

        public JumpIfTrueOpCode() : base(a => a != 0) {}
    }
}