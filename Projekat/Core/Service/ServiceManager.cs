using System;
using Domain.Repositories;
using Service.Abstractions;

namespace Service;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IUserService> _lazyUserService;
    private readonly Lazy<IArrangementService> _lazyArrangementService;
    private readonly Lazy<IAccomodationService> _lazyAccomodationService;
    private readonly Lazy<IReservationService> _lazyReservationService;
    private readonly Lazy<ICommentService> _lazyCommentService;

    public ServiceManager(IRepositoryManager repositoryManager)
    {
        _lazyUserService = new Lazy<IUserService>(()=> new UserService(repositoryManager));
        _lazyArrangementService = new Lazy<IArrangementService>(()=> new ArrangementService(repositoryManager));
        _lazyAccomodationService = new Lazy<IAccomodationService>(()=> new AccomodationService(repositoryManager));
        _lazyReservationService = new Lazy<IReservationService>(()=> new ReservationService(repositoryManager));
        _lazyCommentService = new Lazy<ICommentService>(()=> new CommentService(repositoryManager));
    }

    public IUserService UserService => _lazyUserService.Value;
    public IArrangementService ArrangementService => _lazyArrangementService.Value;
    public IAccomodationService AccomodationService => _lazyAccomodationService.Value;
    public IReservationService ReservationService => _lazyReservationService.Value;
    public ICommentService CommentService => _lazyCommentService.Value;
}
