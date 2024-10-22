namespace MyStack
{
    public class MyStack<T>
    {
        private T[] _inner;

        public IReadOnlyList<T> List { get { return _inner; } }
        public int Size { get { return _inner.Length; } }

        public MyStack(int initialSize)
        {
            _inner = new T[initialSize];
        }

        public void push(T item)
        {
            for (int i = 0; i < Size; i++)
            {
                if (_inner[i] == null)
                {
                    _inner[i] = item;
                    return;
                }
            }

            T[] newInner = _inner;
            _inner = new T[Size + 1];
            int j = 0;
            for (int i = 0; i < Size - 1; i++)
            {
                _inner[i] = newInner[i];
                j = i;
            }

            _inner[j + 1] = item;
        }

        public void pop()
        {
            for (int i = Size - 1; i >= 0; i--)
            {
                if (_inner[i] != null)
                {
                    _inner[i] = default;
                    return;
                }
            }
        }

        public T peek()
        {
            return List.Last();
        }
    }
}
