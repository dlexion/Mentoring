using System.Collections.Generic;

namespace Adapter.Task1
{
    public interface IContainer<T>
    {
        public IEnumerable<T> Items { get; }
        public int Count { get; }
    }
}
