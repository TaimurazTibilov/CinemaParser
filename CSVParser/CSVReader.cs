﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVParser
{
    public class CSVReader
    {
        private CSVConfig m_config;

        public CSVReader(CSVConfig config = null)
        {
            if (config == null)
                m_config = CSVConfig.Default;
            else
                m_config = config;
        }

        public IEnumerable<string[]> Read(string csvFileContents)
        {
            using (StringReader reader = new StringReader(csvFileContents))
            {
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                        yield break;
                    yield return ParseLine(line);
                }
            }
        }

        private string[] ParseLine(string line)
        {
            Stack<string> result = new Stack<string>();

            int i = 0;
            while (true)
            {
                string cell = ParseNextCell(line, ref i);
                if (cell == null)
                    break;
                result.Push(cell);
            }
                       

            var resultAsArray = result.ToArray();
            if (resultAsArray.Length != 23)
            {
                throw new Exception("Неверный формат файла!");
            }
            Array.Reverse(resultAsArray);
            return resultAsArray;
        }

        // returns iterator after delimiter or after end of string
        private string ParseNextCell(string line, ref int i)
        {
            if (i >= line.Length)
                return null;

            if (line[i] != m_config.QuotationMark)
                return ParseNotEscapedCell(line, ref i);
            else
                return ParseEscapedCell(line, ref i);
        }

        // returns iterator after delimiter or after end of string
        private string ParseNotEscapedCell(string line, ref int i)
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                if (i >= line.Length)
                {
                    // return iterator after end of string
                    break;
                }
                if (line[i] == m_config.Delimiter)
                {
                    i++; // return iterator after delimiter
                    break;
                }
                sb.Append(line[i]);
                i++;
            }
            return sb.ToString();
        }

        // returns iterator after delimiter or after end of string
        private string ParseEscapedCell(string line, ref int i)
        {
            i++; // omit first character (quotation mark)
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                if (i >= line.Length)
                    break;
                if (line[i] == m_config.QuotationMark)
                {
                    i++; // we're more interested in the next character
                    if (i >= line.Length)
                    {
                        // quotation mark was closing cell;
                        // return iterator after end of string
                        break;
                    }
                    if (line[i] == m_config.Delimiter)
                    {
                        // quotation mark was closing cell;
                        // return iterator after delimiter
                        i++;
                        break;
                    }
                    if (line[i] == m_config.QuotationMark)
                    {
                        // it was doubled (escaped) quotation mark;
                        // do nothing -- we've already skipped first quotation mark
                    }

                }
                sb.Append(line[i]);
                i++;
            }

            return sb.ToString();
        }
    }
}
