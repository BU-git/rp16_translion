using System.Collections.Generic;

namespace IDAL
{
    public class WorkResult
    {
        #region CTOR

        public WorkResult(bool success)
        {
            Succeeded = success;
            Errors = new string[0];
        }

        public WorkResult(params string[] errors)
            : this((IEnumerable<string>) errors)
        {
        }

        public WorkResult(IEnumerable<string> errors)
        {
            if (errors == null)
                errors = new string[1]
                {
                    "DefaultError"
                };
            Succeeded = false;
            Errors = errors;
        }

        #endregion

        #region Fields

        public bool Succeeded { get; private set; }
        public IEnumerable<string> Errors { get; private set; }

        #endregion

        #region Static methods

        public static WorkResult Failed(params string[] errors)
        {
            return new WorkResult(errors);
        }

        public static WorkResult Success()
        {
            return new WorkResult(true);
        }

        #endregion
    }
}