namespace Transacciones.Core.Entities.Account
{
    public class Accounts
    {
        public required Guid Id { get; set; }
        public required string AccountNumber { get; set; }
        public required double Balance { get; set; }
        public required string Holder { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required bool IsActive { get; set; } = true;
        public DateTime UpdatedAt { get; set; }
    }
}
