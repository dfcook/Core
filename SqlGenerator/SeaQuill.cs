namespace DanielCook.Core.SqlGenerator
{
    public static class SeaQuill
    {
        public static SelectStatement Select() =>
            new SelectStatement();

        public static PagedSelectStatement PagedSelect(int start, int rowsPerPage) =>
            new PagedSelectStatement(start, rowsPerPage);

        public static DeleteStatement Delete() =>
            new DeleteStatement();

        public static UpdateStatement Update() =>
            new UpdateStatement();

        public static InsertStatement Insert() =>
            new InsertStatement();
    }
}
