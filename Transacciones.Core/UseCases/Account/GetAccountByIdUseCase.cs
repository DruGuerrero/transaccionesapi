using AutoMapper;
using Transacciones.Core.Entities.Account;
using Transacciones.Core.Entities.Account.Specifications;
using Transacciones.Core.Interfaces;
using Transacciones.Core.Interfaces.Account;
using Transacciones.Core.Models.Account;

namespace Transacciones.Core.UseCases.Account;

public class GetAccountByIdUseCase : IGetAccountByIdUseCase
{
    private readonly IReadRepository<Accounts> _readRepository;
    private readonly IMapper _mapper;

    public GetAccountByIdUseCase(IReadRepository<Accounts> readRepository, IMapper mapper)
    {
        _readRepository = readRepository;
        _mapper = mapper;
    }

    public async Task<AccountResponse> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var spec = new AccountByIdSpec(id);
        var account = await _readRepository.FirstOrDefaultAsync(spec, cancellationToken) ?? throw new KeyNotFoundException($"No se encontró la cuenta con ID: {id}");
        return _mapper.Map<AccountResponse>(account);
    }
}
