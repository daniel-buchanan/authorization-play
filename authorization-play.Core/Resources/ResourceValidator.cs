using System.Linq;
using authorization_play.Core.Models;
using authorization_play.Core.Resources.Models;

namespace authorization_play.Core.Resources
{
    public interface IResourceValidator
    {
        ValidationResult<CRN> Validate(CRN resource);
        ValidationResult<CRN, ResourceAction> Validate(CRN resource, ResourceAction action);
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

        public ValidationResult<CRN> Validate(CRN resource)
        {
            if (!resource.IsValid) return ValidationResult<CRN>.Invalid(resource, InvalidResourceName);
            if (resource.Parts.Any(p => p.Contains("*"))) return ValidationResult<CRN>.Invalid(resource, ResourceNameCannotContainAny);

            var exists = this.storage.FindByIdentifier(resource);
            if(exists?.Any() == true) 
                return ValidationResult<CRN>.Valid(resource);

            return ValidationResult<CRN>.Invalid(resource, ResourceDoesNotExist);
        }

        public ValidationResult<CRN, ResourceAction> Validate(CRN resource, ResourceAction action)
        {
            var resourceValid = Validate(resource);
            if (!resourceValid.IsValid) 
                return ValidationResult<CRN, ResourceAction>.Invalid(resource, action, resourceValid.Reason);

            var found = this.storage.FindByIdentifier(resource).FirstOrDefault();
            if(found == null)
                return ValidationResult<CRN, ResourceAction>.Invalid(resource, action, ResourceDoesNotExist);

            if(!found.ValidActions.Contains(action)) 
                return ValidationResult<CRN, ResourceAction>.Invalid(resource, action, ActionInvalidForResource);

            return ValidationResult<CRN, ResourceAction>.Valid(resource, action);
        }

        public ValidationResult<ResourceAction> ValidateAction(ResourceAction action)
        {
            var found = this.storage.AllActions().FirstOrDefault(a => a == action);

            if(found != default(ResourceAction)) 
                return ValidationResult<ResourceAction>.Valid(action);

            return ValidationResult<ResourceAction>.Invalid(action, ActionNotFound);
        }
    }
}
