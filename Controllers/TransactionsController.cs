using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentApi.Context;
using PaymentApi.Models;

namespace PaymentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TransactionsController : ControllerBase
    {
        private readonly PaymentDb _context;

        public TransactionsController(PaymentDb context)
        {
            _context = context;
        }

        // GET: api/Transactions
        [HttpPost("/Deposit")]
        public async Task<ActionResult<IEnumerable<AccountInfo>>> PostDeposit(int accountNumber, int amount)
        {
          if (_context.AccountInfos == null)
          {
              return NotFound();
          }
          var accountInfo = await _context.AccountInfos.FindAsync(accountNumber);
            accountInfo.Deposit(amount);
            _context.Update(accountInfo);

            var depositTransaction = new Transaction
            {
                AccountNumber = accountNumber,
                Amount = amount,
                TransactionDateTime = DateTime.Now,
                TransactionType = TransactionType.Deposit

            };
            _context.Transactions.Add(depositTransaction);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("/Withdraw")]
        public async Task<ActionResult<IEnumerable<AccountInfo>>> PostWithdraw(int accountNumber, int amount)
        {
            if (_context.AccountInfos == null)
            {
                return NotFound();
            }
            var accountInfo = await _context.AccountInfos.FindAsync(accountNumber);
            accountInfo.Withdraw(amount);
            _context.Update(accountInfo);

            var withdrawTransaction = new Transaction
            {
                AccountNumber = accountNumber,
                Amount = amount,
                TransactionDateTime = DateTime.Now,
                TransactionType = TransactionType.Withdraw

            };
            _context.Transactions.Add(withdrawTransaction);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpPost("/Payment")]
        public async Task<ActionResult<IEnumerable<AccountInfo>>> PostPayment(int senderAccount, int receiverAccount, int amount)
        {
            if (_context.AccountInfos == null)
            {
                return NotFound();
            }
            var senderAccountInfo = await _context.AccountInfos.FindAsync(senderAccount);
            var receiverAccountInfo = await _context.AccountInfos.FindAsync(receiverAccount);
            senderAccountInfo.Withdraw(amount);
            receiverAccountInfo.Deposit(amount);
            _context.Update(senderAccountInfo);
            _context.Update(receiverAccountInfo);

            if (senderAccountInfo.AccountTypes == AccountType.individual && receiverAccountInfo.AccountTypes == AccountType.corporate)
            {
                var senderTransaction = new Transaction
                {
                    AccountNumber = senderAccount,
                    Amount = amount,
                    TransactionDateTime = DateTime.Now,
                    TransactionType = TransactionType.Payment

                };
                var receiverTransaction = new Transaction
                {
                    AccountNumber = receiverAccount,
                    Amount = amount,
                    TransactionDateTime = DateTime.Now,
                    TransactionType = TransactionType.Payment

                };
                _context.Transactions.Add(senderTransaction);
                _context.Transactions.Add(receiverTransaction);
                await _context.SaveChangesAsync();

            }
            else
            {
                throw new InvalidOperationException(
                    "The account you choose to send money is not a corporate account. Please select a corporate account to send money.");
            }
            return Ok();
        }


        [HttpGet("/TransactionHistory")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionHistory(int accountNumber)
        {
            if (_context.Account == null)
            {
                return NotFound();
            }


            return await _context.Transactions.Where(transaction => transaction.AccountNumber == accountNumber).ToListAsync();

        }

        private bool TransactionExists(int id)
        {
            return (_context.Transactions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
