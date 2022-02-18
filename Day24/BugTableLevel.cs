namespace Day24
{
    public struct BugTableLevel
    {
        private int _storage;

        public BugTableLevel(int storage)
        {
            _storage = storage;
        }

        public BugTableLevel(bool[,] array)
        {
            int biodiversity = 0;
            int mask = 1;
            for(int i = 0; i < array.GetLength(0); i++)
                for(int j = 0; j < array.GetLength(1); j++)
                {
                    if(array[i,j])
                        biodiversity |= mask;
                    mask <<= 1;
                }
            _storage = biodiversity;
        }
    
        public bool this[int i, int j]
        {
            get => (_storage & (1 << (5*i+j))) != 0;
            set 
            {
                var mask = 1 << (5*i+j);
                _storage &= ~mask;
                if(value)
                    _storage |= mask;
            }
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            for(int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    if(i == 2 && j == 2)
                        sb.Append("?");
                    else
                        sb.Append(this[i,j] ? "#" : ".");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}