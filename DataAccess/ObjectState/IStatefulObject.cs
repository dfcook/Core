namespace DanielCook.Core.DataAccess.ObjectState
{
    public interface IStatefulObject
    {
        ObjectState ObjectState { get; }

        void MarkClean();
        void MarkDirty();
        void MarkNew();
        void MarkDeleted();
    }
}
