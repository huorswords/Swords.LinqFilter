namespace Swords.LinqFilter
{
    using System.Collections.Generic;

    public class FilterValue
    {
        private const string DefaultSeparator = ";";

        public FilterValue()
        {
            this.Separators = new List<string> { DefaultSeparator };
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public IEnumerable<string> Separators { get; set; }
    }
}