﻿using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserAPI.Models.Auth;
using UserAPI.Services.Interfaces;

namespace UserAPI.Controllers
{
    /// <summary>
    /// Handles user authentication and authorization, including login, registration, and retrieving authenticated user details.
    /// </summary>
    /// <remarks>
    /// This controller provides authentication-related endpoints for:
    /// - Logging in and receiving a JWT token.
    /// - Registering a new user.
    /// - Retrieving the authenticated user's details.
    /// </remarks>
    /// <param name="authService">Service responsible for authentication and user management.</param>
    /// <param name="jwtOptions">JWT settings for token generation.</param>
    /// <param name="config">Application configuration settings.</param>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService, IOptions<JwtSettings> jwtOptions, IConfiguration config) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly JwtSettings _jwtSettings = jwtOptions.Value;
        private readonly IConfiguration _config = config;

        /// <summary>
        /// Authenticates a user and generates a JWT token upon successful authentication.
        /// </summary>
        /// <param name="request">The login request containing user credentials (email and password).</param>
        /// <returns>
        /// - <c>200 OK</c>: If authentication is successful, returns a JWT token along with user details.<br/>
        /// - <c>400 Bad Request</c>: If the request model is invalid.<br/>
        /// - <c>401 Unauthorized</c>: If the provided credentials are incorrect.
        /// </returns>
        /// <response code="200">Returns the JWT token and authenticated user details.</response>
        /// <response code="400">If the request data is invalid.</response>
        /// <response code="401">If authentication fails due to incorrect credentials.</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _authService.AuthenticateAsync(request);
            if (user == null)
                return Unauthorized("Invalid username or password.");

            var token = GenerateJwtToken(user);
            return Ok(new
            {
                Token = token,
                ExpiresIn = _jwtSettings.ExpiresInDays,
                User = new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.DateOfBirth,
                    user.Email,
                    user.PhoneNumber,
                    user.Role,
                    user.ImageUrl
                }
            });
        }

        /// <summary>
        /// Registers a new user and stores their credentials securely.
        /// </summary>
        /// <param name="request">The registration request containing user details.</param>
        /// <returns>
        /// - <c>201 Created</c>: If registration is successful.<br/>
        /// - <c>400 Bad Request</c>: If the request data is invalid.<br/>
        /// - <c>500 Internal Server Error</c>: If an unexpected error occurs.
        /// </returns>
        /// <response code="201">Returns the registered user details.</response>
        /// <response code="400">If the provided registration data is invalid.</response>
        /// <response code="500">If an unexpected error occurs.</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            await _authService.RegisterAsync(request);
            return Created(nameof(Register), null);
        }

        /// <summary>
        /// Authenticates an existing user and generates a JWT token upon successful authentication. Otherwise retrieves data from Google's account for registration a new user.
        /// </summary>
        /// <param name="request">Request containing JWT token with Google's account details about user.</param>
        /// <returns>
        /// - <c>201 Created</c>: If registration is successful.<br/>
        /// - <c>400 Bad Request</c>: If the request data is invalid.<br/>
        /// - <c>500 Internal Server Error</c>: If an unexpected error occurs.
        /// </returns>
        /// <response code="200">Returns JWT token with user details for existing user or data about new user.</response>
        /// <response code="400">If the provided registration data is invalid.</response>
        [HttpPost("oauth")]
        public async Task<IActionResult> OAuth([FromBody] OAuthRequest request)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = [_config["OAuth:ClientId"]]
            };

            var user = await _authService.OAuthAsync(request.Token, settings);
            
            if (user.Id.Equals(Guid.Empty))
                return Ok(user);

            var token = GenerateJwtToken(user);
            return Ok(new
            {
                Token = token,
                ExpiresIn = _jwtSettings.ExpiresInDays,
                User = user
            });
        }

        /// <summary>
        /// Generates a JWT token for an authenticated user.
        /// </summary>
        /// <param name="user">The user object for whom the token is generated.</param>
        /// <returns>A JWT token string containing encoded user claims.</returns>
        /// <remarks>
        /// This method generates a JWT token that includes:
        /// - User ID (`NameIdentifier`)
        /// - First name (`Name`)
        /// - Last name (`Surname`)
        /// - User role (`Role`)
        /// </remarks>
        [NonAction]
        private string GenerateJwtToken(Dto user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.FirstName),
                new(ClaimTypes.Surname, user.LastName!),
                new(ClaimTypes.Role, user.Role.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwtSettings.ExpiresInDays),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
