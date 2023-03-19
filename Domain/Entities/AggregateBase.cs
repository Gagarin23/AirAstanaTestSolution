using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Interfaces;
using MediatR;

namespace Domain.Entities;

public abstract class AggregateBase : IAggregate
{
    public Guid Id { get; }
    protected readonly Queue<INotification> NotificationsInternal = new Queue<INotification>(0);

    public IEnumerable<INotification> Notifications
    {
        get
        {
            while (NotificationsInternal.TryDequeue(out var notification))
            {
                yield return notification;
            }
        }
    }
}
