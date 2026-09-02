namespace Npgsql.Tvp.Tests
{
    internal static class Data
    {
        public static class Types
        {
            public const string VARIABLE = "dbo.table_valued_variable.xml";
            public const string CONSTANT = "dbo.table_valued_constant.xml";
        }

        public static class Procedures
        {
            /// <remarks>
            /// <code>
            /// dbo.table_valued_variable_insert(IN i_p dbo.table_valued_variable[], OUT o_p BIGINT)
            /// </code>
            /// </remarks>
            public const string VARIABLE = "dbo.table_valued_variable_insert";

            /// <remarks>
            /// <code>
            /// dbo.table_valued_constant_insert(IN i_p dbo.table_valued_constant[], OUT o_p BIGINT)
            /// </code>
            /// </remarks>
            public const string CONSTANT = "dbo.table_valued_constant_insert";
        }
    }
}