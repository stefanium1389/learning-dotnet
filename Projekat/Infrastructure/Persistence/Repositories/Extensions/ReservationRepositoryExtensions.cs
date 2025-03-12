using System;
using Domain.Entities;
using Persistence.Repositories.Extensions.Utility;
using Shared.RequestFeatures;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories.Extensions;

public static class ReservationRepositoryExtensions
{
    public static IQueryable<Reservation> Paginate(this IQueryable<Reservation> arrangements, RequestParameters requestParameters)
    {
        return Paginator.Paginate(arrangements, requestParameters);
    }
    public static IQueryable<Reservation> Filter(this IQueryable<Reservation> reservations, ReservationParameters requestParameters)
    {
        var query = reservations.Where(r => r.AccomodationUnit.Price <= requestParameters.MaxPrice && r.AccomodationUnit.Price >= requestParameters.MinPrice)
                                .Where(r => r.Arrangement.StartDate <= requestParameters.MaxStartDate && r.Arrangement.StartDate >= requestParameters.MinStartDate);
        if(requestParameters.Active is not null)
        {
            query = query.Where(r => r.IsActive == requestParameters.Active);
        }
        if(requestParameters.Future is not null)
        {
            query = query.Where(r => (r.Arrangement.StartDate > DateOnly.FromDateTime(DateTime.Now)) == requestParameters.Future.Value);
        }
        return query;
    }
    public static IQueryable<Reservation> Sort(this IQueryable<Reservation> reservations, string orderByQueryString)
    {
        if(string.IsNullOrWhiteSpace(orderByQueryString))
            return reservations.OrderBy(r => r.Arrangement.StartDate);
        string parameter = orderByQueryString.Split(" ")[0].ToLower();
        bool descending = orderByQueryString.ToLower().EndsWith(" desc");
        return parameter switch
        {
            "price" => descending
                            ? reservations.OrderByDescending(r => r.AccomodationUnit.Price)
                            : reservations.OrderBy(r => r.AccomodationUnit.Price),
            "name" => descending
                            ? reservations.OrderByDescending(r => r.Arrangement.Name)
                            : reservations.OrderBy(r => r.Arrangement.Name),
            "startdate" => descending
                            ? reservations.OrderByDescending(r => r.Arrangement.StartDate)
                            : reservations.OrderBy(r => r.Arrangement.StartDate),
            _ => reservations.OrderBy(r => r.Arrangement.StartDate),
        };
    }
}
