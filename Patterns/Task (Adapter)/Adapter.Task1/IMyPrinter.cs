namespace Adapter.Task1
{
    public interface IMyPrinter
    {
        void Print<T>(IElements<T> elements);
    }
}
