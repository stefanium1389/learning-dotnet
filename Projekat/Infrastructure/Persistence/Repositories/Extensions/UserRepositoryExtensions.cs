using Domain.Entities;
using Domain.Exceptions;
using Persistence.Repositories.Extensions.Utility;
using Shared.RequestFeatures;
using System.Linq.Dynamic.Core;

namespace Persistence.Repositories.Extensions;

public static class UserRepositoryExtensions
{
    public static IQueryable<User> Search(this IQueryable<User> users, UserParameters userParameters)
    {
        var usersQuery = users.Where(u => u.Name.ToLower().Contains(userParameters.Name.ToLower()) && u.Lastname.ToLower().Contains(userParameters.Lastname.ToLower()));
        if (userParameters.Role != string.Empty)
        {
            try
            {
                Role role = Enum.Parse<Role>(userParameters.Role);
                usersQuery = usersQuery.Where(u => u.Role == role);
            }
            catch
            {
                throw new InvalidSearchTermException(userParameters.Role, "Role");
            }
        }
        return usersQuery;
    }
    public static IQueryable<User> Paginate(this IQueryable<User> users, RequestParameters requestParameters)
    {
        return Paginator.Paginate(users, requestParameters);
    }
    public static IQueryable<User> Sort(this IQueryable<User> users, string orderByQueryString)
    {
        if(string.IsNullOrWhiteSpace(orderByQueryString))
            return users.OrderBy(u => u.Name);
        var orderQuery = OrderQueryBuilder.CreateOrderQuery<User>(orderByQueryString);
        if(string.IsNullOrWhiteSpace(orderQuery))
            return users.OrderBy(u => u.Name);
        return users.OrderBy(orderQuery);
    }
}
