using System.Security.Cryptography;
using System.Text;
using authorization_play.Core.Models;
using authorization_play.Core.Resources.Models;

namespace authorization_play.Core.Permissions.Models
{
    public class PermissionRequest
    {
        public MoARN Resource { get; set; }
        public ResourceAction Action { get; set; }
        public MoARN Principal { get; set; }
        public MoASchema Schema { get; set; }

        public string GetHash()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            using (var hash = new SHA512Managed())
            {
                var bytes = Encoding.UTF8.GetBytes(json);
                var hashBytes = hash.ComputeHash(bytes);
                return hashBytes.ToHexString();
            }
        }
    }

    public class PermissionValidationRequest
    {
        public MoARN Resource { get; set; }
        public ResourceAction Action { get; set; }
        public MoARN Principal { get; set; }
        public MoASchema Schema { get; set; }

        public string GetHash()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            using (var hash = new SHA512Managed())
            {
                var bytes = Encoding.UTF8.GetBytes(json);
                var hashBytes = hash.ComputeHash(bytes);
                return hashBytes.ToHexString();
            }
        }

        public static implicit operator PermissionRequest(PermissionValidationRequest request)
        {
            return new PermissionRequest()
            {
                Action = request.Action,
                Schema = request.Schema,
                Principal = request.Principal,
                Resource = request.Resource
            };
        }

        public static implicit operator PermissionValidationRequest(PermissionRequest request)
        {
            return new PermissionValidationRequest()
            {
                Action = request.Action,
                Schema = request.Schema,
                Principal = request.Principal,
                Resource = request.Resource
            };
        }
    }
}