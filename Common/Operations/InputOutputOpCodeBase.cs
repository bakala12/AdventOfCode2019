using Common.Streams;

namespace Common.Operations
{
    public abstract class InputOutputOpCodeBase : WriteParameterModeOpCodeBase
    {
        public virtual void ExecuteWithInput(long[] data, IntProgramState state, long[] parameters, IInputStream inputStream) {}

        public virtual void ExecuteWithOutput(long[] data, IntProgramState state, long[] parameters, IOutputStream outputStream) {}

        protected override void ExecuteWithParameters(long[] data, IntProgramState state, long[] parameters)
        {
        }

        public override IntProgramState Execute(long[] data, IntProgramState state, int parameterModes, IInputStream inputStream, IOutputStream outputStream)
        {
            var parameters = GetParameters(data, state, parameterModes);
            ExecuteWithInput(data, state, parameters, inputStream);
            ExecuteWithOutput(data, state, parameters, outputStream);
            return NewState(data, state, parameters);
        }
    }
}