using System;
using System.Collections.Generic;
using System.Linq;
using authorization_play.Core.Permissions.Models;

namespace authorization_play.Core.Permissions
{
    public interface IPermissionTicketStorage
    {
        void Add(string requestHash, PermissionTicket ticket);
        void Remove(string hash);
        PermissionTicket Find(string hash);
        IEnumerable<PermissionTicket> FindBy(Func<PermissionTicket, bool> predicate);
    }

    public class PermissionTicketStorage : IPermissionTicketStorage
    {
        private readonly IDictionary<string, PermissionTicket> tickets;
        private readonly IDictionary<string, string> requestMap;

        public PermissionTicketStorage()
        {
            this.tickets = new Dictionary<string, PermissionTicket>();
            this.requestMap = new Dictionary<string, string>();
        }

        public void Add(string requestHash, PermissionTicket ticket)
        {
            var hash = ticket.GetHash();
            this.tickets.Add(hash, ticket);
            this.requestMap.Add(requestHash, hash);
        }

        public void Remove(string hash)
        {
            if (this.requestMap.TryGetValue(hash, out var ticketHash))
            {
                this.tickets.Remove(ticketHash);
                this.requestMap.Remove(hash);
            }
            else
            {
                this.tickets.Remove(hash);
            }
        }

        public PermissionTicket Find(string hash)
        {
            if (this.requestMap.TryGetValue(hash, out var ticketHash))
            {
                if (this.tickets.TryGetValue(ticketHash, out var ticketByRequest))
                {
                    return ticketByRequest;
                }
            }
            else if (this.tickets.TryGetValue(hash, out var ticket))
            {
                return ticket;
            }

            return null;
        }

        public IEnumerable<PermissionTicket> FindBy(Func<PermissionTicket, bool> predicate) =>
            this.tickets.Where(kp => predicate(kp.Value))
                .Select(kp => kp.Value);
    }
}
