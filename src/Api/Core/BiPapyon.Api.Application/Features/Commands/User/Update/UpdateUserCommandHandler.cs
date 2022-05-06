using AutoMapper;
using BiPapyon.Api.Application.Interfaces.Repositories;
using BiPapyon.Common;
using BiPapyon.Common.Events;
using BiPapyon.Common.Infrastructure;
using BiPapyon.Common.Infrastructure.Exceptions;
using BiPapyon.Common.Models.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPapyon.Api.Application.Features.Commands.User.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await userRepository.GetByIdAsync(request.Id);

            if (dbUser is null)
                throw new ValidationException("User not found");

            var dbEmailAddress = dbUser.Email;
            var emailChanged = string.CompareOrdinal(dbEmailAddress, request.Email) != 0;

            mapper.Map(request, dbUser);

            var rows = await userRepository.UpdateAsync(dbUser);

            // check if email changed
            if (emailChanged && rows > 0)
            {
                var @event = new UserEmailChangedEvent
                {
                    OldEmail = dbEmailAddress,
                    NewEmail = request.Email
                };

                QueueFactory.SendMessageToExchange(
                    exchangeName: BiPapyonConstants.UserExchangeName,
                    exchangeType: BiPapyonConstants.DefaultExchangeType,
                    queueName: BiPapyonConstants.UserEmailChangedQueueName,
                    obj: @event);

                dbUser.EmailConfirmed = false;

                await userRepository.UpdateAsync(dbUser);
            }

            return dbUser.Id;
        }
    }
}
