using AutoMapper;
using Transacciones.Core.Entities.Account;
using Transacciones.Core.Interfaces;
using Transacciones.Core.Interfaces.Account;
using Transacciones.Core.Models.Account.CreateAccount;

namespace Transacciones.Core.UseCases.Account;

public class CreateAccountUseCase : ICreateAccountUseCase
{
    private readonly IRepository<Accounts> _repository;
    private readonly IMapper _mapper;

    public CreateAccountUseCase(IRepository<Accounts> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CreateAccountResponse> ExecuteAsync(CreateAccountRequest request, CancellationToken cancellationToken = default)
    {
        var account = _mapper.Map<Accounts>(request);

        await _repository.AddAsync(account, cancellationToken);

        return _mapper.Map<CreateAccountResponse>(account);
    }
}
