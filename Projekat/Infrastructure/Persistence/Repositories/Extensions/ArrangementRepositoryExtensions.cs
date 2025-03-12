using System;
using Domain.Entities;
using Domain.Exceptions;
using Persistence.Repositories.Extensions.Utility;
using Shared.RequestFeatures;
using System.Linq.Dynamic.Core;

namespace Persistence.Repositories.Extensions;

public static class ArrangementRepositoryExtensions
{
    public static IQueryable<Arrangement> Search(this IQueryable<Arrangement> arrangements, ArrangementParameters arrangementParameters)
    {
        var query = arrangements.Where(a => a.Name.ToLower().Contains(arrangementParameters.Name.ToLower()));
        if (arrangementParameters.ArrangementType != string.Empty)
        {
            try
            {
                ArrangementType type = Enum.Parse<ArrangementType>(arrangementParameters.ArrangementType);
                query = query.Where(a => a.ArrangementType == type);
            }
            catch
            {
                throw new InvalidSearchTermException(arrangementParameters.ArrangementType, "ArrangementType");
            }
        }
        if (arrangementParameters.TransportationType != string.Empty)
        {
            try
            {
                TransportationType type = Enum.Parse<TransportationType>(arrangementParameters.TransportationType);
                query = query.Where(a => a.TransportationType == type);
            }
            catch
            {
                throw new InvalidSearchTermException(arrangementParameters.ArrangementType, "TransportationType");
            }
        }
        return query;
    }
    public static IQueryable<Arrangement> Filter(this IQueryable<Arrangement> arrangements, ArrangementParameters arrangementParameters)
    {
        var query = arrangements.Where(a =>
                        a.StartDate >= arrangementParameters.MinStartDate &&
                        a.StartDate <= arrangementParameters.MaxStartDate &&
                        a.EndDate >= arrangementParameters.MinEndDate &&  
                        a.EndDate <= arrangementParameters.MaxEndDate);  
        return query;
    }

    public static IQueryable<Arrangement> Sort(this IQueryable<Arrangement> arrangements, string orderByQueryString)
    {
        if(string.IsNullOrWhiteSpace(orderByQueryString))
            return arrangements.OrderByDescending(a => a.StartDate);
        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Arrangement>(orderByQueryString);
        if(string.IsNullOrWhiteSpace(orderQuery))
            return arrangements.OrderByDescending(a => a.StartDate);
        return arrangements.OrderBy(orderQuery);
    }

    public static IQueryable<Arrangement> Paginate(this IQueryable<Arrangement> arrangements, RequestParameters requestParameters)
    {
        return Paginator.Paginate(arrangements, requestParameters);
    }
}
