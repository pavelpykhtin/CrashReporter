DECLARE @targeDBVersion INT = 0005 --Required version of the database
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
			
			alter table crash
				alter column [Message] nvarchar(max) not null
			alter table crash
				alter column StackTrace nvarchar(max) null
			alter table crash
				alter column AdditionalInformation nvarchar(max) null
			alter table problem
				alter column Description nvarchar(max) null

						
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