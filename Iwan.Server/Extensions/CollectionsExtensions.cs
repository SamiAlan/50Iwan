using Iwan.Server.Models;
using Iwan.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iwan.Server.Extensions
{
    public static class CollectionsExtensions
    {
        public static ICollection<T> Extend<T>(this ICollection<T> collection1, params ICollection<T>[] collections)
        {
            var finalCollection = new List<T>(collection1);

            foreach (var collection in collections)
                finalCollection.AddRange(collection);

            return finalCollection;
        }

        public static PagedDto<TMapped> AsPaged<TEntity, TMapped>(this IList<TEntity> source, int pageNumber, int pageSize, int total, Func<TEntity, TMapped> factory)
        {
            return new PagedDto<TMapped>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = total,
                HasNext = total > (pageNumber * pageSize),
                HasPrevious = pageNumber == 0,
                Data = source.Select(factory).ToList()
            };
        }

        public static PagedDto<TMapped> AsPaged<TMapped>(this IList<TMapped> sourceData, int pageNumber, int pageSize, int total)
        {
            return new PagedDto<TMapped>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = total,
                HasNext = total > (pageNumber * pageSize),
                HasPrevious = pageNumber != 1,
                Data = sourceData
            };
        }

        public static PagedViewModel<TMapped> AsPagedModel<TEntity, TMapped>(this IList<TEntity> source, int pageNumber, int pageSize, int total, Func<TEntity, TMapped> factory)
        {
            return new PagedViewModel<TMapped>(total, pageNumber, pageSize)
            {
                Data = source.Select(factory).ToList()
            };
        }

        public static PagedViewModel<TMapped> AsPagedModel<TMapped>(this IList<TMapped> sourceData, int pageNumber, int pageSize, int total)
        {
            return new PagedViewModel<TMapped>(total, pageNumber, pageSize)
            {
                Data = sourceData
            };
        }
    }
}
