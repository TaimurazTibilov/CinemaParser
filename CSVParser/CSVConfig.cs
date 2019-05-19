using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVParser
{
    public class CSVConfig
    {
        public char Delimiter { get; private set; }
        public string NewLineMark { get; private set; }
        public char QuotationMark { get; private set; }

        public CSVConfig(char delimiter, string newLineMark, char quotationMark)
        {
            Delimiter = delimiter;
            NewLineMark = newLineMark;
            QuotationMark = quotationMark;
        }

        // useful configs

        public static CSVConfig Default
        {
            get { return new CSVConfig(';', "\r\n", '\"'); }
        }

        // etc.
    }
}
