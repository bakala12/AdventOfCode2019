namespace Common.Operations
{
    internal class OpCodes
    {
        private readonly OpCode[] _opcodes;

        internal OpCodes()
        {
            _opcodes = new OpCode[100];
            _opcodes[1] = new AdditionOpCode();
            _opcodes[2] = new MultiplicationOpCode();
            _opcodes[99] = new ExitOpCode();
            _opcodes[3] = new InputOpCode();
            _opcodes[4] = new OutputOpCode();
            _opcodes[5] = new JumpIfTrueOpCode();
            _opcodes[6] = new JumpIfFalseOpCode();
            _opcodes[7] = new LessThanOpCode();
            _opcodes[8] = new EqualsOpCode();
            _opcodes[9] = new RelativeBaseMoveOpCode();
        }

        internal OpCode this[int code] => _opcodes[code];
    }
}
