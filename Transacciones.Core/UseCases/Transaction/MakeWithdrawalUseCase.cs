using AutoMapper;
using Microsoft.Extensions.Logging;
using Transacciones.Core.Entities.Account;
using Transacciones.Core.Entities.Transaction;
using Transacciones.Core.Interfaces;
using Transacciones.Core.Interfaces.Transaction;
using Transacciones.Core.Models.Transaction.MakeWithdrawal;

namespace Transacciones.Core.UseCases.Transaction
{
    public class MakeWithdrawalUseCase : IMakeWithdrawalUseCase
    {
        private readonly IRepository<Accounts> _accountRepository;
        private readonly IReadRepository<Accounts> _readAccountRepository;
        private readonly IRepository<Transactions> _transactionRepository;
        private readonly IReadRepository<Transactions> _readTransactionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MakeWithdrawalUseCase> _logger;

        public MakeWithdrawalUseCase(
            IRepository<Accounts> accountRepository,
            IReadRepository<Accounts> readAccountRepository,
            IRepository<Transactions> transactionRepository,
            IReadRepository<Transactions> readTransactionRepository,
            IMapper mapper,
            ILogger<MakeWithdrawalUseCase> logger)
        {
            _accountRepository = accountRepository;
            _readAccountRepository = readAccountRepository;
            _transactionRepository = transactionRepository;
            _readTransactionRepository = readTransactionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<MakeWithdrawalResponse> ExecuteAsync(MakeWithdrawalRequest request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Processing withdrawal with request: {@Request}", request);

            var account = await _readAccountRepository.GetByIdAsync(request.AccountId, cancellationToken);
            if (account == null)
            {
                _logger.LogWarning("Withdrawal failed. Account with ID {AccountId} was not found.", request.AccountId);
                throw new KeyNotFoundException("La cuenta especificada no existe.");
            }

            if (!account.IsActive)
            {
                _logger.LogWarning("Withdrawal failed. Account with ID {AccountId} is inactive.", request.AccountId);
                throw new InvalidOperationException("No se pueden realizar transacciones en una cuenta inactiva o bloqueada.");
            }

            if (account.Balance < request.Amount)
            {
                _logger.LogWarning("Withdrawal failed. Insufficient funds in account {AccountId}. Current balance: {Balance}, Requested amount: {Amount}", request.AccountId, account.Balance, request.Amount);
                throw new InvalidOperationException("Fondos insuficientes para realizar el retiro. El balance no puede ser negativo.");
            }

            var previousBalance = account.Balance;
            var newBalance = previousBalance - request.Amount;

            account.Balance = newBalance;
            account.UpdatedAt = DateTime.UtcNow;

            var transaction = _mapper.Map<Transactions>(request);
            transaction.PreviousBalance = previousBalance;
            transaction.NewBalance = newBalance;

            await _transactionRepository.AddAsync(transaction, cancellationToken);
            await _accountRepository.UpdateAsync(account, cancellationToken);

            _logger.LogInformation("Withdrawal of {Amount} successfully made from account {AccountId}", request.Amount, request.AccountId);

            return _mapper.Map<MakeWithdrawalResponse>(transaction);
        }
    }
}
