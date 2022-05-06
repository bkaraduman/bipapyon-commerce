using AutoMapper;
using BiPapyon.Api.Application.Interfaces.Repositories;
using BiPapyon.Common;
using BiPapyon.Common.Events;
using BiPapyon.Common.Infrastructure;
using BiPapyon.Common.Infrastructure.Exceptions;
using BiPapyon.Common.Models.RequestModels;
using BiPapyon.Common.ViewModels;
using MediatR;

namespace BiPapyon.Api.Application.Features.Commands.User.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public CreateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existUser = await userRepository.GetSingleAsync(x => x.Email == request.Email);

            if (existUser is not null)
                throw new ValidationException("User already exists");

            request.Password = PasswordEncryptor.Encrypt(request.Password);

            var dbUser = mapper.Map<Domain.Models.User>(request);

            dbUser.Status = (int)DatabaseStatus.Active;

            var rows = await userRepository.AddAsync(dbUser);

            if (rows > 0)
            {
                var @event = new UserEmailChangedEvent
                {
                    OldEmail = null,
                    NewEmail = request.Email
                };

                QueueFactory.SendMessageToExchange(
                    exchangeName: BiPapyonConstants.UserExchangeName,
                    exchangeType: BiPapyonConstants.DefaultExchangeType,
                    queueName: BiPapyonConstants.UserEmailChangedQueueName,
                    obj: @event);
            }

            return dbUser.Id;
        }
    }
}
