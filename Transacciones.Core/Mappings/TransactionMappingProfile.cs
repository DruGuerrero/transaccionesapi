using AutoMapper;
using Transacciones.Core.Entities.Transaction;
using Transacciones.Core.Models.Transaction.MakeDeposit;
using Transacciones.Core.Models.Transaction.MakeWithdrawal;

namespace Transacciones.Core.Mappings;

public class TransactionMappingProfile : Profile
{
    public TransactionMappingProfile()
    {
        CreateMap<MakeDepositRequest, Transactions>()
            .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => "ABONO"))
            .ForMember(dest => dest.TransactionDate, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PreviousBalance, opt => opt.Ignore())
            .ForMember(dest => dest.NewBalance, opt => opt.Ignore())
            .ForMember(dest => dest.Account, opt => opt.Ignore());

        CreateMap<Transactions, MakeDepositResponse>();

        CreateMap<MakeWithdrawalRequest, Transactions>()
            .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => "RETIRO"))
            .ForMember(dest => dest.TransactionDate, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PreviousBalance, opt => opt.Ignore())
            .ForMember(dest => dest.NewBalance, opt => opt.Ignore())
            .ForMember(dest => dest.Account, opt => opt.Ignore());

        CreateMap<Transactions, MakeWithdrawalResponse>();
    }
}
