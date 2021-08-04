namespace Adapter.Task1
{
    public class MyPrinterFactory
    {
        public IMyPrinter CreateMyPrinter(IWriter writer)
        {
            return new MyPrinterAdapter(new Printer(writer));
        }
    }
}
