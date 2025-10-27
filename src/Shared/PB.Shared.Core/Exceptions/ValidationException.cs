namespace PB.Shared.Core.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(IDictionary<string, string[]> errors)
            : base("Uma ou mais validações falharam")
        {
            Errors = errors;
        }
    }
}
