namespace Beispiel
{
    public class Stack<T>
    {
        private readonly List<T> items = [];

        public Stack()
        {

        }

        public void Push(T item)
        {
            items.Add(item);
        }

        public T Pop()
        {
            if (items.Count == 0)
            {
                throw new InvalidOperationException("Stack is empty.");
            }

            int lastIndex = items.Count - 1;
            T poppedItem = items[lastIndex];
            items.RemoveAt(lastIndex);
            return poppedItem;
        }

        public T Peek()
        {
            if (items.Count == 0)
            {
                throw new InvalidOperationException("Stack is empty.");
            }

            return items[items.Count - 1];
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool IsEmpty 
        { 
            get 
            { 
                return items.Count < 1; 
            } 
        }
    }
}
