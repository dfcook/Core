using System.Collections.Generic;
using System.Threading;

namespace DanielCook.Core.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private static readonly ThreadLocal<IUnitOfWork> _current =
            new ThreadLocal<IUnitOfWork>(() => new UnitOfWork());

        public static IUnitOfWork Current =>
            _current.Value;

        private IList<EditableObject> Dirty { get; } = new List<EditableObject>();

        private IList<EditableObject> New { get; } = new List<EditableObject>();

        private IList<EditableObject> Deleted { get; } = new List<EditableObject>();

        public void RegisterClean(EditableObject cleanObj)
        {
            Dirty.Remove(cleanObj);
            New.Remove(cleanObj);
            Deleted.Remove(cleanObj);
        }

        public void RegisterDirty(EditableObject dirtyObj)
        {
            Dirty.Add(dirtyObj);
            New.Remove(dirtyObj);
            Deleted.Remove(dirtyObj);
        }

        public void RegisterNew(EditableObject newObj)
        {
            Dirty.Remove(newObj);
            New.Add(newObj);
            Deleted.Remove(newObj);
        }

        public void RegisterDeleted(EditableObject deletedObj)
        {
            Dirty.Remove(deletedObj);
            New.Remove(deletedObj);
            Deleted.Add(deletedObj);
        }

        public void Clear()
        {
            Dirty.Clear();
            New.Clear();
            Deleted.Clear();
        }
    }
}
