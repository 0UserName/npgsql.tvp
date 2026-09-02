using System.Data;

using System.IO;

namespace Npgsql.Tvp.Tests.Rows
{
    internal static class RowsFactory
    {
        /// <param name="type">
        /// Name of the file that contains the
        /// XML schema and data representing a
        /// composite data type.
        /// </param>
        public static DataTable ReadToDataTable(string type)
        {
            DataTable table = new
            DataTable
            ();

            table.ReadXml(Path.Combine("Data", type));

            return table;
        }
    }
}