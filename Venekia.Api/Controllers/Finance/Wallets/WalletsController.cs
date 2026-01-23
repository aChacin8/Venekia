using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Venekia.Application.DTOs.Finance.Wallet;
using Venekia.Application.DTOs.Security;
using Venekia.Application.Interfaces.Finance.Wallets;

namespace Venekia.Api.Controllers.Finance.Wallets
{
    [ApiController]
    [Route ("api/wallets")]
    [Authorize]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService _walletService; 

        public WalletsController (IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpPost]
        public async Task <IActionResult> CreateWallet ([FromBody]CreateWalletDto createWalletDto)
        {
            var userId = GetUserClaims().Id;
            var response = await _walletService.CreateWalletAsync(userId, createWalletDto);
            
            return Ok(new
            {
                message = "Wallet create succesfully",
                response
            });
        }

        [HttpPost ("credit")]
        public async Task <IActionResult> CreditWallet ([FromBody]CreditWalletDto creditWalletDto)
        {
            var userId = GetUserClaims().Id;
            var response = await _walletService.CreditAsync(userId, creditWalletDto);
            
            return Ok(new
            {
                message = "Wallet credited successfully",
                response
            });
        }

        [HttpPost ("debit")]
        public async Task <IActionResult> DebitWallet ([FromBody]DebitWalletDto debitWalletDto)
        {
            var userId = GetUserClaims().Id;
            var response = await _walletService.DebitAsync(userId, debitWalletDto);
            
            return Ok(new
            {
                message = "Wallet debited successfully",
                response
            });
        }

        [HttpGet]
        [Authorize]
        public async Task <IActionResult> GetWalletByUser ()
        {
            var userId = GetUserClaims().Id;
            var response = await _walletService.GetWalletByUserAsync(userId);

            return Ok(new
            {
                message = "Get Wallets successfully",
                response
            });
        }

        private UserClaims GetUserClaims()
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
