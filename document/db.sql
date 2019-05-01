USE [APIServer]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 5/1/2019 9:20:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TBL_COUNTRY]    Script Date: 5/1/2019 9:20:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_COUNTRY](
	[CNY_CD] [nvarchar](128) NOT NULL,
	[REGION] [nvarchar](2048) NULL,
	[CNY_NA] [nvarchar](2048) NOT NULL,
	[CCY_SIG] [nvarchar](128) NOT NULL,
	[CCY_NA] [nvarchar](2048) NOT NULL,
 CONSTRAINT [PK_TBL_COUNTRY] PRIMARY KEY CLUSTERED 
(
	[CNY_CD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TBL_GROUP]    Script Date: 5/1/2019 9:20:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_GROUP](
	[ID] [uniqueidentifier] NOT NULL,
	[CREATED_USER] [nvarchar](2048) NULL,
	[CREATED_DT] [datetime2](7) NULL,
	[LAST_UDT_USER] [nvarchar](2048) NULL,
	[LAST_UDT_DT] [datetime2](7) NULL,
	[GROUP_CD] [nvarchar](128) NULL,
	[GROUP_NA] [nvarchar](2048) NULL,
 CONSTRAINT [PK_TBL_GROUP] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TBL_LOG_WORK]    Script Date: 5/1/2019 9:20:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_LOG_WORK](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[Level] [nvarchar](max) NULL,
	[TimeStamp] [datetime2](7) NOT NULL,
	[Exception] [nvarchar](max) NULL,
	[LogEvent] [nvarchar](max) NULL,
 CONSTRAINT [PK_TBL_LOG_WORK] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TBL_SYS_CONFIG]    Script Date: 5/1/2019 9:20:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_SYS_CONFIG](
	[ID] [uniqueidentifier] NOT NULL,
	[CREATED_USER] [nvarchar](2048) NULL,
	[CREATED_DT] [datetime2](7) NULL,
	[LAST_UDT_USER] [nvarchar](2048) NULL,
	[LAST_UDT_DT] [datetime2](7) NULL,
	[KEY] [nvarchar](2048) NULL,
	[VALUE] [nvarchar](2048) NULL,
	[VALUE_UNIT] [nvarchar](2048) NULL,
 CONSTRAINT [PK_TBL_SYS_CONFIG] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TBL_USER]    Script Date: 5/1/2019 9:20:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_USER](
	[ID] [uniqueidentifier] NOT NULL,
	[CREATED_USER] [nvarchar](2048) NULL,
	[CREATED_DT] [datetime2](7) NULL,
	[LAST_UDT_USER] [nvarchar](2048) NULL,
	[LAST_UDT_DT] [datetime2](7) NULL,
	[LOGIN_FAILED_NR] [int] NULL,
	[TOKEN] [nvarchar](2048) NULL,
	[SUBCRISE_TOKEN] [nvarchar](2048) NULL,
	[TOKEN_EXPIRED_DT] [datetime2](7) NULL,
	[LOGIN_TM] [datetime2](7) NULL,
	[PASSWORD] [nvarchar](1024) NOT NULL,
	[PASSWORD_LAST_UDT] [datetime2](7) NULL,
	[USERNAME] [nvarchar](2048) NOT NULL,
	[CNY_CD] [nvarchar](128) NULL,
	[CODE] [nvarchar](128) NOT NULL,
	[FULL_NAME] [nvarchar](2048) NOT NULL,
	[USER_TYP] [nvarchar](2048) NOT NULL,
	[GROUP_UND_SF] [nvarchar](max) NULL,
	[USERS_UND_MN] [nvarchar](max) NULL,
	[STATUS] [nvarchar](2048) NOT NULL,
	[END_OF_DAY] [bit] NOT NULL,
	[ADDRESS] [nvarchar](2048) NOT NULL,
	[EMAIL] [nvarchar](2048) NULL,
	[PHONE] [nvarchar](128) NOT NULL,
	[START_DT] [datetime2](7) NULL,
	[EXPIRED_DT] [datetime2](7) NULL,
 CONSTRAINT [PK_TBL_USER] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20190429084725_Initial_Database', N'2.2.4-servicing-10062')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'AS', N'APAC', N'Samoa (American)', N'$', N'USD')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'AU', N'APAC', N'Australia', N'$', N'AUD')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'BN', N'APAC', N'Brunei', N'B$', N'BND')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'BT', N'APAC', N'Bhutan', N'Nu', N'BTN')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'HK', N'APAC', N'Hong Kong', N'₣', N'HKD')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'ID', N'APAC', N'Indonesia', N'Rp', N'IDR')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'JP', N'APAC', N'Japan', N'¥', N'JPY')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'KH', N'APAC', N'Kampuchea', N'CR', N'KHR')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'KR', N'APAC', N'Korea (South)', N'₩', N'KRW')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'LA', N'APAC', N'Laos', N'₭', N'LAK')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'MM', N'APAC', N'Myanmar', N'K', N'MMK')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'MN', N'APAC', N'Mongolia', N'₮', N'MNT')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'MO', N'APAC', N'Macao', N'MOP$', N'MOP')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'MY', N'APAC', N'Malaysia', N'RM', N'MYR')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'NC', N'APAC', N'New Caledonia', N'₣', N'CFP')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'NR', N'APAC', N'Nauru', N'$', N'AUD')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'NU', N'APAC', N'Niue', N'$', N'NZD')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'NZ', N'APAC', N'New Zealand', N'$', N'NZD')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'P2', N'APAC', N'French Polynesia & New Caledonia', N'₣', N'CFP')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'PF', N'APAC', N'Polynesia (French)', N'₣', N'CFP')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'PH', N'APAC', N'Philippines', N'₱', N'PHP')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'PN', N'APAC', N'Pitcairn', N'$', N'NZD')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'PW', N'APAC', N'Palau', N'$', N'USD')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'SG', N'APAC', N'Singapore', N'$', N'SGD')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'TH', N'APAC', N'Thai Lan', N'', N'Baht')
INSERT [dbo].[TBL_COUNTRY] ([CNY_CD], [REGION], [CNY_NA], [CCY_SIG], [CCY_NA]) VALUES (N'VN', N'APAC', N'Viet Nam', N'₫', N'VND')
INSERT [dbo].[TBL_GROUP] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [GROUP_CD], [GROUP_NA]) VALUES (N'fdbe4fb8-a2f9-4694-245b-08d6c19a366a', N'Auto-Create', CAST(N'2019-04-15 19:02:20.1454004' AS DateTime2), NULL, CAST(N'2019-04-15 19:02:20.1454004' AS DateTime2), N'GG', N'GG')
INSERT [dbo].[TBL_GROUP] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [GROUP_CD], [GROUP_NA]) VALUES (N'e994148d-1d51-4fd8-245c-08d6c19a366a', N'Auto-Create', CAST(N'2019-04-15 19:02:20.1454004' AS DateTime2), NULL, CAST(N'2019-04-15 19:02:20.1454004' AS DateTime2), N'VG', N'VG')
INSERT [dbo].[TBL_GROUP] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [GROUP_CD], [GROUP_NA]) VALUES (N'95d313cb-a2b1-4a15-245d-08d6c19a366a', N'Auto-Create', CAST(N'2019-04-15 19:02:20.1454004' AS DateTime2), NULL, CAST(N'2019-04-15 19:02:20.1454004' AS DateTime2), N'GD', N'GD')
INSERT [dbo].[TBL_GROUP] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [GROUP_CD], [GROUP_NA]) VALUES (N'fbf4f867-442e-4123-245e-08d6c19a366a', N'Auto-Create', CAST(N'2019-04-15 19:02:20.1454004' AS DateTime2), NULL, CAST(N'2019-04-15 19:02:20.1454004' AS DateTime2), N'MD', N'MD')
INSERT [dbo].[TBL_GROUP] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [GROUP_CD], [GROUP_NA]) VALUES (N'bc73d6ef-2ecc-4b4d-245f-08d6c19a366a', N'Auto-Create', CAST(N'2019-04-15 19:02:20.1454004' AS DateTime2), NULL, CAST(N'2019-04-15 19:02:20.1454004' AS DateTime2), N'BG', N'BG')
INSERT [dbo].[TBL_GROUP] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [GROUP_CD], [GROUP_NA]) VALUES (N'ccbd2812-1b1e-4f28-2460-08d6c19a366a', N'Auto-Create', CAST(N'2019-04-15 19:02:20.1454004' AS DateTime2), NULL, CAST(N'2019-04-15 19:02:20.1454004' AS DateTime2), N'BA', N'BA')
INSERT [dbo].[TBL_GROUP] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [GROUP_CD], [GROUP_NA]) VALUES (N'0df2bfdf-933f-4b83-2461-08d6c19a366a', N'Auto-Create', CAST(N'2019-04-15 19:02:20.1454004' AS DateTime2), NULL, CAST(N'2019-04-15 19:02:20.1454004' AS DateTime2), N'DF', N'DF')

