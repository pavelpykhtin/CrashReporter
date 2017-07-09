DECLARE @targeDBVersion INT = 0001 --Required version of the database
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
			
			CREATE TABLE [dbo].[application](
				[id] [int] IDENTITY(1,1) NOT NULL,
				[key] [nvarchar](50) NOT NULL,
				[language] [nvarchar](50) NOT NULL,
			 CONSTRAINT [PK_application] PRIMARY KEY CLUSTERED 
			(
				[id] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY]
						
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