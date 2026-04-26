using MassTransit;
using MediatR;
using Ong.Commom;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ong.Application.Handlers.Events
{
    public class DonationCreatedIntegrationHandler : INotificationHandler<DonationCreated>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public DonationCreatedIntegrationHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(DonationCreated notification, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(notification, cancellationToken);
        }
    }
}
