﻿using ECommon.Components;
using ECommon.Dapper;
using ENode.Eventing;
using Forum.Domain.Replies;
using Forum.Infrastructure;

namespace Forum.Denormalizers.Dapper
{
    [Component(LifeStyle.Singleton)]
    public class ReplyEventHandler : BaseEventHandler,
        IEventHandler<ReplyCreatedEvent>,
        IEventHandler<ReplyBodyUpdatedEvent>
    {
        public void Handle(ReplyCreatedEvent evnt)
        {
            using (var connection = GetConnection())
            {
                connection.Insert(
                    new
                    {
                        Id = evnt.AggregateRootId,
                        PostId = evnt.PostId,
                        ParentId = evnt.ParentId,
                        AuthorId = evnt.AuthorId,
                        Body = evnt.Body,
                        CreatedOn = evnt.Timestamp,
                        UpdatedOn = evnt.Timestamp
                    },
                    Constants.ReplyTable);
            }
        }
        public void Handle(ReplyBodyUpdatedEvent evnt)
        {
            using (var connection = GetConnection())
            {
                connection.Update(
                    new
                    {
                        Body = evnt.Body,
                        UpdatedOn = evnt.Timestamp
                    },
                    new
                    {
                        Id = evnt.AggregateRootId
                    }, Constants.ReplyTable);
            }
        }
    }
}
