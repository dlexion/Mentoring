using System.Collections.Generic;

namespace Adapter.Task1.Tests
{
    public class MyElements<T> : IElements<T>
    {
        private IEnumerable<T> items;

        public MyElements(IEnumerable<T> items)
        {
            this.items = items;
        }

        public IEnumerable<T> GetElements()
        {
            return this.items;
        }
    }
}
