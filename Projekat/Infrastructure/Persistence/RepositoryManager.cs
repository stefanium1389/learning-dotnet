using System;
using Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Persistence.Repositories;

namespace Persistence;

public class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<IUserRepository> _lazyUserRepository;
    private readonly Lazy<IKeycloakRepository> _lazyKeycloakRepository;
    private readonly Lazy<IArrangementRepository> _lazyArrangementRepository;
    private readonly Lazy<ICommentRepository> _lazyCommentRepository;
    private readonly Lazy<IAccomodationRepository> _lazyAccomodationRepository;
    private readonly Lazy<IReservationRepository> _lazyReservationRepository;
    public RepositoryManager(RepositoryDbContext dbContext, IConfiguration configuration)
    {
        _lazyUserRepository = new Lazy<IUserRepository>(() => new UserRepository(dbContext));
        _lazyKeycloakRepository = new Lazy<IKeycloakRepository>(() => new KeycloakRepository(configuration));
        _lazyArrangementRepository = new Lazy<IArrangementRepository>(() => new ArrangementRepository(dbContext));
        _lazyCommentRepository = new Lazy<ICommentRepository>(() => new CommentRepository(dbContext));
        _lazyAccomodationRepository = new Lazy<IAccomodationRepository>(() => new AccomodationRepository(dbContext));
        _lazyReservationRepository = new Lazy<IReservationRepository>(() => new ReservationRepository(dbContext));
    }
    public IUserRepository UserRepository => _lazyUserRepository.Value;
    
    public IKeycloakRepository KeycloakRepository => _lazyKeycloakRepository.Value;

    public IArrangementRepository ArrangementRepository => _lazyArrangementRepository.Value;

    public ICommentRepository CommentRepository => _lazyCommentRepository.Value;

    public IAccomodationRepository AccomodationRepository => _lazyAccomodationRepository.Value;

    public IReservationRepository ReservationRepository => _lazyReservationRepository.Value;
}
