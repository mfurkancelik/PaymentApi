using Microsoft.EntityFrameworkCore;
using PaymentApi.Models;

namespace PaymentApi.Context
{
    public class PaymentDb : DbContext
    {
        public PaymentDb(DbContextOptions<PaymentDb> options)
        : base(options) { }

        //public DbSet<AccountInfo> AccountInfos => Set<AccountInfo>();

        public DbSet<AccountInfo> AccountInfos { get; set; } = null!;
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<PaymentApi.Models.Account>? Account { get; set; }
}
}
