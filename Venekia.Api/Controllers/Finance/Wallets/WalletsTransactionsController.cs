using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Venekia.Application.DTOs.Security;
using Venekia.Application.Interfaces.Finance.Wallets;

namespace Venekia.Api.Controllers.Finance.Wallets
{
    [ApiController]
    [Route("api/wallets")]
    [Authorize]
    public class WalletsTransactionsController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletsTransactionsController (IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet ("{walletId:guid}")]
        public async Task <IActionResult> GetTransactionsHistory(Guid walletId)
        {
            var userId = GetUserClaims().Id;
            var response = await _walletService.GetTransactionHistoryAsync(userId, walletId );

            return Ok(new
            {
                message = "Get all Transactions successfully",
                response
            });
        }

        [HttpGet("{walletId:guid}/{transactionId:guid}")]
        public async Task <IActionResult> GetTransactionsById ( Guid walletId, Guid transactionId)
        {
            var userId = GetUserClaims().Id;

            var response = await _walletService.GetTransactionByIdAsync(userId, walletId, transactionId);

            return Ok(new
            {
                message = $"Get WalletId Transaction by Id successfully",
                response
            });
        }
        
        protected UserClaims GetUserClaims()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (id is null || email is null)
                throw new UnauthorizedAccessException("Invalid Token");

            return new UserClaims
            {
                Id = Guid.Parse(id),
                Email = email
            };
        }
    }
}
