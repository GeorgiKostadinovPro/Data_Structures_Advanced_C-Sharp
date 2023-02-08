using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.Discord
{
    public class Discord : IDiscord
    {
        private readonly IDictionary<string, Message> messagesById;
        private readonly IDictionary<string, HashSet<Message>> messagesByChannel;

        public Discord()
        {
            this.messagesById = new Dictionary<string, Message>();
            this.messagesByChannel = new Dictionary<string, HashSet<Message>>();
        }

        public int Count => this.messagesById.Count;

        public bool Contains(Message message)
        {
            return this.messagesById.ContainsKey(message.Id);
        }

        public void DeleteMessage(string messageId)
        {
            if (!this.messagesById.ContainsKey(messageId))
            {
                throw new ArgumentException();
            }

            Message message = this.messagesById[messageId];
            this.messagesById.Remove(messageId);
            this.messagesByChannel[message.Channel].Remove(message);
        }

        public IEnumerable<Message> GetAllMessagesOrderedByCountOfReactionsThenByTimestampThenByLengthOfContent()
        {
            return this.messagesById.Values
                .OrderByDescending(m => m.Reactions.Count)
                .ThenBy(m => m.Timestamp)
                .ThenBy(m => m.Content.Length)
                .ToArray();
        }

        public IEnumerable<Message> GetChannelMessages(string channel)
        {
            try
            {
                var messagesByChannel = this.messagesByChannel[channel];

                return messagesByChannel;
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException();
            }    

            //var messagesByChannel = this.messagesById.Values
            //    .Where(m => m.Channel == channel)
            //    .ToArray();

            //if (messagesByChannel.Length == 0)
            //{
            //    throw new ArgumentException();
            //}

            //return messagesByChannel;
        }

        public Message GetMessage(string messageId)
        {
            if (!this.messagesById.ContainsKey(messageId))
            {
                throw new ArgumentException();
            }

            return this.messagesById[messageId];
        }

        public IEnumerable<Message> GetMessageInTimeRange(int lowerBound, int upperBound)
        {
            return this.messagesById.Values
                .Where(m => m.Timestamp >= lowerBound && m.Timestamp <= upperBound)
                .OrderByDescending(m => this.messagesByChannel[m.Channel].Count)
                .ToArray();
        }

        public IEnumerable<Message> GetMessagesByReactions(List<string> reactions)
        {
            return messagesById.Values
                .Where(m => reactions.All(r => m.Reactions.Contains(r)))
                .OrderByDescending(m => m.Reactions.Count)
                .ThenBy(m => m.Timestamp)
                .ToList();
        }

        public IEnumerable<Message> GetTop3MostReactedMessages()
        {
            return this.messagesById.Values
                .OrderByDescending(m => m.Reactions.Count)
                .Take(3)
                .ToArray();
        }

        public void ReactToMessage(string messageId, string reaction)
        {
            if (!this.messagesById.ContainsKey(messageId))
            {
                throw new ArgumentException();
            }

            Message message = this.messagesById[messageId];
            message.Reactions.Add(reaction);
        }

        public void SendMessage(Message message)
        {
            if (!this.messagesById.ContainsKey(message.Id))
            {
                this.messagesById.Add(message.Id, message);
                
                if (!this.messagesByChannel.ContainsKey(message.Channel))
                {
                    this.messagesByChannel.Add(message.Channel, new HashSet<Message>());
                }
                
                this.messagesByChannel[message.Channel].Add(message);
            }
        }
    }
}
