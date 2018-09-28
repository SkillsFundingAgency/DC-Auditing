CREATE VIEW [dbo].[AuditInformation]
	AS SELECT 
	a.[ID], a.[JobId], a.[DateTimeUtc], a.[Filename], a.[Source], a.[UserId], a.[Event], [aet].[EventTitle], [aet].[EventDescription], a.[ExtraInfo], a.[UkPrn]
	FROM [dbo].[Audit] a
	INNER JOIN [dbo].[AuditEventType] aet ON a.Event = aet.EventId
