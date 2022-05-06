using AutoMapper;
using BiPapyon.Api.Application.Interfaces.Repositories;
using BiPapyon.Common.Infrastructure;
using BiPapyon.Common.Infrastructure.Exceptions;
using BiPapyon.Common.Models.Queries;
using BiPapyon.Common.Models.RequestModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BiPapyon.Api.Application.Features.Commands.User
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserViewModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public LoginUserCommandHandler(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<LoginUserViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            string encyrptedPassword = PasswordEncryptor.Encrypt(request.Password);

            var user = await _userRepository.GetSingleAsync(x => x.Email == request.Email && x.Password == encyrptedPassword);

            if (user == null)
                throw new ValidationException("User not found with this email and password");

            if (!user.EmailConfirmed)
                throw new ValidationException("Email not confirmed");

            var result = _mapper.Map<LoginUserViewModel>(user);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName)
            };

            result.Token = GenerateToken(claims);

            return result;
        }

        private string GenerateToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthConfig:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(10);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiry,
                signingCredentials: creds,
                notBefore: DateTime.Now
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
