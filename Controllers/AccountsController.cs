using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentApi.Context;
using PaymentApi.Models;

namespace PaymentApi
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AccountsController : ControllerBase
    {
        private readonly PaymentDb _context;

        public AccountsController(PaymentDb context)
        {
            _context = context;
        }

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
            if (_context.Account == null)
            {
                return Problem("Entity set 'PaymentDb.Account'  is null.");
            }
            var accountInfo = new AccountInfo
            {
                AccountNumber = account.AccountNumber,
                AccountTypes = account.AccountTypes,
                CurrencyCodes = account.CurrencyCodes,
                OwnerName = account.OwnerName,
            };
            _context.AccountInfos.Add(accountInfo);
            _context.Account.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccount", new { id = account.AccountNumber }, account);
        }


        // GET: api/Accounts/5
        [HttpGet("/Account/{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
          if (_context.Account == null)
          {
              return NotFound();
          }
            var account = await _context.Account.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }


        [HttpGet("/AccountInfos")]
        public async Task<ActionResult<AccountInfo>> GetAccountInfo(int id)
        {
            if (_context.Account == null)
            {
                return NotFound();
            }
            var accountInfos = await _context.AccountInfos.FindAsync(id);

            if (accountInfos == null)
            {
                return NotFound();
            }

            return accountInfos;
        }




        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(int id, Account account)
        {
            if (id != account.AccountNumber)
            {
                return BadRequest();
            }

            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            if (_context.Account == null)
            {
                return NotFound();
            }
            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Account.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(int id)
        {
            return (_context.Account?.Any(e => e.AccountNumber == id)).GetValueOrDefault();
        }
    }
}
