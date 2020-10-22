﻿using System;
using System.Linq;
using authorization_play.Core.Models;
using authorization_play.Core.Permissions.Models;

namespace authorization_play.Core.Permissions
{
    public interface IPermissionTicketManager
    {
        PermissionTicket Request(params PermissionRequest[] request);
        void Revoke(MoARN resource, MoARN principal, MoASchema schema = null);
        void Revoke(string hash);
        void Revoke(PermissionTicket ticket);
        bool Validate(string input, string secret = null);
        bool Validate(PermissionTicket ticket);
    }

    public class PermissionTicketManager : IPermissionTicketManager
    {
        private const int DefaultTicketDuration = 30;
        private readonly IPermissionValidator validator;
        private readonly IPermissionTicketStorage storage;

        public PermissionTicketManager(IPermissionValidator validator,
            IPermissionTicketStorage storage)
        {
            this.validator = validator;
            this.storage = storage;
        }

        public PermissionTicket Request(params PermissionRequest[] request)
        {
            if(request == null || request.Length == 0) return PermissionTicket.Invalid();

            var requestHash = string.Join(".", request.Select(r => r.GetHash()));

            var ticket = this.storage.Find(requestHash);
            var existingTicketExpired = ticket != null && ticket?.IsExpired(DateTimeOffset.UtcNow) == true;

            if (!existingTicketExpired && ticket != null) return ticket;
            
            this.storage.Remove(requestHash);

            var responses = new PermissionValidationResponse[request.Length];
            for (var i = 0; i < request.Length; i++)
                responses[i] = this.validator.Validate(request[i]);

            ticket = PermissionTicket.FromValidation(responses);
            ticket.WithExpiry(DateTimeOffset.UtcNow.AddMinutes(DefaultTicketDuration));

            if(ticket.IsValid) this.storage.Add(requestHash, ticket);

            return ticket;
        }

        public void Revoke(MoARN resource, MoARN principal, MoASchema schema = null)
        {
            var found = this.storage.FindBy(it =>
                it.Principal == principal && 
                it.Resources.Any(r =>
                {
                    if (resource.IncludesWildcard) return resource.IsWildcardMatch(r.Identifier);
                    return r.Identifier == resource;
                }));

            if (schema != null)
                found = found.Where(f => f.Resources.Any(r => r.Schema == schema));

            var keys = found.Select(f => f.GetHash());

            foreach (var k in keys) this.storage.Remove(k);
        }

        public void Revoke(string hash)
        {
            var found = this.storage.Find(hash);
            if (found == null) return;
            this.storage.Remove(hash);
        }

        public void Revoke(PermissionTicket ticket)
        {
            if(ticket == null) throw new ArgumentNullException(nameof(ticket), "Ticket cannot be NULL");
            this.storage.Remove(ticket.GetHash());
        }

        public bool Validate(string input, string secret = null)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;
            PermissionTicket ticket;
            try
            {
                ticket = Newtonsoft.Json.JsonConvert.DeserializeObject<PermissionTicket>(input);
            }
            catch
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(secret)) return false;
                    ticket = PermissionTicket.FromJwt(input, secret);
                }
                catch
                {
                    return false;
                }
            }

            return Validate(ticket);
        }

        public bool Validate(PermissionTicket ticket) =>
            ticket?.IsValid == true && 
            ticket?.IsExpired(DateTimeOffset.UtcNow) == false &&
            this.storage.Find(ticket.GetHash()) != null;
    }
}
