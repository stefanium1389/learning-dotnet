using System;

namespace Service.Abstractions;

public interface IServiceManager
{
    public IUserService UserService {get;}
    public IArrangementService ArrangementService {get;}
    public IAccomodationService AccomodationService {get;}
    public IReservationService ReservationService {get;}
    public ICommentService CommentService {get;}
}