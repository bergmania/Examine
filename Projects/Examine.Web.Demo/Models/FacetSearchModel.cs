using System.Diagnostics;
using Examine.LuceneEngine;
using Examine.LuceneEngine.Faceting;

namespace Examine.Web.Demo.Models
{
    public class RebuildModel
    {
        public int TotalIndexed { get; set; }
        public double TotalSeconds { get; set; }
    }

    public class FacetSearchModel
    {
        public bool CountFacets { get; set; }
        public ILuceneSearchResults SearchResult { get; set; }
        public Stopwatch Watch { get; set; }
        public FacetMap FacetMap { get; set; }
    }
}