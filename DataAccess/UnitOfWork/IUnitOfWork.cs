namespace DanielCook.Core.DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        void RegisterClean(EditableObject cleanObj);

        void RegisterDirty(EditableObject dirtyObj);

        void RegisterNew(EditableObject newObj);

        void RegisterDeleted(EditableObject deletedObj);

        void Clear();
    }
}
