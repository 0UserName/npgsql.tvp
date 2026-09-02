using Npgsql.Tvp.Tests.Fixtures;

using Npgsql.Tvp.Tests.Rows;

using System.Data;

using System.Threading.Tasks;

using Testcontainers.Ado.Contracts;

using Testcontainers.Ado.NUnit;
using Testcontainers.Ado.NUnit.Contracts.Abstracts;

using Testcontainers.Ado.NUnit.Extensions;

namespace Npgsql.Tvp.Tests
{
    [TestFixtureSource(typeof(DbFixtures), nameof(DbFixtures.Runners))]
    public sealed class InsertTests<TDbTestRunner>(TDbTestRunner runner) : AbstractDbTests<TDbTestRunner>(runner) where TDbTestRunner : IDbRunner
    {
        private Task TestAsync<TParameter>(string procedure, string type)
        {
            DataTable table = RowsFactory.ReadToDataTable(type);

            return Runner.ThatOutputRowsAsync(procedure, DbParameterNames.O_P, table.Rows.Count, typeof(TParameter).IsAssignableTo(typeof(IDataReader)) ? new DataTableReaderWrapper(table.Rows.Count, table.CreateDataReader()).AsIn() : table.AsIn(), default(long).AsOut());
        }

        [TestCase(Data.Procedures.VARIABLE, Data.Types.VARIABLE)]
        [TestCase(Data.Procedures.CONSTANT, Data.Types.CONSTANT)]
        public async Task TestDataReaderAsync(string procedure, string type)
        {
            await TestAsync<IDataReader>(procedure, type);
        }

        [TestCase(Data.Procedures.VARIABLE, Data.Types.VARIABLE)]
        [TestCase(Data.Procedures.CONSTANT, Data.Types.CONSTANT)]
        public async Task TestDataTableAsync(string procedure, string type)
        {
            await TestAsync<DataTable>(procedure, type);
        }
    }
}