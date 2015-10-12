﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EPiServer;
using EPiServer.Commerce.Catalog;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.SpecializedProperties;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Inventory;
using Mediachase.Commerce.Markets;

namespace BrilliantCut.Core.Extensions
{
    public static class ContentExtensions
    {
        public static string DefaultCurrency(this CatalogContentBase content)
        {
            var catalogContent = content as CatalogContent;
            if (catalogContent != null)
            {
                return catalogContent.DefaultCurrency;
            }

            return null;
        }

        public static string LengthBase(this CatalogContentBase content)
        {
            var catalogContent = content as CatalogContent;
            if (catalogContent != null)
            {
                return catalogContent.LengthBase;
            }

            return null;
        }

        public static string WeightBase(this CatalogContentBase content)
        {
            var catalogContent = content as CatalogContent;
            if (catalogContent != null)
            {
                return catalogContent.WeightBase;
            }

            return null;
        }

        public static string LanguageName(this ILocale content)
        {
            return content.Language.Name;
        }

        public static IEnumerable<ContentReference> ProductLinks(this CatalogContentBase content)
        {
            var variationContent = content as VariationContent;
            if (variationContent == null)
            {
                return Enumerable.Empty<ContentReference>();
            }

            return variationContent.GetParentProducts();
        }

        public static IEnumerable<ContentReference> VariationLinks(this CatalogContentBase content)
        {
            var productContent = content as ProductContent;
            if (productContent == null)
            {
                return Enumerable.Empty<ContentReference>();
            }

            return productContent.GetVariants();
        }

        public static IEnumerable<ContentReference> NodeLinks(this CatalogContentBase content)
        {
            var productContent = content as EPiServer.Commerce.Catalog.ContentTypes.ICategorizable;
            if (productContent == null)
            {
                return Enumerable.Empty<ContentReference>();
            }

            return productContent.GetNodeRelations().Select(x => x.Target);
        }

        public static IEnumerable<string> CategoryNames(this CatalogContentBase content)
        {
            var productContent = content as EPiServer.Commerce.Catalog.ContentTypes.ICategorizable;
            if (productContent == null)
            {
                return Enumerable.Empty<string>();
            }

            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            var contentLinks = productContent.GetNodeRelations().Select(x => x.Target);

            var contentItems = contentLoader.GetItems(contentLinks, content.Language);
            return contentItems.Select(x => x.Name);
        }

        public static int? MetaClassId(this CatalogContentBase content)
        {
            var nodeContent = content as NodeContent;
            if (nodeContent != null)
            {
                return nodeContent.MetaClassId;
            }

            var entryContent = content as EntryContentBase;
            if (entryContent != null)
            {
                return entryContent.MetaClassId;
            }

            return default(int?);
        }

        public static IEnumerable<Price> Prices(this CatalogContentBase content)
        {
            var pricing = content as IPricing;
            if (pricing == null)
            {
                return Enumerable.Empty<Price>();
            }

            return pricing.GetPrices() ?? Enumerable.Empty<Price>();
        }

        public static double? DefaultPrice(this CatalogContentBase content)
        {
            var pricing = content as IPricing;
            if (pricing == null)
            {
                return default(double?);
            }

            var price = pricing.GetDefaultPrice();
            if (price == null)
            {
                return default(double?);
            }

            return Convert.ToDouble(price.UnitPrice.Amount);
        }

        public static double? TotalInStock(this VariationContent content)
        {
            var warehouseInventoryService = ServiceLocator.Current.GetInstance<IWarehouseInventoryService>();

            var allWarehouses = warehouseInventoryService.ListAll();
            if (!allWarehouses.Any())
            {
                return default(double?);
            }

            var totalInStock = warehouseInventoryService.ListAll()
                .Where(x => x.CatalogKey.CatalogEntryCode == content.Code)
                .Select(x => x.InStockQuantity)
                .Sum();

            return Convert.ToDouble(totalInStock);
        }

        public static IEnumerable<Inventory> Inventories(this CatalogContentBase content)
        {
            var stockPlacement = content as VariationContent;
            if (stockPlacement == null)
            {
                return Enumerable.Empty<Inventory>();
            }

            var warehouseInventoryService = ServiceLocator.Current.GetInstance<IWarehouseInventoryService>();
            return warehouseInventoryService.ListAll()
                .Where(x => x.CatalogKey.CatalogEntryCode == stockPlacement.Code)
                .Select(x => new Inventory(x))
                .ToArray();
        }

        public static string Code(this CatalogContentBase content)
        {
            var entryContentBase = content as EntryContentBase;
            if (entryContentBase != null)
            {
                return entryContentBase.Code;
            }

            var nodeContent = content as NodeContent;
            if (nodeContent != null)
            {
                return nodeContent.Code;
            }

            return null;
        }

        public static string ThumbnailPath(this CatalogContentBase content)
        {
            var assetContainer = content as IAssetContainer;
            if (assetContainer == null)
            {
                return null;
            }

            return ServiceLocator.Current.GetInstance<ThumbnailUrlResolver>()
                .GetThumbnailUrl(assetContainer, "Thumbnail");
        }

        public static IEnumerable<string> SelectedMarkets(this EntryContentBase content)
        {
            var marketService = ServiceLocator.Current.GetInstance<IMarketService>();
            return marketService.GetAllMarkets().Select(x => x.MarketId.Value).Except(content.MarketFilter);
        }
    }
}
