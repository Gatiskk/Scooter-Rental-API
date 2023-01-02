using ScooterRental.Core.Interfaces;

namespace ScooterRental.Core.Services
{
    public class ServiceResult
    {
        public bool Success { get; }
        public IEntity Entity { get; private set; }
        public IList<string> Errors { get; }
        public string FormattedErrors => string.Join(", ", Errors);

        public ServiceResult(bool success)
        {
            Success = success;
            Errors = new List<string>();
        }

        public ServiceResult SetEntity(IEntity entity)
        {
            Entity = entity;
            return this;
        }

        public ServiceResult AddError(string error)
        {
            Errors.Add(error);
            return this;
        }
    }
}
