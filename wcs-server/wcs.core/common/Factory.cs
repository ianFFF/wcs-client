using System;

namespace wcs.core.common
{
    public class Factory<T> where T : class, new()
    {
        private static T _instance;

        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }

                return _instance;
            }
        }
    }
}

