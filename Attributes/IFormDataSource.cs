using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DanielCook.Core.Attributes
{
    public interface IFormDataSource : IEditableObject, IDataErrorInfo,
        INotifyPropertyChanged, INotifyPropertyChanging, IValidatableObject
    {
        void PopulateDataErrorInfo(IEnumerable<ValidationResult> errors);
    }

    public static class FormDataSourceValidator
    {
        public static IEnumerable<ValidationResult> Validate(IFormDataSource ds)
        {
            var ctx = new ValidationContext(ds);
            var errors = new List<ValidationResult>();
            Validator.TryValidateObject(ds, ctx, errors, true);

            errors.AddRange(ds.Validate(ctx));
            ds.PopulateDataErrorInfo(errors);

            return errors;
        }
    }
}
