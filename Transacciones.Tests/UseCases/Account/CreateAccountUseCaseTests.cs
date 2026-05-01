using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Transacciones.Core.Entities.Account;
using Transacciones.Core.Interfaces;
using Transacciones.Core.Models.Account.CreateAccount;
using Transacciones.Core.UseCases.Account;
using Ardalis.Specification;
using Microsoft.Extensions.Logging;

namespace Transacciones.Tests.UseCases.Account;

public class CreateAccountUseCaseTests
{
    private readonly IRepository<Accounts> _repository;
    private readonly IReadRepository<Accounts> _readRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateAccountUseCase> _logger;
    private readonly CreateAccountUseCase _createAccountUseCase;

    public CreateAccountUseCaseTests()
    {
        _repository = Substitute.For<IRepository<Accounts>>();
        _readRepository = Substitute.For<IReadRepository<Accounts>>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CreateAccountUseCase>>();
        _createAccountUseCase = new CreateAccountUseCase(_repository, _readRepository, _unitOfWork, _mapper, _logger);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCreateAccount_WhenAccountDoesNotExist()
    {
        var request = new CreateAccountRequest("1234567890", 1000, "Andres Guerrero", true);
        var createdAt = DateTime.UtcNow;
        var account = new Accounts { AccountNumber = request.AccountNumber, Balance = request.Balance, Holder = request.Holder, CreatedAt = createdAt, IsActive = request.IsActive };
        var response = new CreateAccountResponse(Guid.NewGuid(), request.AccountNumber, request.Balance, request.Holder, createdAt, request.IsActive);

        _readRepository.AnyAsync(Arg.Any<ISpecification<Accounts>>(), Arg.Any<CancellationToken>())
            .Returns(false);

        _mapper.Map<Accounts>(request).Returns(account);
        _mapper.Map<CreateAccountResponse>(account).Returns(response);

        var result = await _createAccountUseCase.ExecuteAsync(request);

        result.Should().NotBeNull();
        result.AccountNumber.Should().Be(request.AccountNumber);
        await _repository.Received(1).AddAsync(Arg.Is<Accounts>(a => a.AccountNumber == request.AccountNumber), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowInvalidOperationException_WhenAccountAlreadyExists()
    {
        var request = new CreateAccountRequest("1234567890", 1000, "Andres Guerrero", true);

        _readRepository.AnyAsync(Arg.Any<ISpecification<Accounts>>(), Arg.Any<CancellationToken>())
            .Returns(true);

        Func<Task> act = async () => await _createAccountUseCase.ExecuteAsync(request);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Ya existe una cuenta con el número de cuenta {request.AccountNumber}");
        
        await _repository.DidNotReceive().AddAsync(Arg.Any<Accounts>(), Arg.Any<CancellationToken>());
    }
}
