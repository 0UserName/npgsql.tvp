using System.Collections;

using Testcontainers.Ado.PostgreSql;

namespace Npgsql.Tvp.Tests.Fixtures
{
    internal static class DbFixtures
    {
        public static IEnumerable Runners
        {
            get
            {
                yield return new TestFixtureData(new PostgreSqlRunner("postgres:14-alpine", "Init", default, (NpgsqlDataSourceBuilder builder) => builder.UseTvp()));
            }
        }
    }
}