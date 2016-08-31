using System.Collections.Generic;
using System.Linq;

using DanielCook.Core.Extensions;

namespace DanielCook.Core.DataAccess.Sorting
{
    public enum SortDirection
    {
        Ascending,
        Descending
    }

    public class SortColumnList : List<SortColumn>
    {
        public SortColumnList(IEnumerable<SortColumn> sortColumns) : base(sortColumns) { }
        public SortColumnList() { }

        public override string ToString()
        {
            if (!this.Any())
                return string.Empty;

            var s = " order by" + EnumerableExtensions.
                Map(this, x => x.ToString()).
                Reduce(string.Empty, (current, col) => $"{current} {col},");

            return s.Remove(s.Length - 1);
        }
    }

    public class SortColumn
    {
        public string ColumnName { get; }
        public SortDirection Direction { get; }

        public SortColumn(string columnName, SortDirection direction)
        {
            ColumnName = columnName;
            Direction = direction;
        }

        private string SqlDirection =>
            Direction == SortDirection.Ascending ? "asc" : "desc";

        public override string ToString() =>
            $"{ColumnName} {SqlDirection}";
    }
}
