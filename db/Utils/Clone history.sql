set identity_insert tErrorType on
insert into CrashReports.dbo.tErrorType
	(ErrorTypeId, UniqueDescription, Cleared, ShortDescription, Description, Module, Status, FixedInVersion)
select ErrorTypeId, UniqueDescription, Cleared, ShortDescription, Description, Module, Status, FixedInVersion from OWUser.dbo.tErrorType 
set identity_insert tErrorType off

set identity_insert tError on
insert into CrashReports.dbo.tError
	(ErrorId, TimeStamp, LogLevel, Module, Message, StackTrace, AdditionalInformation, UserId, PersonId, TypeId, InnerException, Version)
select ErrorId, TimeStamp, LogLevel, Module, Message, StackTrace, AdditionalInformation, UserId, PersonId, TypeId, InnerException, Version from OWUser.dbo.tError 
set identity_insert tError off