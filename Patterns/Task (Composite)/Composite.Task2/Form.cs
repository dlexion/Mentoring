using System;
using System.Collections.Generic;
using System.Text;

namespace Composite.Task2
{
    public class Form : IComponent
    {
        private List<IComponent> components = new List<IComponent>();

        string name;

        public Form(string name)
        {
            this.name = name;
        }

        public void AddComponent(IComponent component)
        {
            components.Add(component);
        }

        public string ConvertToString(int depth = 0)
        {
            var result = new StringBuilder();
            result.Append($"<form name='{name}'>").AppendLine();
            depth++;

            var whiteSpace = new string(' ', depth);
            foreach (var component in components)
            {
                result.Append(whiteSpace).Append(component.ConvertToString(depth)).AppendLine();
            }

            depth--;

            whiteSpace = new string(' ', depth);
            result.Append(whiteSpace).Append("</form>");

            return result.ToString();
        }
    }
}