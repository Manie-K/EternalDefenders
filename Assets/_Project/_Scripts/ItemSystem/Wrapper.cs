using UnityEngine;

namespace EternalDefenders
{
    public class Wrapper<T>
    {
        public T Value;

        public Wrapper(T value)
        {
            Value = value;
        }
    }
}
