using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OmegaSoftTest.Domain;
using OmegaSoftTest.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSoftTest.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IConfiguration _configuration;
        private IUserService _userService;

        public IdentityService(JwtSettings jwtSettings, TokenValidationParameters tokenValidationParameters, IConfiguration configuration, IUserService userService)
        {
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _configuration = configuration;
            _userService = userService;
        }

        public async Task<AuthenticationResult> RegisterAsync(string email, string password, int roleId)
        {
            bool existingUser = await _userService.UserExists(email);

            if (existingUser)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User with this email address already exists" }
                };
            }

            var newSalt = GenerateSalt();
            var newUser = new User
            {
                Email = email,
                Salt = newSalt,
                PasswordHash = GenerateHash(password, SaltToByte(newSalt)),
                RoleID = roleId
            };

            var createdUser = await _userService.AddUser(newUser);

            if (createdUser < 0)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "Error creating the user" }
                };
            }

            return new AuthenticationResult()
            {
                Success = true,
                Token = GenerateToken(newUser)
            };
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var userExists = await _userService.UserExists(email);

            if (!userExists)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new[] { "User does not exist" }
                };
            }
            var user = await _userService.GetUserByEmailAsync(email);

            var hashedPassword = GenerateHash(password, SaltToByte(user.Salt));
            var valid = hashedPassword == user.PasswordHash;

            if (valid)
                return new AuthenticationResult()
                {
                    Success = true,
                    Token = GenerateToken(user)
                };

  
            return new AuthenticationResult
            {
                Success = false,
                Errors = new[] { "Wrong user name or password" }
            };
        }
        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("role", _userService.GetUserRole(user.Email))
            };

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.Now.Add(_jwtSettings.TokenLifetime),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateSalt()
        {
            var salt = new byte[32];
            var randomProvider = new RNGCryptoServiceProvider();
            randomProvider.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }

        private byte[] SaltToByte(string salt)
        {
            var byteSalt = Convert.FromBase64String(salt);
            return byteSalt;
        }

        private string GenerateHash(string password, byte[] salt)
        {
            var rfc2898 = new Rfc2898DeriveBytes(password, salt);
            var Password = rfc2898.GetBytes(32);
            return Convert.ToBase64String(Password);
        }

        private bool AuthenticateUser(string enteredPassword, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 10);
            return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(32)) == storedHash;
        }
    }

}
        //public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        //{
        //    var validatedToken = GetPrincipalFromToken(token);

        //    if (validatedToken == null)
        //    {
        //        return new AuthenticationResult { Errors = new[] { "Invalid Token" } };
        //    }

        //    var expiryDateUnix =
        //        long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

        //    var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        //        .AddSeconds(expiryDateUnix);

        //    if (expiryDateTimeUtc > DateTime.UtcNow)
        //    {
        //        return new AuthenticationResult { Errors = new[] { "This token hasn't expired yet" } };
        //    }

        //    var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

        //    var storedRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);

        //    if (storedRefreshToken == null)
        //    {
        //        return new AuthenticationResult { Errors = new[] { "This refresh token does not exist" } };
        //    }

        //    if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
        //    {
        //        return new AuthenticationResult { Errors = new[] { "This refresh token has expired" } };
        //    }

        //    if (storedRefreshToken.Invalidated)
        //    {
        //        return new AuthenticationResult { Errors = new[] { "This refresh token has been invalidated" } };
        //    }

        //    if (storedRefreshToken.Used)
        //    {
        //        return new AuthenticationResult { Errors = new[] { "This refresh token has been used" } };
        //    }

        //    if (storedRefreshToken.JwtId != jti)
        //    {
        //        return new AuthenticationResult { Errors = new[] { "This refresh token does not match this JWT" } };
        //    }

        //    storedRefreshToken.Used = true;
        //    _context.RefreshTokens.Update(storedRefreshToken);
        //    await _context.SaveChangesAsync();

        //    var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);
        //    return await GenerateAuthenticationResultForUserAsync(user);
        //}

        //private ClaimsPrincipal GetPrincipalFromToken(string token)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();

        //    try
        //    {
        //        var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
        //        if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
        //        {
        //            return null;
        //        }

        //        return principal;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        //private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        //{
        //    return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
        //           jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
        //               StringComparison.InvariantCultureIgnoreCase);
        //}

        //private string GenerateJWT(User user) {
        //    var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        //    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //};

