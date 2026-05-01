using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Transacciones.Core.Entities.Account;
using Transacciones.Core.Entities.Transaction;
using Transacciones.Core.Interfaces;
using Transacciones.Core.Models.Transaction.MakeWithdrawal;
using Transacciones.Core.UseCases.Transaction;

namespace Transacciones.Tests.UseCases.Transaction;

public class MakeWithdrawalUseCaseTests
{
    private readonly IRepository<Accounts> _accountRepository;
    private readonly IReadRepository<Accounts> _readAccountRepository;
    private readonly IRepository<Transactions> _transactionRepository;
    private readonly IReadRepository<Transactions> _readTransactionRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<MakeWithdrawalUseCase> _logger;
    private readonly MakeWithdrawalUseCase _useCase;

    public MakeWithdrawalUseCaseTests()
    {
        _accountRepository = Substitute.For<IRepository<Accounts>>();
        _readAccountRepository = Substitute.For<IReadRepository<Accounts>>();
        _transactionRepository = Substitute.For<IRepository<Transactions>>();
        _readTransactionRepository = Substitute.For<IReadRepository<Transactions>>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<MakeWithdrawalUseCase>>();
        _useCase = new MakeWithdrawalUseCase(
            _accountRepository,
            _readAccountRepository,
            _transactionRepository,
            _readTransactionRepository,
            _mapper,
            _logger);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldProcessWithdrawal_WhenAccountExistsIsActiveAndHasEnoughBalance()
    {
        var accountId = Guid.NewGuid();
        var request = new MakeWithdrawalRequest(accountId, 500, "Retiro inicial");
        var account = new Accounts 
        { 
            Id = accountId, 
            AccountNumber = "1234567890", 
            Balance = 1000, 
            Holder = "Andres Guerrero", 
            CreatedAt = DateTime.UtcNow, 
            IsActive = true 
        };
        var transaction = new Transactions 
        { 
            AccountId = accountId, 
            TransactionType = "RETIRO",
            Amount = 500, 
            Description = "Retiro inicial",
            PreviousBalance = 1000, 
            NewBalance = 500,
            TransactionDate = DateTime.UtcNow
        };
        var response = new MakeWithdrawalResponse(
            Guid.NewGuid(), 
            accountId, 
            "RETIRO", 
            500, 
            DateTime.UtcNow, 
            "Retiro inicial", 
            1000, 
            500);

        _readAccountRepository.GetByIdAsync(accountId, Arg.Any<CancellationToken>()).Returns(account);
        _mapper.Map<Transactions>(request).Returns(transaction);
        _mapper.Map<MakeWithdrawalResponse>(transaction).Returns(response);

        var result = await _useCase.ExecuteAsync(request);

        result.Should().NotBeNull();
        account.Balance.Should().Be(500);
        await _transactionRepository.Received(1).AddAsync(Arg.Any<Transactions>(), Arg.Any<CancellationToken>());
        await _accountRepository.Received(1).UpdateAsync(account, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowKeyNotFoundException_WhenAccountDoesNotExist()
    {
        var accountId = Guid.NewGuid();
        var request = new MakeWithdrawalRequest(accountId, 500, "Retiro inicial");
        _readAccountRepository.GetByIdAsync(accountId, Arg.Any<CancellationToken>()).Returns((Accounts?)null);
        Func<Task> act = async () => await _useCase.ExecuteAsync(request);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("La cuenta especificada no existe.");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowInvalidOperationException_WhenAccountIsInactive()
    {
        var accountId = Guid.NewGuid();
        var request = new MakeWithdrawalRequest(accountId, 500, "Retiro inicial");
        var account = new Accounts 
        { 
            Id = accountId, 
            AccountNumber = "1234567890", 
            Balance = 1000, 
            Holder = "Andres Guerrero", 
            CreatedAt = DateTime.UtcNow, 
            IsActive = false 
        };
        _readAccountRepository.GetByIdAsync(accountId, Arg.Any<CancellationToken>()).Returns(account);

        Func<Task> act = async () => await _useCase.ExecuteAsync(request);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("No se pueden realizar transacciones en una cuenta inactiva o bloqueada.");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowInvalidOperationException_WhenAccountHasInsufficientFunds()
    {
        var accountId = Guid.NewGuid();
        var request = new MakeWithdrawalRequest(accountId, 1500, "Retiro inicial");
        var account = new Accounts 
        { 
            Id = accountId, 
            AccountNumber = "1234567890", 
            Balance = 1000, 
            Holder = "Andres Guerrero", 
            CreatedAt = DateTime.UtcNow, 
            IsActive = true 
        };
        _readAccountRepository.GetByIdAsync(accountId, Arg.Any<CancellationToken>()).Returns(account);

        Func<Task> act = async () => await _useCase.ExecuteAsync(request);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Fondos insuficientes para realizar el retiro. El balance no puede ser negativo.");
    }
}
