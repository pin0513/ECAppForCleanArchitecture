IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'SchemaVersions')
   )
BEGIN
    IF (NOT EXISTS(SELECT * FROM dbo.SchemaVersions))
BEGIN
Drop TABLE SchemaVersions
CREATE TABLE [dbo].[SchemaVersions](
    [Id] [uniqueidentifier] NOT NULL,
    [PartitionKey] [varchar](100) NULL,
    [RawKey] [varchar](100) NOT NULL,
    [Description] [nvarchar](100) NULL,
    [Data] [nvarchar](max) NULL,
    [DeletedTime] [datetimeoffset](7) NULL,
    [Creator] [bigint] NOT NULL,
    [CreatedTime] [datetimeoffset](7) NOT NULL,
    [LatestUpdater] [bigint] NULL,
    [LatestUpdatedTime] [datetimeoffset](7) NULL
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
END
ELSE
BEGIN
CREATE TABLE [dbo].[SchemaVersions](
    [Id] [uniqueidentifier] NOT NULL,
    [PartitionKey] [varchar](100) NULL,
    [RawKey] [varchar](100) NOT NULL,
    [Description] [nvarchar](100) NULL,
    [Data] [nvarchar](max) NULL,
    [DeletedTime] [datetimeoffset](7) NULL,
    [Creator] [bigint] NOT NULL,
    [CreatedTime] [datetimeoffset](7) NOT NULL,
    [LatestUpdater] [bigint] NULL,
    [LatestUpdatedTime] [datetimeoffset](7) NULL
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END