using System.ComponentModel.DataAnnotations;

namespace PaymentApi.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public int AccountNumber { get; set; }

        public int Amount { get; set; }

        public TransactionType TransactionType { get; set; }

        public DateTime TransactionDateTime { get; set; }
    }
}
