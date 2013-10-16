using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oba30.Models
{
    public class JqInViewModel
    {
        /// <summary>
        /// Number of records to fetch
        /// </summary>
        public int rows { get; set; }

        /// <summary>
        /// The Page Index
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// Sort column name
        /// </summary>
        public string sidx { get; set; }

        /// <summary>
        /// Sort order "asc" or "desc"
        /// </summary>
        public string sord { get; set; }
    }
}