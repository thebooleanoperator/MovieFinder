using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieFinder.Models
{
    public class Synopsis
    {
        public int SynopsisId { get; set; }
        public string SynopsisSummary { get; set; }
        public int MovieId { get; set; }
    }
}
