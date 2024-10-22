namespace MyStack
{
    public class Stack<T>
    {
        private T[] _inner;
        private int top;

        public IReadOnlyList<T> List { get { return _inner; } }
        public int Size { get { return _inner.Length; } }

        public Stack(int initialSize)
        {
            _inner = new T[initialSize];
            top = 0;
        }

        public void push(T item)
        {
            if (top >= Size)
            {
                T[] newInner = _inner;
                _inner = new T[Size + 1];
                for (int i = 0; i < newInner.Length; i++)
                {
                    _inner[i] = newInner[i];
                }
                _inner[top] = item;
                top++;
            }
            else
            {
                _inner[top] = item;
                top++;
            }
        }

        public void pop()
        {
            top--;
        }

        public T peek()
        {
            return _inner[top - 1];
        }
    }
}
