using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Models
{
    public class HistoryItem
    {
        public string Expression { get; set; }
        public string Result { get; set; }
        public string FormattedEntry => $"{Expression} = {Result}";
    }
}