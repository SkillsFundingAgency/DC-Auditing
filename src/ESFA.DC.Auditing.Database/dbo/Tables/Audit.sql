CREATE TABLE [dbo].[Audit] (
    [ID]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [JobId]       BIGINT         NOT NULL,
    [DateTimeUtc] DATETIME       NOT NULL,
    [Filename]    NVARCHAR (MAX) NULL,
    [Source]      NVARCHAR (MAX) NOT NULL,
    [UserId]      NVARCHAR (MAX) NOT NULL,
    [Event]       INT            NOT NULL,
    [ExtraInfo]   NVARCHAR (MAX) NULL,
    [UkPrn]       NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Audit] PRIMARY KEY CLUSTERED ([ID] ASC)
);

