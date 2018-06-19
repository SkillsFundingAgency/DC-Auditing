/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

GO
RAISERROR('		   Extended Property',10,1) WITH NOWAIT;
GO

RAISERROR('		         %s - %s',10,1,'BuildNumber','$(BUILD_BUILDNUMBER)') WITH NOWAIT;
IF NOT EXISTS (SELECT name, value FROM fn_listextendedproperty('BuildNumber', default, default, default, default, default, default))
	EXEC sp_addextendedproperty @name = N'BuildNumber', @value = '$(BUILD_BUILDNUMBER)';  
ELSE
	EXEC sp_updateextendedproperty @name = N'BuildNumber', @value = '$(BUILD_BUILDNUMBER)';  
	
GO
RAISERROR('		         %s - %s',10,1,'BuildBranch','$(BUILD_BRANCHNAME)') WITH NOWAIT;
IF NOT EXISTS (SELECT name, value FROM fn_listextendedproperty('BuildBranch', default, default, default, default, default, default))
	EXEC sp_addextendedproperty @name = N'BuildBranch', @value = '$(BUILD_BRANCHNAME)';  
ELSE
	EXEC sp_updateextendedproperty @name = N'BuildBranch', @value = '$(BUILD_BRANCHNAME)';  

GO
DECLARE @DeploymentTime VARCHAR(35) = CONVERT(VARCHAR(35),GETUTCDATE(),113);
RAISERROR('		         %s - %s',10,1,'DeploymentDatetime',@DeploymentTime) WITH NOWAIT;
IF NOT EXISTS (SELECT name, value FROM fn_listextendedproperty('DeploymentDatetime', default, default, default, default, default, default))
	EXEC sp_addextendedproperty @name = N'DeploymentDatetime', @value = @DeploymentTime;  
ELSE
	EXEC sp_updateextendedproperty @name = N'DeploymentDatetime', @value = @DeploymentTime;  


GO
RAISERROR('		   Extended Property - Compelete',10,1) WITH NOWAIT;

GO

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
	WHEN NOT MATCHED BY SOURCE THEN DELETE;

Go
RAISERROR('		   Update User Account Passwords',10,1) WITH NOWAIT;
GO
ALTER USER AppAuditUser WITH PASSWORD = N'$(AppAuditUserPwd)';

GO
RAISERROR('		   Update User Account Passwords Update Complete',10,1) WITH NOWAIT;
GO
