using System.Collections.Generic;
using MediatR;

namespace Domain.Interfaces;

public interface IAggregate
{
    public IEnumerable<INotification> Notifications { get; }
}