using System;

namespace authorization_play.Core.Models
{
    public abstract class ValidationResult
    {
        public bool IsValid { get; protected set; }
        public string Reason { get; protected set; }
    }

    public class ValidationResult<T1> : ValidationResult
    {
        public T1 Principal { get; protected set; }

        public static ValidationResult<T1> Valid(T1 principal) => new ValidationResult<T1>()
        {
            IsValid = true,
            Principal = principal
        };

        public static ValidationResult<T1> Invalid(T1 principal, string reason) => new ValidationResult<T1>()
        {
            Reason = reason,
            Principal = principal
        };
    }

    public class ValidationResult<T1, T2> : ValidationResult
    {
        public T1 Principal { get; protected set; }
        public T2 Secondary { get; protected set; }

        public static ValidationResult<T1, T2> Valid(T1 principal, T2 secondary) => new ValidationResult<T1, T2>()
        {
            IsValid = true,
            Principal = principal,
            Secondary = secondary
        };

        public static ValidationResult<T1, T2> Invalid(T1 principal, T2 secondary, string reason) => new ValidationResult<T1, T2>()
        {
            Reason = reason,
            Principal = principal,
            Secondary = secondary
        };
    }

    public class ValidationResult<T1, T2, T3> : ValidationResult
    {
        public T1 Principal { get; protected set; }
        public T2 Secondary { get; protected set; }
        public T3 Tertiary { get; protected set; }

        public static ValidationResult<T1, T2, T3> Valid(T1 principal, T2 secondary, T3 tertiary) => new ValidationResult<T1, T2, T3>()
        {
            IsValid = true,
            Principal = principal,
            Secondary = secondary,
            Tertiary = tertiary
        };

        public static ValidationResult<T1, T2, T3> Invalid(T1 principal, T2 secondary, T3 tertiary, string reason) => new ValidationResult<T1, T2, T3>()
        {
            Reason = reason,
            Principal = principal,
            Secondary = secondary,
            Tertiary = tertiary
        };
    }

    public static class Validator
    {
        public static bool TryValidate<T>(Func<T> validate, out T result)
            where T : ValidationResult
        {
            result = validate();
            return result.IsValid;
        }
    }
}
