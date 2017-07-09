/****** Object:  Table [dbo].[DBVersion]    Script Date: 01.03.2016 21:09:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DBVersion](
	[DBVersion] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tError]    Script Date: 01.03.2016 21:09:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tError](
	[ErrorID] [int] IDENTITY(1,1) NOT NULL,
	[TimeStamp] [datetime] NULL,
	[LogLevel] [nvarchar](50) NOT NULL,
	[Module] [nvarchar](50) NOT NULL,
	[Message] [ntext] NOT NULL,
	[StackTrace] [ntext] NULL,
	[AdditionalInformation] [ntext] NULL,
	[UserId] [int] NULL,
	[PersonId] [int] NULL,
	[TypeID] [int] NULL,
	[InnerException] [nvarchar](max) NULL,
	[Version] [nvarchar](50) NULL,
 CONSTRAINT [PK_tError] PRIMARY KEY CLUSTERED 
(
	[ErrorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tErrorType]    Script Date: 01.03.2016 21:09:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tErrorType](
	[ErrorTypeID] [int] IDENTITY(1,1) NOT NULL,
	[UniqueDescription] [nvarchar](400) NOT NULL,
	[Cleared] [bit] NOT NULL,
	[ShortDescription] [nvarchar](255) NULL,
	[Description] [ntext] NULL,
	[Module] [nvarchar](50) NULL,
	[Status] [int] NOT NULL,
	[FixedInVersion] [nvarchar](50) NULL,
 CONSTRAINT [PK_tErrorType] PRIMARY KEY CLUSTERED 
(
	[ErrorTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[tError] ADD  DEFAULT ((0)) FOR [Version]
GO
ALTER TABLE [dbo].[tErrorType] ADD  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[tErrorType] ADD  DEFAULT ((0)) FOR [FixedInVersion]
GO
ALTER TABLE [dbo].[tError]  WITH CHECK ADD  CONSTRAINT [FK_tError_tErrorType] FOREIGN KEY([TypeID])
REFERENCES [dbo].[tErrorType] ([ErrorTypeID])
GO
ALTER TABLE [dbo].[tError] CHECK CONSTRAINT [FK_tError_tErrorType]
GO

insert into DBVersion (DBVersion) values (0)