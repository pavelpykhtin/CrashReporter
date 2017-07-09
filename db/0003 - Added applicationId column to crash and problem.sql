DECLARE @targeDBVersion INT = 0002 --Required version of the database
DECLARE @numberOfRecordsInVersionTable INT
DECLARE @currenDBVersion INT

set @numberOfRecordsInVersionTable = (select count(*) from DBVersion)

set @currenDBVersion = (select max(DBVersion) from DBVersion)

IF (@numberOfRecordsInVersionTable <> 1)
	PRINT 'Something strange with version table. It should have only one record!!!'
ELSE IF (@currenDBVersion <> @targeDBVersion)
	PRINT 'Wrong version of the database. Version ' + STR(@targeDBVersion) + ' required. Your version is:' + STR(@currenDBVersion)
ELSE
BEGIN
	BEGIN TRY	
		BEGIN TRANSACTION
			
			ALTER TABLE dbo.problem ADD
				applicationId int NULL

			ALTER TABLE dbo.problem ADD CONSTRAINT
				FK_problem_application FOREIGN KEY
				(
				applicationId
				) REFERENCES dbo.application
				(
				id
				) ON UPDATE  NO ACTION 
				 ON DELETE  NO ACTION
				 
			ALTER TABLE dbo.crash ADD
				applicationId int NULL

			ALTER TABLE dbo.crash ADD CONSTRAINT
				FK_crash_application FOREIGN KEY
				(
				applicationId
				) REFERENCES dbo.application
				(
				id
				) ON UPDATE  NO ACTION 
				 ON DELETE  NO ACTION	
						
			UPDATE DBVersion SET DBVersion = @targeDBVersion + 1
			
		COMMIT TRANSACTION                                                                          
		
	END TRY
	BEGIN CATCH
	
		ROLLBACK TRAN
		
		DECLARE @errorMessage NVARCHAR(MAX) = ERROR_MESSAGE()
		DECLARE @errorSeverity INT = ERROR_SEVERITY()
        DECLARE @errorState INT = ERROR_STATE()
        
		RAISERROR(@errorMessage, @errorSeverity, @errorState)
	END CATCH
END