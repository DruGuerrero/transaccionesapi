using AutoMapper;
using Microsoft.Extensions.Logging;
using Transacciones.Core.Entities.Account;
using Transacciones.Core.Entities.Transaction;
using Transacciones.Core.Interfaces;
using Transacciones.Core.Interfaces.Transaction;
using Transacciones.Core.Models.Transaction.MakeDeposit;

namespace Transacciones.Core.UseCases.Transaction
{
    public class MakeDepositUseCase : IMakeDepositUseCase
    {
        private readonly IRepository<Accounts> _accountRepository;
        private readonly IReadRepository<Accounts> _readAccountRepository;
        private readonly IRepository<Transactions> _transactionRepository;
        private readonly IReadRepository<Transactions> _readTransactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MakeDepositUseCase> _logger;

        public MakeDepositUseCase(
            IRepository<Accounts> accountRepository,
            IReadRepository<Accounts> readAccountRepository,
            IRepository<Transactions> transactionRepository,
            IReadRepository<Transactions> readTransactionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<MakeDepositUseCase> logger)
        {
            _accountRepository = accountRepository;
            _readAccountRepository = readAccountRepository;
            _transactionRepository = transactionRepository;
            _readTransactionRepository = readTransactionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<MakeDepositResponse> ExecuteAsync(MakeDepositRequest request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Processing deposit with request: {@Request}", request);

            var account = await _readAccountRepository.GetByIdAsync(request.AccountId, cancellationToken);
            if (account == null)
            {
                _logger.LogWarning("Deposit failed. Account with ID {AccountId} was not found.", request.AccountId);
                throw new KeyNotFoundException("La cuenta especificada no existe.");
            }

            if (!account.IsActive)
            {
                _logger.LogWarning("Deposit failed. Account with ID {AccountId} is inactive.", request.AccountId);
                throw new InvalidOperationException("No se pueden realizar transacciones en una cuenta inactiva o bloqueada.");
            }

            var previousBalance = account.Balance;
            var newBalance = previousBalance + request.Amount;

            account.Balance = newBalance;
            account.UpdatedAt = DateTime.UtcNow;

            var transaction = _mapper.Map<Transactions>(request);
            transaction.PreviousBalance = previousBalance;
            transaction.NewBalance = newBalance;

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                await _transactionRepository.AddAsync(transaction, cancellationToken);
                await _accountRepository.UpdateAsync(account, cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing deposit transaction for account {AccountId}", request.AccountId);
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }

            _logger.LogInformation("Deposit of {Amount} successfully made to account {AccountId}", request.Amount, request.AccountId);

            return _mapper.Map<MakeDepositResponse>(transaction);
        }
    }
}
