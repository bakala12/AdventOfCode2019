namespace Day12
{
    public static class MoonMover
    {
        public static void ApplyGravityAndMove(Moon[] moons)
        {
            for(int i = 0; i < moons.Length; i++)
            {
                for(int j = i+1; j < moons.Length; j++)
                {
                    var posDiff = (moons[i].Position - moons[j].Position).Sign();
                    moons[i].Velcity -= posDiff;
                    moons[j].Velcity += posDiff;
                }
                moons[i].Position += moons[i].Velcity;
            }
        }
    }
}