
/****** Object:  Table [dbo].[InternalUser]    Script Date: 2021-06-22 09:10:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InternalUser](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[Telephone] [nvarchar](50) NULL,
	[Email] [varchar](50) NULL,
	[IsAdmin] [bit] NOT NULL,
 CONSTRAINT [PK_Staff] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProjectMaster]    Script Date: 2021-06-22 09:10:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectMaster](
	[ProjectID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectName] [varchar](100) NULL,
	[ProjectCode] [varchar](10) NULL,
 CONSTRAINT [PK_ProjectMaster] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaskDetails]    Script Date: 2021-06-22 09:10:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaskDetails](
	[TaskID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[TimeSheetID] [int] NOT NULL,
	[StartTime] [char](10) NOT NULL,
	[EndTime] [char](10) NOT NULL,
 CONSTRAINT [PK_TaskDetails] PRIMARY KEY CLUSTERED 
(
	[TaskID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TimeSheetDetails]    Script Date: 2021-06-22 09:10:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TimeSheetDetails](
	[TimeSheetID] [int] IDENTITY(1,1) NOT NULL,
	[ForDate] [date] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[StartTime] [char](5) NOT NULL,
	[EndTime] [char](5) NOT NULL,
	[LunchStartTime] [char](5) NOT NULL,
 CONSTRAINT [PK_TimeSheetDetails] PRIMARY KEY CLUSTERED 
(
	[TimeSheetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[InternalUser] ADD  CONSTRAINT [DF_InternalUser_IsAdmin]  DEFAULT ((0)) FOR [IsAdmin]
GO
ALTER TABLE [dbo].[TaskDetails]  WITH CHECK ADD  CONSTRAINT [FK_TaskDetails_TimeSheetDetails] FOREIGN KEY([TimeSheetID])
REFERENCES [dbo].[TimeSheetDetails] ([TimeSheetID])
GO
ALTER TABLE [dbo].[TaskDetails] CHECK CONSTRAINT [FK_TaskDetails_TimeSheetDetails]
GO
ALTER TABLE [dbo].[TimeSheetDetails]  WITH CHECK ADD  CONSTRAINT [FK_TimeSheetDetails_InternalUser] FOREIGN KEY([UserID])
REFERENCES [dbo].[InternalUser] ([UserId])
GO
ALTER TABLE [dbo].[TimeSheetDetails] CHECK CONSTRAINT [FK_TimeSheetDetails_InternalUser]
GO
ALTER TABLE [dbo].[TimeSheetDetails]  WITH CHECK ADD  CONSTRAINT [FK_TimeSheetDetails_ProjectMaster] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [dbo].[TimeSheetDetails] CHECK CONSTRAINT [FK_TimeSheetDetails_ProjectMaster]
GO
