

namespace Domain.Repositories;

public interface IRepositoryManager
{
    IUserRepository UserRepository {get;}
    IKeycloakRepository KeycloakRepository {get;}
    IArrangementRepository ArrangementRepository {get;}
    ICommentRepository CommentRepository { get; }
    IAccomodationRepository AccomodationRepository {get;}
    IReservationRepository ReservationRepository {get;}
}
