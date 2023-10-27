namespace Helpers
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        protected Singleton()
        {
        }

        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                    instance.Init();
                }
                return instance;
            }
        }

        protected abstract void Init();
    }
}
