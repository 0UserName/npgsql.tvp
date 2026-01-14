using System;

namespace Npgsql.Tvp.Internal.Converters.Models.Contracts
{
    internal interface IParameter : IDisposable
    {
        int ColumnsCount
        {
            get;
        }

        int RowsCount
        {
            get;
        }

        /// <summary>
        /// Size of the parameter headers.
        /// </summary>
        /// 
        /// <remarks>
        /// Dimensions + Flags + OID + (array length and lower bound) * $DIMENSIONS + $Value size integers.
        /// </remarks>
        int MetadataSize
        {
            get;
        }

        /// <summary>
        /// Unique id identifying the data type in a given database (in pg_type).
        /// </summary>
        uint OID
        {
            get;
        }

        /// <summary>
        /// Gets the size of the specified row.
        /// </summary>
        int this[int row]
        {
            get;
        }

        /// <summary>
        /// Gets the value at 
        /// the specified row 
        /// and column.
        /// </summary>
        Value this[int row, int column]
        {
            get;
        }

        int GetSize();
    }
}