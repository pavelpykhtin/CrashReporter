DECLARE @targeDBVersion INT = 0008 --Required version of the database
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

			CREATE TABLE [dbo].[emailNotification](
				[id] [int] IDENTITY(1,1) NOT NULL,
				[message] [nvarchar](max) NOT NULL,
				[subject] [nvarchar](1024) NOT NULL,
				[recepients] [nvarchar](1024) NOT NULL,
				[isPending] [bit] NOT NULL,
				[createdAt] [datetime] NOT NULL,
			 CONSTRAINT [PK_emailNotification] PRIMARY KEY CLUSTERED 
			(
				[id] ASC
			)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
			) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


			ALTER TABLE [dbo].[emailNotification] ADD  CONSTRAINT [DF_emailNotification_isPending]  DEFAULT ((1)) FOR [isPending]

			ALTER TABLE [dbo].[emailNotification] ADD  CONSTRAINT [DF_emailNotification_createdAt]  DEFAULT (getdate()) FOR [createdAt]
						
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