namespace Auth.Domain.Interface.Logic.External.Randomiz
{
    public interface IRandomService
    {
        public int Get(int min, int max);
        public int Get(int max);
        public int GetIndex(params int[] chances);
        public T Get<T>(params (int chance, T value)[] chances);
    }
}
