namespace Game
{
    public class Singletone<T> where T : new()
    {
        public static T Instance { get; private set; } = new T();
    }
}
