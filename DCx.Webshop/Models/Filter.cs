using System.Collections.Generic;

namespace DCx.Webshop.Models
{
    public class Filter
    {
        public string Category { get; set; }
        public List<FilterItem> Values { get; set; } = new List<FilterItem>();
        private IEnumerable<string> _selected = new string[] { };
        public IEnumerable<string> Selected
        {
            set
            {
                _selected = value ?? new string[] { };
            }
            get
            {
                return _selected;
            }
        }
        public string Column { get; set; }
    }
}
