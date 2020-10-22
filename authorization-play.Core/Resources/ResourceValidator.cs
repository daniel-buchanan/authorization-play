using System.Linq;
using authorization_play.Core.Models;
using authorization_play.Core.Resources.Models;
using authorization_play.Core.Static;

namespace authorization_play.Core.Resources
{
    public interface IResourceValidator
    {
        ValidationResult<MoARN> Validate(MoARN resource);
        ValidationResult<MoARN, ResourceAction> Validate(MoARN resource, ResourceAction action);
        ValidationResult<ResourceAction> ValidateAction(ResourceAction action);
    }

    public class ResourceValidator : IResourceValidator
    {
        private const string InvalidResourceName = "Resource Name is Invalid";
        private const string ResourceNameCannotContainAny = "Resource Name must not contain any character (*)";
        private const string ResourceDoesNotExist = "Resource does not exist";
        private const string ActionInvalidForResource = "The specified Action is invalid for the provided Resource";
        private const string ActionNotFound = "The specified Action does not exist, or is Invalid";

        private readonly IResourceStorage storage;

        public ResourceValidator(IResourceStorage storage)
        {
            this.storage = storage;
        }

        public ValidationResult<MoARN> Validate(MoARN resource)
        {
            if (!resource.IsValid) return ValidationResult<MoARN>.Invalid(resource, InvalidResourceName);
            if (resource.Parts.Any(p => p.Contains("*"))) return ValidationResult<MoARN>.Invalid(resource, ResourceNameCannotContainAny);

            var exists = this.storage.Exists(r => r.Identifier == resource);
            if(exists) 
                return ValidationResult<MoARN>.Valid(resource);

            return ValidationResult<MoARN>.Invalid(resource, ResourceDoesNotExist);
        }

        public ValidationResult<MoARN, ResourceAction> Validate(MoARN resource, ResourceAction action)
        {
            var resourceValid = Validate(resource);
            if (!resourceValid.IsValid) 
                return ValidationResult<MoARN, ResourceAction>.Invalid(resource, action, resourceValid.Reason);

            var found = this.storage.FirstOrDefault(r => r.Identifier == resource);
            if(found == null)
                return ValidationResult<MoARN, ResourceAction>.Invalid(resource, action, ResourceDoesNotExist);

            if(!found.ValidActions.Contains(action)) 
                return ValidationResult<MoARN, ResourceAction>.Invalid(resource, action, ActionInvalidForResource);

            return ValidationResult<MoARN, ResourceAction>.Valid(resource, action);
        }

        public ValidationResult<ResourceAction> ValidateAction(ResourceAction action)
        {
            var found = ResourceActions.All().FirstOrDefault(a => a == action);

            if(found != null) 
                return ValidationResult<ResourceAction>.Valid(action);

            return ValidationResult<ResourceAction>.Invalid(action, ActionNotFound);
        }
    }
}
