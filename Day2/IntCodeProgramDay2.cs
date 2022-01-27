namespace Day2
{
    public class IntCodeProgramDay2 : IntCodeProgramBase
    {
        protected override void RegisterOpCodes()
        {
            _opCodes[1] = (numbers, pos) => { numbers[numbers[pos+3]] = numbers[numbers[pos+2]] + numbers[numbers[pos+1]]; return pos+4; };
            _opCodes[2] = (numbers, pos) => { numbers[numbers[pos+3]] = numbers[numbers[pos+2]] * numbers[numbers[pos+1]]; return pos+4; };
            _opCodes[99] = (numbers, pos) => numbers.Length;
        }
    }
}