using AutoMapper;
using Microsoft.Extensions.Logging;
using Transacciones.Core.Entities.Account;
using Transacciones.Core.Entities.Account.Specifications;
using Transacciones.Core.Interfaces;
using Transacciones.Core.Interfaces.Account;
using Transacciones.Core.Models.Account.CreateAccount;

namespace Transacciones.Core.UseCases.Account;

public class CreateAccountUseCase : ICreateAccountUseCase
{
    private readonly IRepository<Accounts> _repository;
    private readonly IReadRepository<Accounts> _readRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateAccountUseCase> _logger;

    public CreateAccountUseCase(
        IRepository<Accounts> repository, 
        IReadRepository<Accounts> readRepository,
        IMapper mapper,
        ILogger<CreateAccountUseCase> logger)
    {
        _repository = repository;
        _readRepository = readRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CreateAccountResponse> ExecuteAsync(CreateAccountRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating new account with request: {@Request}", request);

        var spec = new AccountByNumberSpec(request.AccountNumber);
        var exists = await _readRepository.AnyAsync(spec, cancellationToken);
        
        if (exists)
        {
            _logger.LogWarning("Failed to create account. Account number {AccountNumber} already exists.", request.AccountNumber);
            throw new InvalidOperationException($"Ya existe una cuenta con el número de cuenta {request.AccountNumber}");
        }

        var account = _mapper.Map<Accounts>(request);

        await _repository.AddAsync(account, cancellationToken);

        _logger.LogInformation("Account {AccountNumber} created successfully.", request.AccountNumber);

        return _mapper.Map<CreateAccountResponse>(account);
    }
}
