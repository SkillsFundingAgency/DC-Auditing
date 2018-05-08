CREATE TABLE [dbo].[JobStatus] (
    [JobId]       BIGINT         NOT NULL,
    [Status]      INT            NOT NULL,
    [Created_By]  NVARCHAR (MAX) NOT NULL,
    [Created_On]  DATETIME       NOT NULL,
    [Modified_By] NVARCHAR (MAX) NOT NULL,
    [Modified_On] DATETIME       NOT NULL,
    CONSTRAINT [PK_JobStatus] PRIMARY KEY CLUSTERED ([JobId] ASC)
);

