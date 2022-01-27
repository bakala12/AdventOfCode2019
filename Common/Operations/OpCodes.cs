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
        }

        internal OpCode this[int code] => _opcodes[code];
    }
}
