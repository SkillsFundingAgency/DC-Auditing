CREATE TABLE [dbo].[AuditEventType]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [EventId] INT NOT NULL, 
    [EventTitle] NVARCHAR(MAX) NOT NULL, 
    [EventDescription] NVARCHAR(MAX) NOT NULL
)
