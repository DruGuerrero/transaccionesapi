using AutoMapper;
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
        private readonly IMapper _mapper;

        public MakeDepositUseCase(
            IRepository<Accounts> accountRepository,
            IReadRepository<Accounts> readAccountRepository,
            IRepository<Transactions> transactionRepository,
            IReadRepository<Transactions> readTransactionRepository,
            IMapper mapper)
        {
            _accountRepository = accountRepository;
            _readAccountRepository = readAccountRepository;
            _transactionRepository = transactionRepository;
            _readTransactionRepository = readTransactionRepository;
            _mapper = mapper;
        }

        public async Task<MakeDepositResponse> ExecuteAsync(MakeDepositRequest request, CancellationToken cancellationToken = default)
        {
            var account = await _readAccountRepository.GetByIdAsync(request.AccountId, cancellationToken) ?? throw new KeyNotFoundException("La cuenta especificada no existe.");

            if (!account.IsActive)
                throw new InvalidOperationException("No se pueden realizar transacciones en una cuenta inactiva o bloqueada.");

            var previousBalance = account.Balance;
            var newBalance = previousBalance + request.Amount;

            account.Balance = newBalance;
            account.UpdatedAt = DateTime.UtcNow;

            await _accountRepository.UpdateAsync(account, cancellationToken);

            var transaction = _mapper.Map<Transactions>(request);
            transaction.PreviousBalance = previousBalance;
            transaction.NewBalance = newBalance;

            await _transactionRepository.AddAsync(transaction, cancellationToken);

            return _mapper.Map<MakeDepositResponse>(transaction);
        }
    }
}
