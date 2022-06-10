using System.ComponentModel.DataAnnotations;

namespace PaymentApi.Models
{
    public class AccountInfo
    {
        [Key]
        [Required]
        public int AccountNumber { get; set; }

        public CurrencyCode CurrencyCodes { get; set; }

        [StringLength(40, ErrorMessage = "Please enter a value under 40 characters.")]
        [Required(ErrorMessage = "Owner name is required.")]
        public string OwnerName { get; set; }

        public AccountType AccountTypes { get; set; }

        public decimal Balance { get; set; }//2 decimal yapmak lazım

        public void Deposit(int amount)
        {
            if (amount <= 0)
            {
                throw new Exception();
            }
            else
            {
                Balance += amount;
            }

        }

        public void Withdraw(int amount)
        {
            if (Balance < amount)
            {
                throw new Exception();
            }
            else
            {
                Balance -= amount;
            }

        }
    }
}
