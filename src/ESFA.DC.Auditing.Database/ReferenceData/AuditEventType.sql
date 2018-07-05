

DECLARE @SummaryOfChanges_AuditEventType TABLE ([EventId] INT, [Action] VARCHAR(100));

MERGE INTO AuditEventType AS Target
USING (VALUES
	(0, 'JobSubmitted', 'Job was submitted'),
	(1, 'JobStarted', 'Job was started'),
	(2, 'ServiceStarted', 'A service started'),
	(3, 'ServiceFailed', 'A service failed'),
	(4, 'ServiceFinished', 'A service finished'),
	(5, 'JobFailed', 'A job failed'),
	(6, 'JobFinished', 'A job finished')
	)
	AS Source([EventId], [EventTitle], [EventDescription])
	ON Target.[EventId] = Source.[EventId]
	WHEN MATCHED THEN UPDATE SET Target.EventDescription = Source.EventDescription	
	WHEN NOT MATCHED BY TARGET THEN INSERT([EventId], [EventTitle], [EventDescription]) VALUES ([EventId], [EventTitle], [EventDescription])
	WHEN NOT MATCHED BY SOURCE THEN DELETE
	OUTPUT Inserted.[EventId],$action INTO @SummaryOfChanges_AuditEventType([EventId],[Action])
;

	DECLARE @AddCount_AET INT, @UpdateCount_AET INT, @DeleteCount_AET INT
	SET @AddCount_AET  = ISNULL((SELECT Count(*) FROM @SummaryOfChanges_AuditEventType WHERE [Action] = 'Insert' GROUP BY Action),0);
	SET @UpdateCount_AET = ISNULL((SELECT Count(*) FROM @SummaryOfChanges_AuditEventType WHERE [Action] = 'Update' GROUP BY Action),0);
	SET @DeleteCount_AET = ISNULL((SELECT Count(*) FROM @SummaryOfChanges_AuditEventType WHERE [Action] = 'Delete' GROUP BY Action),0);
	RAISERROR('                       %s           - Added %i - Update %i - Delete %i',10,1,'AuditEventType', @AddCount_AET, @UpdateCount_AET, @DeleteCount_AET) WITH NOWAIT;

