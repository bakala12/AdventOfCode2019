using System.Collections.Generic;

namespace Day24
{
    public class BugsTable
    {
        private DefaultDictionary<int, BugTableLevel> _levels = new DefaultDictionary<int, BugTableLevel>();

        public BugsTable(bool[,] array)
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
            _levels[0] = new BugTableLevel(biodiversity);
        }

        private int CountBugNeighbours(int level, int i, int j)
        {
            int count = 0;
            foreach(var (ni, nj) in GetNeighbours(i,j))
            {
                if(ni == 2 && nj == 2)
                {
                    foreach(var (ci, cj) in GetNeighboursFromCenter(i,j))
                        if(CheckIfBug(level+1, ci, cj)) count++;   
                }
                else if(CheckIfBug(level, ni, nj)) count++;
            }
            return count;
        }

        private IEnumerable<(int, int)> GetNeighboursFromCenter(int i, int j)
        {
            if(i == 1)
                for(int k = 0; k < 5; k++)
                    yield return (0,k);
            else if(i == 3)
                for(int k = 0; k < 5; k++)
                    yield return (4,k);
            else if(j == 1)
                for(int k = 0; k < 5; k++)
                    yield return (k, 0);
            else if(j == 3)
                for(int k = 0; k < 5; k++)
                    yield return (k, 4);
        }

        private IEnumerable<(int, int)> GetNeighbours(int i, int j)
        {
            yield return (i-1,j);
            yield return (i+1,j);
            yield return (i,j-1);
            yield return (i,j+1);
        }

        private bool CheckIfBug(int level, int i, int j)
        {
            if(i < 0)
                return CheckIfBug(level-1, 1, 2);
            if(i >= 5)
                return CheckIfBug(level-1, 3, 2);
            if(j < 0)
                return CheckIfBug(level-1, 2, 1);
            if(j >= 5)
                return CheckIfBug(level-1, 2, 3);
            return _levels[level][i,j];
        }

        public void Iteration()
        {
            var minLevel = _levels.Keys.Min();
            var maxLevel = _levels.Keys.Max();
            var dic = new DefaultDictionary<int, BugTableLevel>();
            for(int l = minLevel; l <= maxLevel; l++)
            {
                dic[l] = IterationForLevel(l);
            }
            dic[minLevel-1] = IterationForNewLevelOutside(minLevel-1);
            dic[maxLevel+1] = IterationForNewLevelInside(maxLevel+1);
            foreach(var l in dic.Keys)
                _levels[l] = dic[l];
        }

        private BugTableLevel IterationForLevel(int l)
        {
            var level = _levels[l];
            var levelCpy = level;
            for(int i = 0; i < 5; i++)
                for(int j = 0; j < 5; j++)
                    if(i != 2 || j != 2)
                    {
                        int b = CountBugNeighbours(l, i, j);
                        if(level[i,j] && b != 1)
                            levelCpy[i,j] = false;
                        else if(!level[i,j] && (b == 1 || b == 2))
                            levelCpy[i,j] = true;
                    }
            return levelCpy;
        }

        private BugTableLevel IterationForNewLevelOutside(int l)
        {
            var level = _levels[l];
            var levelCpy = level;
            foreach(var (i, j) in GetNeighbours(2,2))
            {
                int b = CountBugNeighbours(l, i, j);
                if(level[i,j] && b != 1)
                    levelCpy[i,j] = false;
                else if(!level[i,j] && (b == 1 || b == 2))
                    levelCpy[i,j] = true;
            }
            return levelCpy;
        }

        private BugTableLevel IterationForNewLevelInside(int l)
        {
            var level = _levels[l];
            var levelCpy = level;
            for(int i = 0; i < 5; i += 4)
                for(int j = 0; j < 5; j++)
                {
                    int b = CountBugNeighbours(l, i, j);
                    if(level[i,j] && b != 1)
                        levelCpy[i,j] = false;
                    else if(!level[i,j] && (b == 1 || b == 2))
                        levelCpy[i,j] = true;
                }
            for(int j = 0; j < 5; j += 4)
                for(int i = 0; i < 5; i++)
                {
                    int b = CountBugNeighbours(l, i, j);
                    if(level[i,j] && b != 1)
                        levelCpy[i,j] = false;
                    else if(!level[i,j] && (b == 1 || b == 2))
                        levelCpy[i,j] = true;
                }
            return levelCpy;
        }

        public int CountAllBugs()
        {
            int count = 0;
            foreach(var bl in _levels.Values)
                for(int i = 0; i < 5; i++)
                    for(int j = 0; j < 5; j++)
                        if((i != 2 || j != 2) && bl[i,j])
                            count++;
            return count;
        }
    
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            foreach(var l in _levels.Keys.OrderBy(k => k))
            {
                sb.AppendLine($"Level {l}:");
                sb.AppendLine(_levels[l].ToString());
            }
            return sb.ToString();
        }
    }
}