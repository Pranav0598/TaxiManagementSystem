USE [TaxiManagementSystem]
GO

/****** Object:  Table [dbo].[Users]    Script Date: 31/03/2021 11:08:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](255) NOT NULL,
	[LastName] [varchar](255) NOT NULL,
	[FirstName] [varchar](255) NULL,
	[Age] [int] NULL,
	[Email] [varchar](255) NOT NULL,
	[Password] [varchar](255) NOT NULL,
	[IsPasswordReset] [bit] NULL,
	[IsOwner] [bit] NULL DEFAULT 0,
	[IsActive] [bit] NULL DEFAULT 1,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



USE [TaxiManagementSystem]
GO

/****** Object:  Table [dbo].[Earnings]    Script Date: 31/03/2021 11:09:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Earnings](
	[EarningId] [int] IDENTITY(1,1) NOT NULL,
	[ShiftDate] [date] NOT NULL,
	[UserId] [int] NOT NULL,
	[Earning] [float] NULL,
	[Expenditure] [float] NULL,
	[IncomeEarned] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[EarningId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Earnings]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO



USE [TaxiManagementSystem]
GO

/****** Object:  Table [dbo].[Driver]    Script Date: 31/03/2021 11:09:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Driver](
	[DriverId] [int] IDENTITY(1,1) NOT NULL,
	[LastName] [varchar](255) NOT NULL,
	[FirstName] [varchar](255) NULL,
	[Age] [int] NULL,
	[Email] [varchar](255) NOT NULL,
	[Phonenumber] [int] NOT NULL,
	[DriversLicense] [varchar](50) NULL,
	[UserId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[DriverId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Driver]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO


USE [TaxiManagementSystem]
GO

/****** Object:  Table [dbo].[Driver]    Script Date: 31/03/2021 11:09:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Owner](
	[OwnerId] [int] IDENTITY(1,1) NOT NULL,
	[LastName] [varchar](255) NOT NULL,
	[FirstName] [varchar](255) NULL,
	[Email] [varchar](255) NOT NULL,
	[Phonenumber] [int] NOT NULL,
	[UserId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OwnerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Owner]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO

USE [TaxiManagementSystem]
GO

/****** Object:  Table [dbo].[Earnings]    Script Date: 1/04/2021 12:01:14 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OwnerDriver](
	[OwnerDriverId] [int] IDENTITY(1,1) NOT NULL,
	[OwnerId] [int] NOT NULL,
	[DriverId] [int] NOT NULL,	
	[IsActiveDriver] [bit] NULL DEFAULT 1,
PRIMARY KEY CLUSTERED 
(
	[OwnerDriverId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[OwnerDriver]  WITH CHECK ADD FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Owner] ([OwnerId])
GO

ALTER TABLE [dbo].[OwnerDriver]  WITH CHECK ADD FOREIGN KEY([DriverId])
REFERENCES [dbo].[Driver] ([DriverId])
GO

