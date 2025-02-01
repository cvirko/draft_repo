using Auth.Domain.Interface.Logic.External.Randomiz;

namespace Auth.Infrastructure.Logic.External.Randomiz
{
    internal class RandomService : IRandomService
    {
        public int Get(int min, int max)
        {
            if (min < 0 || max <= 0)
                throw new ArgumentException($"Negative number {min} or {max}");
            return Random.Shared.Next(min, max + 1);
        }

        public int Get(int max)
        {
            return Get(0, max); ;
        }

        public T Get<T>(params (int chance, T value)[] chances)
        {
            var index = GetIndex(chances.Select(i => i.chance).ToArray());
            return chances[index].value;
        }

        public int GetIndex(params int[] chances)
        {
            for (int i = 0; i < chances.Length; i++)
                if (chances[i] < 0)
                    throw new ArgumentException($"Negative number {chances[i]}");
            int sum = chances.Sum();
            var randomValue = Get(sum);
            for (var i = 0; i < chances.Length; i++)
            {
                if (chances[i] == 0)
                    continue;
                randomValue -= chances[i];
                if (randomValue <= 0)
                    return i;
            }
            return -1;
        }
    }  
}
