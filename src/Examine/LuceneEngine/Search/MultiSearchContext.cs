﻿using System.Collections.Generic;
using System.Linq;
using Examine.LuceneEngine.Indexing;
using Lucene.Net.Search;

namespace Examine.LuceneEngine.Search
{
    public class MultiSearchContext : ISearchContext
    {
        private readonly ISearchContext[] _inner;
        
        public MultiSearchContext(IndexSearcher searcher, ISearchContext[] inner)
        {
            _inner = inner;
            Searcher = searcher;
        }

        public IndexSearcher Searcher { get; }

        public IIndexFieldValueType GetFieldValueType(string fieldName)
        {
            return _inner.Select(cc => cc.GetFieldValueType(fieldName)).FirstOrDefault(type => type != null);
        }
    }
}