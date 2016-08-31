using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using DanielCook.Core.Attributes;
using DanielCook.Core.Extensions;
using IStatefulObject = DanielCook.Core.DataAccess.ObjectState.IStatefulObject;
using State = DanielCook.Core.DataAccess.ObjectState.ObjectState;

namespace DanielCook.Core.DataAccess.UnitOfWork
{
    [Serializable]
    public abstract class EditableObject :
           IFormDataSource, IStatefulObject
    {
        [NonSerialized]
        private EditableObject _backup;

        protected bool _loading;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        [field: NonSerialized]
        public event PropertyChangingEventHandler PropertyChanging;

        private State _objectState;
        public State ObjectState { get; private set; }

        [NonSerialized]
        private IDictionary<string, ValidationResult> _validationResults;

        public virtual string Error =>
            (_validationResults != null && _validationResults.Any()) ?
            $"There are {_validationResults.Count} errors" :
            string.Empty;

        public virtual string this[string columnName]
        {
            get
            {
                return (_validationResults != null && _validationResults.ContainsKey(columnName)) ?
                    _validationResults[columnName].ErrorMessage :
                    string.Empty;
            }
        }

        protected EditableObject()
        {
            MarkNew();
        }

        public void BeginLoad()
        {
            _loading = true;
        }

        public void EndLoad()
        {
            _loading = false;
            MarkClean();
        }

        protected virtual void DoBackup()
        {
            _backup = this.DeepClone();
        }

        protected abstract void Rollback(EditableObject backup);

        public virtual void BeginEdit()
        {
            DoBackup();
        }

        public virtual void CancelEdit()
        {
            Rollback(_backup);
            if (ObjectState == State.Dirty)
                MarkClean();
        }

        public virtual void EndEdit()
        {
            //throw new NotImplementedException();
        }

        protected void OnPropertyChanging([CallerMemberName] string propertyName = "") =>
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (!_loading &&
                ObjectState == State.Clean)
            {
                MarkDirty();
            }
        }

        public abstract IEnumerable<ValidationResult> Validate(ValidationContext validationContext);

        public void PopulateDataErrorInfo(IEnumerable<ValidationResult> errors)
        {
            _validationResults = new Dictionary<string, ValidationResult>();
            errors.Each(error =>
                error.MemberNames.Each(member =>
                    _validationResults.Add(member, error)));
        }

        public void MarkClean()
        {
            ObjectState = State.Clean;
            UnitOfWork.Current.RegisterClean(this);
        }

        public void MarkDirty()
        {
            if (ObjectState != State.New)
            {
                ObjectState = State.Dirty;
                UnitOfWork.Current.RegisterDirty(this);
            }

        }

        public void MarkNew()
        {
            ObjectState = State.New;
            UnitOfWork.Current.RegisterNew(this);
        }

        public void MarkDeleted()
        {
            ObjectState = State.Deleted;
            UnitOfWork.Current.RegisterDeleted(this);
        }
    }
}
