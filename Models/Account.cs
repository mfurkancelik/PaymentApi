using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PaymentApi.Models
{
    public class Account
    {
        [Key]
        [Required]
        public int AccountNumber { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required]
        public CurrencyCode CurrencyCodes { get; set; }

        [StringLength(40, ErrorMessage = "Please enter a value under 40 characters.")]
        [Required]
        public string? OwnerName { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required]
        public AccountType AccountTypes { get; set; }

        public Account()
        {

        }

        public Account(AccountInfo accountInfo)
        {
            this.AccountNumber = accountInfo.AccountNumber;
            this.CurrencyCodes = accountInfo.CurrencyCodes;
            this.OwnerName = accountInfo.OwnerName;
            this.AccountTypes = accountInfo.AccountTypes;
        }
    }
}
