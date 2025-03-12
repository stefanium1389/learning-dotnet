using System;
using Domain.Entities;
using Persistence.Repositories.Extensions.Utility;
using Shared.RequestFeatures;
using System.Linq.Dynamic.Core;
using Domain.Exceptions;

namespace Persistence.Repositories.Extensions;

public static class AccomodationRepositoryExtensions
{

    public static IQueryable<AccomodationUnit> Paginate(this IQueryable<AccomodationUnit> units, RequestParameters requestParameters)
    {
        return Paginator.Paginate(units, requestParameters);
    }

    public static IQueryable<Accomodation> Paginate(this IQueryable<Accomodation> accomodations, RequestParameters requestParameters)
    {
        return Paginator.Paginate(accomodations, requestParameters);
    }

    public static IQueryable<AccomodationUnit> Filter(this IQueryable<AccomodationUnit> units, AccomodationUnitParameters requestParameters)
    {
        var query = units.Where(u => u.MaximumGuests >= requestParameters.MinGuests && u.MaximumGuests <= requestParameters.MaxGuests)
                    .Where(u => u.Price >= requestParameters.MinPrice && u.Price <= requestParameters.MaxPrice);
        if(requestParameters.PetsAllowed is not null)
        {
            query = query.Where(u => u.PetsAllowed == requestParameters.PetsAllowed);
        }
        if(requestParameters.IsBooked is not null)
        {
            query = query.Where(u => u.IsBooked == requestParameters.IsBooked);
        }
        return query;
    }
    public static IQueryable<AccomodationUnit> Sort(this IQueryable<AccomodationUnit> units, string orderByQueryString)
    {
        if(string.IsNullOrWhiteSpace(orderByQueryString))
            return units.OrderByDescending(u => u.Price);
        var orderQuery = OrderQueryBuilder.CreateOrderQuery<AccomodationUnit>(orderByQueryString);
        if(string.IsNullOrWhiteSpace(orderQuery))
            return units.OrderByDescending(u => u.Price);
        return units.OrderBy(orderQuery);
    }
    public static IQueryable<Accomodation> Sort(this IQueryable<Accomodation> accomodatons, string orderByQueryString)
    {
        if(string.IsNullOrWhiteSpace(orderByQueryString))
            return accomodatons.OrderByDescending(a => a.AccomodationUnits.Count());
        var orderQuery = OrderQueryBuilder.CreateOrderQuery<AccomodationUnit>(orderByQueryString);
        if(string.IsNullOrWhiteSpace(orderQuery))
            return accomodatons.OrderByDescending(a => a.AccomodationUnits.Count());
        return accomodatons.OrderBy(orderQuery);
    }
    public static IQueryable<Accomodation> Search(this IQueryable<Accomodation> accomodations, AccomodationParameters requestParameters)
    {
        var query = accomodations.Where(a => a.Name.ToLower().Contains(requestParameters.Name.ToLower()));
        if (requestParameters.AccomodationType != string.Empty)
        {
            try
            {
                AccomodationType type = Enum.Parse<AccomodationType>(requestParameters.AccomodationType);
                query = query.Where(a => a.AccomodationType == type);
            }
            catch
            {
                throw new InvalidSearchTermException(requestParameters.AccomodationType, "AccomodationType");
            }
        }
        if(requestParameters.HasPool is not null)
        {
            query = query.Where(u => u.HasPool == requestParameters.HasPool);
        }
        if(requestParameters.HasSpa is not null)
        {
            query = query.Where(u => u.HasSpa == requestParameters.HasSpa);
        }
        if(requestParameters.HasWifi is not null)
        {
            query = query.Where(u => u.HasWifi == requestParameters.HasWifi);
        }
        if(requestParameters.DisabledFriendly is not null)
        {
            query = query.Where(u => u.DisabledFriendly == requestParameters.DisabledFriendly);
        }
        return query;
    }

}
