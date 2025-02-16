using Npgsql.Internal;

using Npgsql.Tvp.Internal.Accessors;

using System;
using System.Data;

using System.Threading;
using System.Threading.Tasks;

namespace Npgsql.Tvp.Internal.Converters
{
    internal static class DataTableWriter
    {
        [ThreadStatic]
        private static object _writeState;

        /// <summary>
        /// 
        /// </summary>
        private static PgTypeInfo GetPgTypeInfo(PgSerializerOptions options, Type type)
        {
            return options.GetDefaultTypeInfo(type);
        }

        /// <summary>
        /// 
        /// </summary>
        private static PgConverter GetPgConverter(PgTypeInfo pgTypeInfo, object payload)
        {
            return pgTypeInfo.GetObjectResolution(payload).Converter;
        }

        /// <summary>
        /// 
        /// </summary>
        private static BufferRequirements BufferRequirements(PgTypeInfo pgTypeInfo, object payload)
        {
            return pgTypeInfo.GetBufferRequirements(GetPgConverter(pgTypeInfo, payload), DataFormat.Binary) ?? throw new NotSupportedException($"Binary format is not supported: {pgTypeInfo.PgTypeId}");
        }

        /// <summary>
        /// Calculates the payload size in bytes.
        /// </summary>
        private static int GetPayloadSize(PgConverter pgConverter, BufferRequirements bufferRequirements, object payload)
        {
            if (payload == DBNull.Value)
            {
                return -1;
            }

            return bufferRequirements.Write.Kind == SizeKind.Exact ? bufferRequirements.Write.Value : _PgConverter.GetSize(pgConverter, default, payload, ref _writeState).Value;
        }

        /// <summary>
        /// Writes 
        /// the binary representation of DataTable: 
        /// set of rows is interpreted as an array 
        /// of type T, where T is a composite type 
        /// described by columns.
        /// </summary>
        public static async ValueTask WriteAsync(PgWriter writer, PgSerializerOptions options, CancellationToken cancellationToken)
        {
            DataTable dt = writer.Current.WriteState as DataTable;

            if (writer.ShouldFlush(DataTableSizes.GetDataTable(dt.Rows.Count)))
            {
                await writer.FlushAsync(cancellationToken).ConfigureAwait(default);
            }

            // 4 bytes with number of dimensions.
            writer.WriteInt32(1);

            // 4 bytes, boolean
            // indicating nulls
            // present or not.
            writer.WriteInt32(0);

            // 4 bytes array type OID.
            writer.WriteUInt32(0);

            // 4 bytes for length.
            writer.WriteInt32(dt.Rows.Count);

            // 4 bytes for lower bound on length
            // to check for overflow (it appears
            // this value can always be 0).
            writer.WriteInt32(0);


            foreach (DataRow row in dt.Rows)
            {
                if (writer.ShouldFlush(DataTableSizes.DataColumn))
                {
                    await writer.FlushAsync(cancellationToken).ConfigureAwait(default);
                }

                // writer
                //     .WriteInt32(row.SizeOverall);
                writer
                    .WriteInt32(dt.Columns.Count);

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    object payload = row.ItemArray[i];

                    if (writer.ShouldFlush(DataTableSizes.DataRow))
                    {
                        await writer.FlushAsync(cancellationToken).ConfigureAwait(default);
                    }

                    PgTypeInfo pgTypeInfo = GetPgTypeInfo(options, dt.Columns[i].DataType);

                    BufferRequirements bufferRequirements = BufferRequirements(pgTypeInfo, payload);

                    // 4 bytes elemen type OID.
                    writer.WriteUInt32(pgTypeInfo.PgTypeId.Value.Oid.Value);

                    int size = GetPayloadSize(GetPgConverter(pgTypeInfo, payload), bufferRequirements, payload);

                    // 4 bytes describing length
                    // of element, -1 means null.
                    writer.WriteInt32(size);

                    if (payload != DBNull.Value)
                    {
                        using (await writer.BeginNestedWriteAsync(bufferRequirements.Write, size, _writeState, cancellationToken))
                        {
                            // Binary representation of element.
                            await _PgConverter.WriteValueAsync(GetPgConverter(pgTypeInfo, payload), writer, payload, cancellationToken).ConfigureAwait(default);
                        }
                    }
                }
            }
        }
    }
}