INSERT [dbo].[TBL_SYS_CONFIG] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [KEY], [VALUE], [VALUE_UNIT]) VALUES (N'cabd9fcc-6693-4e93-c4d5-08d6c19a3639', N'Auto-Create', CAST(N'2019-04-15 12:02:19.8323825' AS DateTime2), NULL, CAST(N'2019-04-15 19:02:19.8313824' AS DateTime2), N'ArchiveJobHistory', N'2', N'months')
INSERT [dbo].[TBL_SYS_CONFIG] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [KEY], [VALUE], [VALUE_UNIT]) VALUES (N'f16ca148-06db-4591-c4d6-08d6c19a3639', N'Auto-Create', CAST(N'2019-04-15 12:02:19.8773851' AS DateTime2), NULL, CAST(N'2019-04-15 19:02:19.8773851' AS DateTime2), N'ArchiveLogData', N'2', N'weeks')
INSERT [dbo].[TBL_SYS_CONFIG] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [KEY], [VALUE], [VALUE_UNIT]) VALUES (N'0d814502-409c-4eb1-c4d7-08d6c19a3639', N'Auto-Create', CAST(N'2019-04-15 12:02:19.8923859' AS DateTime2), NULL, CAST(N'2019-04-15 19:02:19.8923859' AS DateTime2), N'OpenEAM', N'false', N'boolean')
INSERT [dbo].[TBL_SYS_CONFIG] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [KEY], [VALUE], [VALUE_UNIT]) VALUES (N'e449e8b6-3fa3-47ef-c4d8-08d6c19a3639', N'Auto-Create', CAST(N'2019-04-15 12:02:19.9003864' AS DateTime2), NULL, CAST(N'2019-04-15 19:02:19.9003864' AS DateTime2), N'WebPassExpDate', N'30', N'days')
INSERT [dbo].[TBL_SYS_CONFIG] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [KEY], [VALUE], [VALUE_UNIT]) VALUES (N'20288348-cef9-43f2-c4d9-08d6c19a3639', N'Auto-Create', CAST(N'2019-04-15 12:02:19.9093869' AS DateTime2), NULL, CAST(N'2019-04-15 19:02:19.9093869' AS DateTime2), N'AppPassExpDate', N'30', N'minutes')
INSERT [dbo].[TBL_SYS_CONFIG] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [KEY], [VALUE], [VALUE_UNIT]) VALUES (N'3ac503a6-0796-422f-c4da-08d6c19a3639', N'Auto-Create', CAST(N'2019-04-15 12:02:19.9213876' AS DateTime2), NULL, CAST(N'2019-04-15 19:02:19.9213876' AS DateTime2), N'WebSessExpDate', N'1', N'hour')
INSERT [dbo].[TBL_SYS_CONFIG] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [KEY], [VALUE], [VALUE_UNIT]) VALUES (N'd2d5322a-0694-4b1e-c4db-08d6c19a3639', N'Auto-Create', CAST(N'2019-04-15 12:02:19.9393886' AS DateTime2), NULL, CAST(N'2019-04-15 19:02:19.9393886' AS DateTime2), N'AppSessExpDate', N'1', N'hour')

