using System;

namespace Composite.Task2
{
    public class Form : IComponent
    {
        String name;

        public Form(String name)
        {
            this.name = name;
        }

        public void AddComponent(IComponent component)
        {
            throw new NotImplementedException();
        }

        public string ConvertToString(int depth = 0)
        {
            throw new NotImplementedException();
        }
    }
}