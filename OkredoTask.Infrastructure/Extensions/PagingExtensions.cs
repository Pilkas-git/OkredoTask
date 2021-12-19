﻿using System.Linq;

namespace OkredoTask.Infrastructure.Extensions
{
    public static class PagingExtensions
    {
        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}