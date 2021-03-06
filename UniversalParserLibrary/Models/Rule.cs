﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Models
{
    /// <summary>
    /// Class with rules for trainig
    /// </summary>
    internal class Rule
    {
        public string _id { get; set; } = Convert.ToString(Guid.NewGuid());

        public List<Tuple<string, string>> rules = new List<Tuple<string, string>>();
    }
}
