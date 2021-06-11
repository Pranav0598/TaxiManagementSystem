USE [master]
GO
/****** Object:  Database [TaxiManagementSystem]    Script Date: 6/12/2021 1:31:22 AM ******/
CREATE DATABASE [TaxiManagementSystem]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TaxiManagementSystem', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\TaxiManagementSystem.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'TaxiManagementSystem_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\TaxiManagementSystem_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [TaxiManagementSystem] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TaxiManagementSystem].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TaxiManagementSystem] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TaxiManagementSystem] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TaxiManagementSystem] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TaxiManagementSystem] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TaxiManagementSystem] SET ARITHABORT OFF 
GO
ALTER DATABASE [TaxiManagementSystem] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TaxiManagementSystem] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TaxiManagementSystem] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TaxiManagementSystem] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TaxiManagementSystem] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TaxiManagementSystem] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TaxiManagementSystem] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TaxiManagementSystem] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TaxiManagementSystem] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TaxiManagementSystem] SET  DISABLE_BROKER 
GO
ALTER DATABASE [TaxiManagementSystem] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TaxiManagementSystem] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TaxiManagementSystem] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TaxiManagementSystem] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TaxiManagementSystem] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TaxiManagementSystem] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TaxiManagementSystem] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TaxiManagementSystem] SET RECOVERY FULL 
GO
ALTER DATABASE [TaxiManagementSystem] SET  MULTI_USER 
GO
ALTER DATABASE [TaxiManagementSystem] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TaxiManagementSystem] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TaxiManagementSystem] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TaxiManagementSystem] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [TaxiManagementSystem] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [TaxiManagementSystem] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'TaxiManagementSystem', N'ON'
GO
ALTER DATABASE [TaxiManagementSystem] SET QUERY_STORE = OFF
GO
USE [TaxiManagementSystem]
GO
/****** Object:  Table [dbo].[CarMake]    Script Date: 6/12/2021 1:31:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CarMake](
	[MakeId] [int] IDENTITY(1,1) NOT NULL,
	[Make] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MakeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CarModel]    Script Date: 6/12/2021 1:31:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CarModel](
	[ModelId] [int] IDENTITY(1,1) NOT NULL,
	[Model] [varchar](255) NOT NULL,
	[MakeId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ModelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Driver]    Script Date: 6/12/2021 1:31:22 AM ******/
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
	[IsActive] [bit] NULL,
	[IsAvailable] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[DriverId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Earnings]    Script Date: 6/12/2021 1:31:22 AM ******/
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
	[TaxiId] [int] NOT NULL,
	[DriverId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[EarningId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Owner]    Script Date: 6/12/2021 1:31:22 AM ******/
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
/****** Object:  Table [dbo].[OwnerDriver]    Script Date: 6/12/2021 1:31:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OwnerDriver](
	[OwnerDriverId] [int] IDENTITY(1,1) NOT NULL,
	[OwnerId] [int] NOT NULL,
	[DriverId] [int] NOT NULL,
	[IsActiveDriver] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[OwnerDriverId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Schedule]    Script Date: 6/12/2021 1:31:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Schedule](
	[ScheduleId] [int] IDENTITY(1,1) NOT NULL,
	[Comments] [varchar](250) NULL,
	[DriverId] [int] NOT NULL,
	[TaxiId] [int] NOT NULL,
	[ShiftTime] [datetime] NULL,
	[ShiftTimeEnd] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ScheduleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Taxi]    Script Date: 6/12/2021 1:31:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Taxi](
	[TaxiId] [int] IDENTITY(1,1) NOT NULL,
	[Model] [int] NOT NULL,
	[Make] [int] NOT NULL,
	[Registration] [varchar](255) NOT NULL,
	[Comments] [varchar](255) NULL,
	[IsWorking] [bit] NULL,
	[RegoExpiry] [datetime] NULL,
	[Report] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[TaxiId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 6/12/2021 1:31:22 AM ******/
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
	[IsOwner] [bit] NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OwnerDriver] ADD  DEFAULT ((1)) FOR [IsActiveDriver]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [IsOwner]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[CarModel]  WITH CHECK ADD FOREIGN KEY([MakeId])
REFERENCES [dbo].[CarMake] ([MakeId])
GO
ALTER TABLE [dbo].[Driver]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Earnings]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Earnings]  WITH CHECK ADD  CONSTRAINT [FK_DriverEarnings] FOREIGN KEY([DriverId])
REFERENCES [dbo].[Driver] ([DriverId])
GO
ALTER TABLE [dbo].[Earnings] CHECK CONSTRAINT [FK_DriverEarnings]
GO
ALTER TABLE [dbo].[Earnings]  WITH CHECK ADD  CONSTRAINT [FK_EarningTaxi] FOREIGN KEY([TaxiId])
REFERENCES [dbo].[Taxi] ([TaxiId])
GO
ALTER TABLE [dbo].[Earnings] CHECK CONSTRAINT [FK_EarningTaxi]
GO
ALTER TABLE [dbo].[Owner]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[OwnerDriver]  WITH CHECK ADD FOREIGN KEY([DriverId])
REFERENCES [dbo].[Driver] ([DriverId])
GO
ALTER TABLE [dbo].[OwnerDriver]  WITH CHECK ADD FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Owner] ([OwnerId])
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD FOREIGN KEY([DriverId])
REFERENCES [dbo].[Driver] ([DriverId])
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD FOREIGN KEY([TaxiId])
REFERENCES [dbo].[Taxi] ([TaxiId])
GO
ALTER TABLE [dbo].[Taxi]  WITH CHECK ADD FOREIGN KEY([Make])
REFERENCES [dbo].[CarMake] ([MakeId])
GO
ALTER TABLE [dbo].[Taxi]  WITH CHECK ADD FOREIGN KEY([Model])
REFERENCES [dbo].[CarModel] ([ModelId])
GO
USE [master]
GO
ALTER DATABASE [TaxiManagementSystem] SET  READ_WRITE 
GO
