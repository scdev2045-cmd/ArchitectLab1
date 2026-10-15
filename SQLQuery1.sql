-- Создаем таблицу только в том случае, если ее еще нет.
-- Существующая таблица и ее записи не удаляются.
IF OBJECT_ID(N'dbo.Stars', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Stars
    (
        -- Первичный ключ.
        -- SQL Server сам выдает номер новой записи.
        Id INT IDENTITY(1, 1) NOT NULL
            CONSTRAINT PK_Stars PRIMARY KEY,

        -- NVARCHAR поддерживает русские буквы.
        Name NVARCHAR(200) NOT NULL,

        Discoverer NVARCHAR(200) NOT NULL,

        StarType NVARCHAR(100) NOT NULL,

        -- FLOAT соответствует используемому в модели double.
        Radius FLOAT NOT NULL,

        -- Дополнительные ограничения на уровне базы.
        -- Они не заменяют проверки в Logic.
        CONSTRAINT CK_Stars_Name
            CHECK (LEN(LTRIM(RTRIM(Name))) > 0),

        CONSTRAINT CK_Stars_Discoverer
            CHECK (LEN(LTRIM(RTRIM(Discoverer))) > 0),

        CONSTRAINT CK_Stars_Type
            CHECK
            (
                StarType IN
                (
                    N'Красный гигант',
                    N'Желтый карлик',
                    N'Белый карлик',
                    N'Нейтронная звезда'
                )
            ),

        CONSTRAINT CK_Stars_Radius
            CHECK (Radius > 0)
    );
END;