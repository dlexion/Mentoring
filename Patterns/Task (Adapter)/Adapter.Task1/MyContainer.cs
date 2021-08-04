using System.Collections.Generic;
using System.Linq;

namespace Adapter.Task1
{
    public class MyContainer<T> : IContainer<T>
    {
        public MyContainer(IEnumerable<T> items)
        {
            Items = items;
            Count = Items.Count();
        }

        public IEnumerable<T> Items { get; }
        public int Count { get; }
    }
}