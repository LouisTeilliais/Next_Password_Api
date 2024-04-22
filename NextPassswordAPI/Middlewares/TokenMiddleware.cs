using NextPassswordAPI.Models;
using NextPassswordAPI.Repository.Interfaces;
using NextPassswordAPI.Services.Interfaces;

namespace NextPassswordAPI.Middlewares
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IHashPasswordService _hashPasswordService;
        private readonly IPasswordRepository _passwordRepository;
        private readonly ITokenRepository _tokenRepository;

        public TokenMiddleware(RequestDelegate next,
            IPasswordService passwordService,
            ITokenService tokenService,
            IHashPasswordService hashPasswordService,
            IPasswordRepository passwordRepository,
            ITokenRepository tokenRepository)
        {
            _next = next;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _hashPasswordService = hashPasswordService;
            _passwordRepository = passwordRepository;
            _tokenRepository = tokenRepository;
        }

        /// <summary>
        /// Middleware for security on request GET/:passwordId
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task InvokeAsync(HttpContext context)
        {

            if (context.Request.Method == HttpMethods.Get)
            {
                var path = context.Request.Path.Value;

                if (path.Length > "/api/password".Length)
                {
                    if (Guid.TryParse(path.Substring("/api/password/".Length), out Guid passwordId))
                    {
                        var password = await _passwordService.GetPasswordByIdAsync(passwordId);

                        var token = await _tokenService.FindByIdAsync(password!.TokenId);

                        if (token == null)
                        {
                            throw new Exception("Une erreur est survenue");
                        }

                        if (token.NumberUses > 0)
                        {
                            token.NumberUses--;
                            await _tokenService.UpdateToken(token);
                        }
                        else
                        {
                            // Create new token
                            string tokenValue = _hashPasswordService.GenerateToken();

                            // Decrypt old password
                            var actualPassword = _hashPasswordService.DecryptPassword(password.PasswordHash, token.TokenValue);

                            if (actualPassword == null)
                            {
                                throw new Exception("Failed to decrypt old password");
                            }

                            var passwordHashed = _hashPasswordService.EncryptPassword(actualPassword, tokenValue);

                            if (passwordHashed == null)
                            {
                                throw new Exception("Failed to encrypt new password");
                            }

                            var newToken = new Token
                            {
                                TokenValue = tokenValue,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow,
                                PasswordId = passwordId,
                                NumberUses = 3
                            };

                            await _tokenRepository.AddTokenAsync(newToken);

                            password.PasswordHash = passwordHashed;
                            password.TokenId = newToken.Id;

                            await _passwordRepository.UpdatePasswordTokenAsync(password);
                        }
                    }
                }

            } 
            await _next(context);
        }
    }
}
