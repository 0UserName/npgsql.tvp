-- -------------------------------------------------
-- 1. Create Table-Valued Parameter Type
-- -------------------------------------------------

CREATE TYPE dbo.table_valued_variable AS
(
    "Property1" BOOLEAN,
    "Property2" INTEGER,
    "Property3" BIGINT,
    "Property4" TEXT,
    "Property5" INTEGER,
    "Property6" TIMESTAMPTZ,
    "Property7" TIMESTAMPTZ,
    "Property8" TIMETZ
);


-- -------------------------------------------------
-- 2. Create Target Table
-- -------------------------------------------------

CREATE TABLE dbo.table_valued_variable_test
(
    "Property1" BOOLEAN,
    "Property2" INTEGER,
    "Property3" BIGINT,
    "Property4" TEXT,
    "Property5" INTEGER,
    "Property6" TIMESTAMPTZ,
    "Property7" TIMESTAMP,
    "Property8" TIME WITH TIME ZONE
)
TABLESPACE pg_default;


-- -------------------------------------------------
-- 3. Create Procedure Accepting TVP
-- -------------------------------------------------

CREATE PROCEDURE dbo.table_valued_variable_insert(IN i_p dbo.table_valued_variable[], OUT o_p BIGINT)
LANGUAGE plpgsql
AS $body$
BEGIN
    INSERT INTO dbo.table_valued_variable_test
    (
        "Property1",
        "Property2",
        "Property3",
        "Property4",
        "Property5",
        "Property6",
        "Property7",
        "Property8"
    )
    SELECT
        p."Property1",
        p."Property2",
        p."Property3",
        p."Property4",
        p."Property5",
        p."Property6",
        p."Property7",
        p."Property8"
    FROM unnest(i_p) AS p;

    GET DIAGNOSTICS o_p = ROW_COUNT;
END;
$body$;