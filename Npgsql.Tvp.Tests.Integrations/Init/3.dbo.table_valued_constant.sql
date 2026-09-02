-- -------------------------------------------------
-- 1. Create Table-Valued Parameter Type
-- -------------------------------------------------

CREATE TYPE dbo.table_valued_constant AS
(
    "Property1" BOOLEAN,
    "Property2" INTEGER,
    "Property3" BIGINT
);


-- -------------------------------------------------
-- 2. Create Target Table
-- -------------------------------------------------

CREATE TABLE dbo.table_valued_constant_test
(
    "Property1" BOOLEAN NOT NULL,
    "Property2" INTEGER NOT NULL,
    "Property3" BIGINT  NOT NULL
)
TABLESPACE pg_default;


-- -------------------------------------------------
-- 3. Create Procedure Accepting TVP
-- -------------------------------------------------

CREATE PROCEDURE dbo.table_valued_constant_insert(IN i_p dbo.table_valued_constant[], OUT o_p BIGINT)
LANGUAGE plpgsql
AS $body$
BEGIN
    INSERT INTO dbo.table_valued_constant_test
    (
        "Property1",
        "Property2",
        "Property3"
    )
    SELECT
        p."Property1",
        p."Property2",
        p."Property3"
    FROM unnest(i_p) AS p;

    GET DIAGNOSTICS o_p = ROW_COUNT;
END;
$body$;