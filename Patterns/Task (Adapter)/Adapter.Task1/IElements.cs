using System.Collections.Generic;

namespace Adapter.Task1
{
    public interface IElements<T>
    {
        IEnumerable<T> GetElements();
    }
}
