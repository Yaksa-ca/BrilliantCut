﻿using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiTube.FasetFilter.Core.DataAnnotation;

namespace EPiTube.FasetFilter.Core.Filters
{
    [RadiobuttonFilter]
    public class IsActiveFilter : FilterContentBase<CatalogContentBase, string>
    {
        public override string Name
        {
            get { return "Active"; }
        }

        public override ITypeSearch<CatalogContentBase> Filter(IContent currentCntent, ITypeSearch<CatalogContentBase> query, IEnumerable<string> values)
        {
            var value = values.FirstOrDefault();
            if (value == null || value == "all")
            {
                return query;
            }

            return query.CurrentlyPublished().ExcludeDeleted();
        }

        public override IEnumerable<IFilterOptionModel> GetFilterOptions(SearchResults<EPiTubeModel> searchResults)
        {
            yield return new FilterOptionModel("all", "All", false, false, -1);
            yield return new FilterOptionModel("active", "Active", false, false, -1);
            //yield return new FilterOptionModel("unactive", "Unactive", false, false, -1);
        }

        public override ITypeSearch<CatalogContentBase> AddFasetToQuery(ITypeSearch<CatalogContentBase> query)
        {
            return query;
        }
    }
}