INSERT [dbo].[TBL_USER] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [LOGIN_FAILED_NR], [TOKEN], [SUBCRISE_TOKEN], [TOKEN_EXPIRED_DT], [LOGIN_TM], [PASSWORD], [PASSWORD_LAST_UDT], [USERNAME], [CNY_CD], [CODE], [FULL_NAME], [USER_TYP], [GROUP_UND_SF], [USERS_UND_MN], [STATUS], [END_OF_DAY], [ADDRESS], [EMAIL], [PHONE], [START_DT], [EXPIRED_DT]) VALUES (N'588e1d9a-f390-442b-1cf1-08d6c1a589d5', N'Auto-Create', CAST(N'2019-04-15 20:23:24.4623586' AS DateTime2), NULL, CAST(N'2019-04-15 20:23:24.4623586' AS DateTime2), 0, N'313c48a5-1b28-4bdb-ac84-b90b04be29df', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJTeXN0ZW0iLCJVc2VybmFtZSI6IlN5c3RlbSIsIkZ1bGxuYW1lIjoiU3lzdGVtQWRtaW4iLCJDb3VudHJ5IjoiQUxMIiwiR3JvdXAiOiJBbGwiLCJVc2VyVHlwZSI6IlN1cGVyQWRtaW4iLCJFeHBpcmVkUGFzc3dvcmQiOiIxNSIsIlVzZXJJZCI6IjU4OGUxZDlhLWYzOTAtNDQyYi0xY2YxLTA4ZDZjMWE1ODlkNSIsImp0aSI6IjMxM2M0OGE1LTFiMjgtNGJkYi1hYzg0LWI5MGIwNGJlMjlkZiIsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDkzOTMvIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo0OTM5My8ifQ._GPToLL05R8_n0mOxiJwyNbO0EIAQYPqPBawFOha-D8', NULL, CAST(N'2019-04-30 16:05:59.1680398' AS DateTime2), N'26vTj5NJ2+bA1qh5cg3oAA==', CAST(N'2019-04-15 20:23:24.4623586' AS DateTime2), N'System', NULL, N'SA:MTAwMDAx:U', N'SystemAdmin', N'SuperAdmin', N'All', N'All', N'Active', 0, N'NA', N'ges.api@gmail.com', N'NA', CAST(N'2019-04-15 20:23:24.4623586' AS DateTime2), CAST(N'2019-10-15 20:23:24.4623586' AS DateTime2))
INSERT [dbo].[TBL_USER] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [LOGIN_FAILED_NR], [TOKEN], [SUBCRISE_TOKEN], [TOKEN_EXPIRED_DT], [LOGIN_TM], [PASSWORD], [PASSWORD_LAST_UDT], [USERNAME], [CNY_CD], [CODE], [FULL_NAME], [USER_TYP], [GROUP_UND_SF], [USERS_UND_MN], [STATUS], [END_OF_DAY], [ADDRESS], [EMAIL], [PHONE], [START_DT], [EXPIRED_DT]) VALUES (N'1e6550d8-8518-4b14-1cf2-08d6c1a589d5', N'Auto-Create', CAST(N'2019-04-15 20:23:24.4883601' AS DateTime2), N'string', CAST(N'2019-04-20 15:41:16.6655628' AS DateTime2), 0, N'009aaf71-cfea-4f46-869a-8a98d0867f0c', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzdHJpbmciLCJVc2VybmFtZSI6InN0cmluZyIsIkZ1bGxuYW1lIjoiU3VwZXJBZG1pbiIsIkNvdW50cnkiOiJBTEwiLCJHcm91cCI6IkFsbCIsIlVzZXJUeXBlIjoiU3VwZXJBZG1pbiIsIkV4cGlyZWRQYXNzd29yZCI6IjIxIiwiVXNlcklkIjoiMWU2NTUwZDgtODUxOC00YjE0LTFjZjItMDhkNmMxYTU4OWQ1IiwianRpIjoiMDA5YWFmNzEtY2ZlYS00ZjQ2LTg2OWEtOGE5OGQwODY3ZjBjIiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo0OTM5My8iLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjQ5MzkzLyJ9.zR9wQRxuRQK7MABpRttCsiDPYg8zcOZybokbPY71a4k', NULL, CAST(N'2019-04-29 10:07:22.5173214' AS DateTime2), N'o4CWMr74MHQNDtHjl4ykeA==', CAST(N'2019-04-20 15:41:16.6655628' AS DateTime2), N'string', NULL, N'SA:MTAwMDAy:U', N'SuperAdmin', N'SuperAdmin', N'All', N'All', N'Active', 0, N'NA', N'ges.api@gmail.com', N'NA', CAST(N'2019-04-15 20:23:24.4883601' AS DateTime2), CAST(N'2019-10-15 20:23:24.4883601' AS DateTime2))
INSERT [dbo].[TBL_USER] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [LOGIN_FAILED_NR], [TOKEN], [SUBCRISE_TOKEN], [TOKEN_EXPIRED_DT], [LOGIN_TM], [PASSWORD], [PASSWORD_LAST_UDT], [USERNAME], [CNY_CD], [CODE], [FULL_NAME], [USER_TYP], [GROUP_UND_SF], [USERS_UND_MN], [STATUS], [END_OF_DAY], [ADDRESS], [EMAIL], [PHONE], [START_DT], [EXPIRED_DT]) VALUES (N'fc685499-e0dc-46a2-9fe2-0b0fb1f0c9c4', N'System', CAST(N'2019-04-17 18:42:35.4610766' AS DateTime2), N'System', CAST(N'2019-04-30 21:14:36.6118194' AS DateTime2), NULL, NULL, NULL, NULL, NULL, N'KPKhkIJ64/g5H1sSfBpfEA==', CAST(N'2019-04-17 18:42:35.4610766' AS DateTime2), N'duyngoc', N'VN', N'EA:MTAwMDE0:M', N'Đào Duy Ngọc', N'Employee', N'GD', NULL, N'Available', 0, N'Số xx/xx/xx, Đường xxx xxx xxxx, Phường xxxx xxx xxx x Quận x, TP - Hồ Chí Minh', N'duyngoc@gmail.com', N'08xxx', CAST(N'2019-04-17 18:42:35.4380753' AS DateTime2), CAST(N'2019-10-18 00:00:00.0000000' AS DateTime2))
INSERT [dbo].[TBL_USER] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [LOGIN_FAILED_NR], [TOKEN], [SUBCRISE_TOKEN], [TOKEN_EXPIRED_DT], [LOGIN_TM], [PASSWORD], [PASSWORD_LAST_UDT], [USERNAME], [CNY_CD], [CODE], [FULL_NAME], [USER_TYP], [GROUP_UND_SF], [USERS_UND_MN], [STATUS], [END_OF_DAY], [ADDRESS], [EMAIL], [PHONE], [START_DT], [EXPIRED_DT]) VALUES (N'a43fb1f9-5853-49a2-a692-11deba19c595', N'System', CAST(N'2019-04-30 15:20:25.9225027' AS DateTime2), N'System', CAST(N'2019-04-30 16:57:20.7720930' AS DateTime2), NULL, NULL, NULL, NULL, NULL, N'hNcIfu1vlAPegRMnETWlfg==', CAST(N'2019-04-30 15:20:25.9225027' AS DateTime2), N'huonganh', N'VN', N'EA:MTAwMDE5:M', N'Đỗ Hương Anh', N'Employee', N'BG', NULL, N'Unavailable', 0, N'Số xx/xx/xx, Đường xxx xxx xxxx, Phường xxxx xxx xxx x Quận x, TP - Hồ Chí Minh', N'huonganh@gmail.com', N'08xxxxxxxx', CAST(N'2019-04-30 00:00:00.0000000' AS DateTime2), CAST(N'2019-10-30 00:00:00.0000000' AS DateTime2))
INSERT [dbo].[TBL_USER] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [LOGIN_FAILED_NR], [TOKEN], [SUBCRISE_TOKEN], [TOKEN_EXPIRED_DT], [LOGIN_TM], [PASSWORD], [PASSWORD_LAST_UDT], [USERNAME], [CNY_CD], [CODE], [FULL_NAME], [USER_TYP], [GROUP_UND_SF], [USERS_UND_MN], [STATUS], [END_OF_DAY], [ADDRESS], [EMAIL], [PHONE], [START_DT], [EXPIRED_DT]) VALUES (N'215e4b87-d942-4927-9d1c-1b87e92d5f38', N'System', CAST(N'2019-04-15 20:33:48.9980800' AS DateTime2), N'trunghieu', CAST(N'2019-04-30 15:45:52.5220236' AS DateTime2), NULL, NULL, NULL, NULL, NULL, N'QPcNduL9UpgvxrEXeP3Skw==', CAST(N'2019-04-15 20:33:48.9980800' AS DateTime2), N'trantran', N'VN', N'SF:MTAwMDA4:T', N'Trần Trân', N'Staff', N'GG', N'EA:MTAwMDEz:M,EA:MTAwMDIy:M', N'Active', 0, N'Số xx/xx/xx, Đường xxx xxx xxxx, Phường xxxx xxx xxx x Quận x, TP - Hồ Chí Minh', N'trantran.it@gmail.com', N'08xxx', CAST(N'2019-04-15 20:33:48.9700784' AS DateTime2), CAST(N'2019-10-15 20:33:48.9700784' AS DateTime2))
INSERT [dbo].[TBL_USER] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [LOGIN_FAILED_NR], [TOKEN], [SUBCRISE_TOKEN], [TOKEN_EXPIRED_DT], [LOGIN_TM], [PASSWORD], [PASSWORD_LAST_UDT], [USERNAME], [CNY_CD], [CODE], [FULL_NAME], [USER_TYP], [GROUP_UND_SF], [USERS_UND_MN], [STATUS], [END_OF_DAY], [ADDRESS], [EMAIL], [PHONE], [START_DT], [EXPIRED_DT]) VALUES (N'2e4c78f4-6fa0-434f-a355-21152bafa112', N'System', CAST(N'2019-04-15 20:36:07.0319751' AS DateTime2), N'System', CAST(N'2019-04-30 14:50:07.4676974' AS DateTime2), NULL, NULL, NULL, NULL, NULL, N'QUMdgeFijhZYDmUOX3XeYQ==', CAST(N'2019-04-15 20:36:07.0319751' AS DateTime2), N'tramanh', N'VN', N'SF:MTAwMDEx:T', N'Đồ Trâm Anh', N'Staff', N'MD', N'', N'Active', 0, N'Số xx/xx/xx, Đường xxx xxx xxxx, Phường xxxx xxx xxx x Quận x, TP - Hồ Chí Minh', N'tramanh.it@gmail.com', N'08xxx', CAST(N'2019-04-15 20:36:06.9869725' AS DateTime2), CAST(N'2019-10-15 20:36:06.9869725' AS DateTime2))
INSERT [dbo].[TBL_USER] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [LOGIN_FAILED_NR], [TOKEN], [SUBCRISE_TOKEN], [TOKEN_EXPIRED_DT], [LOGIN_TM], [PASSWORD], [PASSWORD_LAST_UDT], [USERNAME], [CNY_CD], [CODE], [FULL_NAME], [USER_TYP], [GROUP_UND_SF], [USERS_UND_MN], [STATUS], [END_OF_DAY], [ADDRESS], [EMAIL], [PHONE], [START_DT], [EXPIRED_DT]) VALUES (N'd9936853-16fc-45ac-8c24-4782e9d2d77b', N'System', CAST(N'2019-04-15 20:31:00.3524340' AS DateTime2), NULL, CAST(N'2019-04-15 20:31:00.3524340' AS DateTime2), NULL, NULL, NULL, NULL, NULL, N'xlf60007JP8DlQRvTpxw0g==', CAST(N'2019-04-15 20:31:00.3524340' AS DateTime2), N'hoangdung', N'VN', N'SF:MTAwMDA2:T', N'Hoàng Dung', N'Staff', N'VG', N'', N'Inactive', 0, N'Số xx/xx/xx, Đường xxx xxx xxxx, Phường xxxx xxx xxx x Quận x, TP - Hồ Chí Minh', N'hoangdung.it@gmail.com', N'08xxx', CAST(N'2019-04-15 20:31:00.2274268' AS DateTime2), CAST(N'2019-10-15 20:31:00.2274268' AS DateTime2))
INSERT [dbo].[TBL_USER] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [LOGIN_FAILED_NR], [TOKEN], [SUBCRISE_TOKEN], [TOKEN_EXPIRED_DT], [LOGIN_TM], [PASSWORD], [PASSWORD_LAST_UDT], [USERNAME], [CNY_CD], [CODE], [FULL_NAME], [USER_TYP], [GROUP_UND_SF], [USERS_UND_MN], [STATUS], [END_OF_DAY], [ADDRESS], [EMAIL], [PHONE], [START_DT], [EXPIRED_DT]) VALUES (N'7965b91d-b486-44e6-9e41-53ba03bf272c', N'System', CAST(N'2019-04-15 20:31:50.8333213' AS DateTime2), N'System', CAST(N'2019-04-30 14:50:12.5259867' AS DateTime2), NULL, NULL, NULL, NULL, NULL, N'xXw9B4dP3zl9ppd4dsl5eA==', CAST(N'2019-04-15 20:31:50.8333213' AS DateTime2), N'duythanh', N'VN', N'SF:MTAwMDA3:T', N'Đào Duy Thành', N'Staff', N'BA', N'EA:MTAwMDE3:M,EA:MTAwMDE2:M', N'Active', 0, N'Số xx/xx/xx, Đường xxx xxx xxxx, Phường xxxx xxx xxx x Quận x, TP - Hồ Chí Minh', N'duythanh.it@gmail.com', N'08xxx', CAST(N'2019-04-15 20:31:50.8083199' AS DateTime2), CAST(N'2019-10-15 20:31:50.8083199' AS DateTime2))
INSERT [dbo].[TBL_USER] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [LOGIN_FAILED_NR], [TOKEN], [SUBCRISE_TOKEN], [TOKEN_EXPIRED_DT], [LOGIN_TM], [PASSWORD], [PASSWORD_LAST_UDT], [USERNAME], [CNY_CD], [CODE], [FULL_NAME], [USER_TYP], [GROUP_UND_SF], [USERS_UND_MN], [STATUS], [END_OF_DAY], [ADDRESS], [EMAIL], [PHONE], [START_DT], [EXPIRED_DT]) VALUES (N'fbce05d2-aa3e-471c-a4f6-6e71b0a3730a', N'System', CAST(N'2019-04-17 18:41:56.2238324' AS DateTime2), N'trunghieu', CAST(N'2019-04-30 21:51:33.6666277' AS DateTime2), NULL, NULL, NULL, NULL, NULL, N'Nk4UnMOdLHOFYsSsCKItDw==', CAST(N'2019-04-17 18:41:56.2238324' AS DateTime2), N'ngoctoan', N'VN', N'EA:MTAwMDEz:M', N'Bạch Ngọc Toàn', N'Employee', N'GG', NULL, N'Available', 0, N'Số xx/xx/xx, Đường xxx xxx xxxx, Phường xxxx xxx xxx x Quận x, TP - Hồ Chí Minh', N'ngoctoan@gmail.com', N'08xxx', CAST(N'2019-04-17 18:41:55.7018025' AS DateTime2), CAST(N'2019-10-18 00:00:00.0000000' AS DateTime2))
INSERT [dbo].[TBL_USER] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [LOGIN_FAILED_NR], [TOKEN], [SUBCRISE_TOKEN], [TOKEN_EXPIRED_DT], [LOGIN_TM], [PASSWORD], [PASSWORD_LAST_UDT], [USERNAME], [CNY_CD], [CODE], [FULL_NAME], [USER_TYP], [GROUP_UND_SF], [USERS_UND_MN], [STATUS], [END_OF_DAY], [ADDRESS], [EMAIL], [PHONE], [START_DT], [EXPIRED_DT]) VALUES (N'd8e91dae-fe90-4f6c-8725-89bcf28f4ff8', N'System', CAST(N'2019-04-17 18:45:50.9002551' AS DateTime2), N'System', CAST(N'2019-04-30 16:52:43.6342417' AS DateTime2), NULL, NULL, NULL, NULL, NULL, N'FpzwhIMR69cqIwdh1ALALQ==', CAST(N'2019-04-17 18:45:50.9002551' AS DateTime2), N'ngocthanh', N'VN', N'EA:MTAwMDE3:M', N'Đỗ Ngọc Thành', N'Employee', N'BA', NULL, N'Unavailable', 0, N'Số xx/xx/xx, Đường xxx xxx xxxx, Phường xxxx xxx xxx x Quận x, TP - Hồ Chí Minh', N'ngocthanh@gmail.com', N'08xxx', CAST(N'2019-04-17 18:45:50.8572527' AS DateTime2), CAST(N'2019-10-18 00:00:00.0000000' AS DateTime2))
INSERT [dbo].[TBL_USER] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [LOGIN_FAILED_NR], [TOKEN], [SUBCRISE_TOKEN], [TOKEN_EXPIRED_DT], [LOGIN_TM], [PASSWORD], [PASSWORD_LAST_UDT], [USERNAME], [CNY_CD], [CODE], [FULL_NAME], [USER_TYP], [GROUP_UND_SF], [USERS_UND_MN], [STATUS], [END_OF_DAY], [ADDRESS], [EMAIL], [PHONE], [START_DT], [EXPIRED_DT]) VALUES (N'a6277221-9762-434c-8723-907a65837764', N'System', CAST(N'2019-04-17 18:43:42.5859160' AS DateTime2), N'System', CAST(N'2019-04-30 16:38:50.3305794' AS DateTime2), NULL, NULL, NULL, NULL, NULL, N'nakf9JalOvUwt37H2adiQQ==', CAST(N'2019-04-17 18:43:42.5859160' AS DateTime2), N'thanhtan2', N'VN', N'EA:MTAwMDE1:M', N'Đỗ Thanh Tân', N'Employee', N'VG', NULL, N'Available', 0, N'Số xx/xx/xx, Đường xxx xxx xxxx, Phường xxxx xxx xxx x Quận x, TP - Hồ Chí Minh', N'thanhtan2@gmail.com', N'0819607196', CAST(N'2019-04-17 18:43:42.5599145' AS DateTime2), CAST(N'2019-10-18 00:00:00.0000000' AS DateTime2))
INSERT [dbo].[TBL_USER] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [LOGIN_FAILED_NR], [TOKEN], [SUBCRISE_TOKEN], [TOKEN_EXPIRED_DT], [LOGIN_TM], [PASSWORD], [PASSWORD_LAST_UDT], [USERNAME], [CNY_CD], [CODE], [FULL_NAME], [USER_TYP], [GROUP_UND_SF], [USERS_UND_MN], [STATUS], [END_OF_DAY], [ADDRESS], [EMAIL], [PHONE], [START_DT], [EXPIRED_DT]) VALUES (N'6d829494-f86d-47f2-81b7-ada7ef0d6435', N'System', CAST(N'2019-04-15 20:25:52.4008202' AS DateTime2), N'System', CAST(N'2019-04-30 09:58:21.4255622' AS DateTime2), 0, NULL, NULL, NULL, NULL, N'o4CWMr74MHQNDtHjl4ykeA==', CAST(N'2019-04-15 20:25:52.4008202' AS DateTime2), N'trunghieu', N'VN', N'MA:MTAwMDAz:G', N'Lương Trung Hiếu', N'Manager', N'GG, GD', N'SF:MTAwMDA4:T,SF:MTAwMDA5:T', N'Active', 0, N'No-Address', N'trunghieu.student.it@gmail.com', N'No-Phone', CAST(N'2019-04-15 20:25:52.2198098' AS DateTime2), CAST(N'2019-10-16 00:00:00.0000000' AS DateTime2))
INSERT [dbo].[TBL_USER] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [LOGIN_FAILED_NR], [TOKEN], [SUBCRISE_TOKEN], [TOKEN_EXPIRED_DT], [LOGIN_TM], [PASSWORD], [PASSWORD_LAST_UDT], [USERNAME], [CNY_CD], [CODE], [FULL_NAME], [USER_TYP], [GROUP_UND_SF], [USERS_UND_MN], [STATUS], [END_OF_DAY], [ADDRESS], [EMAIL], [PHONE], [START_DT], [EXPIRED_DT]) VALUES (N'6e6943f4-7350-4a76-8dbb-b1089b18b106', N'System', CAST(N'2019-04-15 20:30:04.5542425' AS DateTime2), NULL, CAST(N'2019-04-15 20:30:04.5542425' AS DateTime2), NULL, NULL, NULL, NULL, NULL, N'jqMyOmjPgULXwhHgPgKztg==', CAST(N'2019-04-15 20:30:04.5542425' AS DateTime2), N'toantran', N'VN', N'SF:MTAwMDA1:T', N'Trần Ngọc Toàn', N'Staff', N'BA', N'', N'Active', 0, N'Số xx/xx/xx, Đường xxx xxx xxxx, Phường xxxx xxx xxx x Quận x, TP - Hồ Chí Minh', N'toantran.it@gmail.com', N'0819607196', CAST(N'2019-04-15 20:30:04.4832385' AS DateTime2), CAST(N'2019-10-15 20:30:04.4832385' AS DateTime2))
INSERT [dbo].[TBL_USER] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [LOGIN_FAILED_NR], [TOKEN], [SUBCRISE_TOKEN], [TOKEN_EXPIRED_DT], [LOGIN_TM], [PASSWORD], [PASSWORD_LAST_UDT], [USERNAME], [CNY_CD], [CODE], [FULL_NAME], [USER_TYP], [GROUP_UND_SF], [USERS_UND_MN], [STATUS], [END_OF_DAY], [ADDRESS], [EMAIL], [PHONE], [START_DT], [EXPIRED_DT]) VALUES (N'd9aa3bd0-bf0e-4519-a675-bb1ba174ade4', N'System', CAST(N'2019-04-15 20:28:46.0537526' AS DateTime2), N'System', CAST(N'2019-04-30 09:58:23.7916975' AS DateTime2), NULL, NULL, NULL, NULL, NULL, N'5KnSKH4IWlgiQtCuEFTw1g==', CAST(N'2019-04-15 20:28:46.0537526' AS DateTime2), N'viethoang', N'VN', N'MA:MTAwMDA0:G', N'Lương Việt Hoàng', N'Manager', N'VG, BA', N'SF:MTAwMDA2:T,SF:MTAwMDA3:T,SF:MTAwMDA1:T', N'Active', 0, N'No-Address', N'viethoang.student.it@gmail.com', N'No-Phone', CAST(N'2019-04-15 20:28:46.0037497' AS DateTime2), CAST(N'2019-04-19 00:00:00.0000000' AS DateTime2))
INSERT [dbo].[TBL_USER] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [LOGIN_FAILED_NR], [TOKEN], [SUBCRISE_TOKEN], [TOKEN_EXPIRED_DT], [LOGIN_TM], [PASSWORD], [PASSWORD_LAST_UDT], [USERNAME], [CNY_CD], [CODE], [FULL_NAME], [USER_TYP], [GROUP_UND_SF], [USERS_UND_MN], [STATUS], [END_OF_DAY], [ADDRESS], [EMAIL], [PHONE], [START_DT], [EXPIRED_DT]) VALUES (N'4bdf7277-7b3d-4030-bc7f-c7f5901e0a41', N'System', CAST(N'2019-04-18 04:21:56.2432797' AS DateTime2), NULL, CAST(N'2019-04-18 04:21:56.2432797' AS DateTime2), NULL, NULL, NULL, NULL, NULL, N'QGdtdRyzx650/PrSzRjOIA==', CAST(N'2019-04-18 04:21:56.2432797' AS DateTime2), N'daonguyen', N'VN', N'EA:MTAwMDE4:M', N'Nguyễn Thị Đào', N'Employee', N'DF', NULL, N'Unavailable', 0, N'Số xx/xx/xx, Đường xxx xxx xxxx, Phường xxxx xxx xxx x Quận x, TP - Hồ Chí Minh', N'daonguyen@gmailcom', N'08xxx', CAST(N'2019-04-18 04:21:55.9962655' AS DateTime2), CAST(N'2019-10-18 04:21:55.9962655' AS DateTime2))
INSERT [dbo].[TBL_USER] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [LOGIN_FAILED_NR], [TOKEN], [SUBCRISE_TOKEN], [TOKEN_EXPIRED_DT], [LOGIN_TM], [PASSWORD], [PASSWORD_LAST_UDT], [USERNAME], [CNY_CD], [CODE], [FULL_NAME], [USER_TYP], [GROUP_UND_SF], [USERS_UND_MN], [STATUS], [END_OF_DAY], [ADDRESS], [EMAIL], [PHONE], [START_DT], [EXPIRED_DT]) VALUES (N'239c375f-3588-4bce-9ef2-cbb8d3cc08a9', N'System', CAST(N'2019-04-17 18:44:12.5126277' AS DateTime2), NULL, CAST(N'2019-04-17 18:44:12.5126277' AS DateTime2), 1, NULL, NULL, NULL, NULL, N'm8Vanz0qUsj6aCAAJWRzug==', CAST(N'2019-04-17 18:44:12.5126277' AS DateTime2), N'baonguyen', N'VN', N'EA:MTAwMDE2:M', N'Trần Bảo Nguyên', N'Employee', N'BA', NULL, N'Available', 0, N'Số xx/xx/xx, Đường xxx xxx xxxx, Phường xxxx xxx xxx x Quận x, TP - Hồ Chí Minh', N'baonguyen@gmail.com', N'08xxx', CAST(N'2019-04-17 18:44:12.4806258' AS DateTime2), CAST(N'2019-10-17 18:44:12.4806258' AS DateTime2))
INSERT [dbo].[TBL_USER] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [LOGIN_FAILED_NR], [TOKEN], [SUBCRISE_TOKEN], [TOKEN_EXPIRED_DT], [LOGIN_TM], [PASSWORD], [PASSWORD_LAST_UDT], [USERNAME], [CNY_CD], [CODE], [FULL_NAME], [USER_TYP], [GROUP_UND_SF], [USERS_UND_MN], [STATUS], [END_OF_DAY], [ADDRESS], [EMAIL], [PHONE], [START_DT], [EXPIRED_DT]) VALUES (N'b4db5a00-ed20-49b8-b482-d875096c922e', N'System', CAST(N'2019-04-30 15:28:45.5190779' AS DateTime2), N'System', CAST(N'2019-04-30 16:58:02.3594717' AS DateTime2), NULL, NULL, NULL, NULL, NULL, N'qVvF6mF8aYIN1wCM1PyqIg==', CAST(N'2019-04-30 15:28:45.5190779' AS DateTime2), N'ngocduy', N'VN', N'EA:MTAwMDIw:M', N'Lê Ngọc Duy', N'Employee', N'VG', NULL, N'Available', 0, N'Số xx/xx/xx, Đường xxx xxx xxxx, Phường xxxx xxx xxx x Quận x, TP - Hồ Chí Minh', N'ngocduy@gmail.com', N'08xxxxxxxx', CAST(N'2019-04-30 00:00:00.0000000' AS DateTime2), CAST(N'2020-01-02 00:00:00.0000000' AS DateTime2))
INSERT [dbo].[TBL_USER] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [LOGIN_FAILED_NR], [TOKEN], [SUBCRISE_TOKEN], [TOKEN_EXPIRED_DT], [LOGIN_TM], [PASSWORD], [PASSWORD_LAST_UDT], [USERNAME], [CNY_CD], [CODE], [FULL_NAME], [USER_TYP], [GROUP_UND_SF], [USERS_UND_MN], [STATUS], [END_OF_DAY], [ADDRESS], [EMAIL], [PHONE], [START_DT], [EXPIRED_DT]) VALUES (N'2b07a9cd-45eb-4fd7-b899-dab82235dbf9', N'System', CAST(N'2019-04-30 17:07:08.0496834' AS DateTime2), NULL, CAST(N'2019-04-30 17:07:08.0496834' AS DateTime2), NULL, NULL, NULL, NULL, NULL, N'0BRc5Q39ZIRoEP8/wCYUmw==', CAST(N'2019-04-30 17:07:08.0496834' AS DateTime2), N'nguyendo', N'VN', N'MA:MTAwMDIx:G', N'Đỗ Trần Bảo Nguyên', N'Manager', N'VG, BG', N'SF:MTAwMDEw:T', N'Active', 0, N'No-Address', N'nguyendo@gmail.com', N'No-Phone', CAST(N'2019-04-30 00:00:00.0000000' AS DateTime2), CAST(N'2019-10-30 00:00:00.0000000' AS DateTime2))
INSERT [dbo].[TBL_USER] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [LOGIN_FAILED_NR], [TOKEN], [SUBCRISE_TOKEN], [TOKEN_EXPIRED_DT], [LOGIN_TM], [PASSWORD], [PASSWORD_LAST_UDT], [USERNAME], [CNY_CD], [CODE], [FULL_NAME], [USER_TYP], [GROUP_UND_SF], [USERS_UND_MN], [STATUS], [END_OF_DAY], [ADDRESS], [EMAIL], [PHONE], [START_DT], [EXPIRED_DT]) VALUES (N'491794d1-af40-4936-a106-e62f1a70535a', N'System', CAST(N'2019-04-15 20:34:33.7566400' AS DateTime2), N'System', CAST(N'2019-04-30 14:50:16.6262212' AS DateTime2), 0, NULL, NULL, NULL, NULL, N'o4CWMr74MHQNDtHjl4ykeA==', CAST(N'2019-04-15 20:34:33.7566400' AS DateTime2), N'baongoc', N'VN', N'SF:MTAwMDA5:T', N'Đồ Bảo Ngọc', N'Staff', N'GD', N'EA:MTAwMDE0:M', N'Active', 0, N'Số xx/xx/xx, Đường xxx xxx xxxx, Phường xxxx xxx xxx x Quận x, TP - Hồ Chí Minh', N'baongoc.it@gmail.com', N'08xxx', CAST(N'2019-04-15 20:34:33.6936364' AS DateTime2), CAST(N'2019-10-15 20:34:33.6936364' AS DateTime2))
INSERT [dbo].[TBL_USER] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [LOGIN_FAILED_NR], [TOKEN], [SUBCRISE_TOKEN], [TOKEN_EXPIRED_DT], [LOGIN_TM], [PASSWORD], [PASSWORD_LAST_UDT], [USERNAME], [CNY_CD], [CODE], [FULL_NAME], [USER_TYP], [GROUP_UND_SF], [USERS_UND_MN], [STATUS], [END_OF_DAY], [ADDRESS], [EMAIL], [PHONE], [START_DT], [EXPIRED_DT]) VALUES (N'c2ec8ee8-bcc2-4663-b5c1-eb0b7217dc53', N'System', CAST(N'2019-04-15 20:42:37.3633007' AS DateTime2), N'System', CAST(N'2019-04-30 14:50:19.4643836' AS DateTime2), NULL, NULL, NULL, NULL, NULL, N'6VoT4z+LakVLdzRwgwrz4w==', CAST(N'2019-04-15 20:42:37.3633007' AS DateTime2), N'thanhtan', N'VN', N'SF:MTAwMDEy:T', N'Nguyễn Thanh Tần', N'Staff', N'DF', N'EA:MTAwMDE4:M', N'Active', 0, N'Số xx/xx/xx, Đường xxx xxx xxxx, Phường xxxx xxx xxx x Quận x, TP - Hồ Chí Minh', N'thanhtan.it@gmail.com', N'08xxx', CAST(N'2019-04-15 20:42:37.3352991' AS DateTime2), CAST(N'2019-10-15 20:42:37.3352991' AS DateTime2))
INSERT [dbo].[TBL_USER] ([ID], [CREATED_USER], [CREATED_DT], [LAST_UDT_USER], [LAST_UDT_DT], [LOGIN_FAILED_NR], [TOKEN], [SUBCRISE_TOKEN], [TOKEN_EXPIRED_DT], [LOGIN_TM], [PASSWORD], [PASSWORD_LAST_UDT], [USERNAME], [CNY_CD], [CODE], [FULL_NAME], [USER_TYP], [GROUP_UND_SF], [USERS_UND_MN], [STATUS], [END_OF_DAY], [ADDRESS], [EMAIL], [PHONE], [START_DT], [EXPIRED_DT]) VALUES (N'55415d64-9e04-43b0-9268-fb69a35d57c5', N'System', CAST(N'2019-04-15 20:35:27.4517112' AS DateTime2), N'System', CAST(N'2019-04-30 16:05:59.1280375' AS DateTime2), NULL, NULL, NULL, NULL, NULL, N'ZzsL+hPT1Yc+zocglheDbg==', CAST(N'2019-04-15 20:35:27.4517112' AS DateTime2), N'baoanh', N'VN', N'SF:MTAwMDEw:T', N'Đồ Bảo Anh', N'Staff', N'BG', N'EA:MTAwMDE5:M', N'Active', 0, N'Số xx/xx/xx, Đường xxx xxx xxxx, Phường xxxx xxx xxx x Quận x, TP - Hồ Chí Minh', N'baoanh.it@gmail.com', N'08xxx', CAST(N'2019-04-15 20:35:27.3597059' AS DateTime2), CAST(N'2019-10-15 20:35:27.3597059' AS DateTime2))
ALTER TABLE [dbo].[TBL_USER]  WITH CHECK ADD  CONSTRAINT [FK_TBL_USER_TBL_COUNTRY_CNY_CD] FOREIGN KEY([CNY_CD])
REFERENCES [dbo].[TBL_COUNTRY] ([CNY_CD])
GO
ALTER TABLE [dbo].[TBL_USER] CHECK CONSTRAINT [FK_TBL_USER_TBL_COUNTRY_CNY_CD]
GO
