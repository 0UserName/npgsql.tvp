namespace Npgsql.Tvp.Internal.Converters
{
    internal static class DataTableSizes
    {
        /// <summary>
        /// Sum 
        /// of the sizes of 
        /// the headers for 
        /// a single column.
        /// </summary>
        public const int DataColumn = sizeof(uint) + sizeof(int);

        /// <summary>
        /// Gets sum of the column header sizes.
        /// </summary>
        public const int DataRow = sizeof(int) + sizeof(int);

        /// <summary>
        /// Gets sum of the sizes of the table headers.
        /// </summary>
        public static int GetDataTable(int length)
        {
            return sizeof(int) + sizeof(int) + sizeof(uint) + 1 * (sizeof(int) + sizeof(int)) + sizeof(int) * length;
        }
    }
}