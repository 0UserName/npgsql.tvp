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
        /// Size 
        /// of the parameter 
        /// headers in bytes.
        /// </summary>
        int MetadataSize
        {
            get;
        }

        /// <summary>
        /// A data type used for
        /// identifying internal 
        /// objects.
        /// </summary>
        uint Oid
        {
            get;
        }

        Value this[int row, int column]
        {
            get;
        }

        int GetRowSize(int row);

        int GetOverallSize();
    }
}