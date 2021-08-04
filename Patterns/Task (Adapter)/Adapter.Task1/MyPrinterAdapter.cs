using System.Collections.Generic;
using System.Linq;

namespace Adapter.Task1
{
    internal class MyPrinterAdapter : IMyPrinter
    {
        private readonly Printer _printer;

        public MyPrinterAdapter(Printer printer)
        {
            _printer = printer;
        }

        public void Print<T>(IElements<T> elements)
        {
            var container = new MyContainer<T>(elements.GetElements());

            _printer.Print(container);
        }
    }


}