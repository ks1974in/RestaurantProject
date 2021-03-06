USE [master]
GO
/****** Object:  Database [RestaurantDB]    Script Date: 23-06-2019 05:05:36 ******/
CREATE DATABASE [RestaurantDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RestaurantDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\RestaurantDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RestaurantDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\RestaurantDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [RestaurantDB] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RestaurantDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RestaurantDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RestaurantDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RestaurantDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RestaurantDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RestaurantDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [RestaurantDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [RestaurantDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RestaurantDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RestaurantDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RestaurantDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RestaurantDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RestaurantDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RestaurantDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RestaurantDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RestaurantDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [RestaurantDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RestaurantDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RestaurantDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RestaurantDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RestaurantDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RestaurantDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RestaurantDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RestaurantDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [RestaurantDB] SET  MULTI_USER 
GO
ALTER DATABASE [RestaurantDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RestaurantDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RestaurantDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RestaurantDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RestaurantDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [RestaurantDB] SET QUERY_STORE = OFF
GO
USE [RestaurantDB]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 23-06-2019 05:05:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [uniqueidentifier] NOT NULL,
	[Code] [varchar](10) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [text] NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DiningTable]    Script Date: 23-06-2019 05:05:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DiningTable](
	[Id] [uniqueidentifier] NOT NULL,
	[Number] [varchar](50) NOT NULL,
	[SeatingCapacity] [tinyint] NOT NULL,
	[Occupied] [bit] NOT NULL,
 CONSTRAINT [PK_DiningTable] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FoodOrder]    Script Date: 23-06-2019 05:05:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FoodOrder](
	[Id] [uniqueidentifier] NOT NULL,
	[TableId] [uniqueidentifier] NULL,
	[OrderNumber] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Completed] [bit] NOT NULL,
	[Billed] [bit] NOT NULL,
	[Amount] [decimal](18, 0) NOT NULL,
	[taxes] [decimal](18, 0) NOT NULL,
	[Subtotal] [decimal](18, 0) NOT NULL,
	[Discount] [decimal](18, 0) NOT NULL,
	[Total] [decimal](18, 0) NOT NULL,
	[Remarks] [text] NULL,
	[WaiterId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_FoodOrder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item]    Script Date: 23-06-2019 05:05:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item](
	[Id] [uniqueidentifier] NOT NULL,
	[CategoryId] [uniqueidentifier] NOT NULL,
	[Code] [varchar](10) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[Available] [bit] NOT NULL,
	[Description] [text] NULL,
	[UnitId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderedItem]    Script Date: 23-06-2019 05:05:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderedItem](
	[Id] [uniqueidentifier] NOT NULL,
	[OrderId] [uniqueidentifier] NULL,
	[ItemId] [uniqueidentifier] NULL,
	[Quantity] [tinyint] NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[Subtotal] [decimal](18, 0) NOT NULL,
	[Remarks] [text] NULL,
 CONSTRAINT [PK_OrderedItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 23-06-2019 05:05:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Unit]    Script Date: 23-06-2019 05:05:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Unit](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Code] [varchar](10) NOT NULL,
 CONSTRAINT [PK_Unit] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 23-06-2019 05:05:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [uniqueidentifier] NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[Enabled] [bit] NOT NULL,
	[Locked] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[MobileNumber] [varchar](15) NOT NULL,
	[EmailAddress] [varchar](50) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserSession]    Script Date: 23-06-2019 05:05:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSession](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Start] [datetime] NOT NULL,
	[End] [datetime] NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_UserSession] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WaiterTable]    Script Date: 23-06-2019 05:05:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WaiterTable](
	[Id] [uniqueidentifier] NOT NULL,
	[WaiterId] [uniqueidentifier] NOT NULL,
	[TableId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_WaiterTable_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Category] ([Id], [Code], [Name], [Description]) VALUES (N'c46c6cc0-9369-11e9-a658-3548ed1030d9', N'4', N'Condiments', NULL)
INSERT [dbo].[Category] ([Id], [Code], [Name], [Description]) VALUES (N'7bec2300-935f-11e9-8e44-d9cdafda7319', N'1', N'Patties', NULL)
INSERT [dbo].[Category] ([Id], [Code], [Name], [Description]) VALUES (N'8f398b00-935f-11e9-8e44-d9cdafda7319', N'3', N'Dressings', NULL)
INSERT [dbo].[Category] ([Id], [Code], [Name], [Description]) VALUES (N'87351550-935f-11e9-8e44-d9cdafda7319', N'2', N'Buns/Rice', NULL)
INSERT [dbo].[Category] ([Id], [Code], [Name], [Description]) VALUES (N'ace5fa80-935f-11e9-8e44-d9cdafda7319', N'8', N'Indulgence', NULL)
INSERT [dbo].[Category] ([Id], [Code], [Name], [Description]) VALUES (N'a5d051a0-935f-11e9-8e44-d9cdafda7319', N'7', N'Sauce', NULL)
INSERT [dbo].[DiningTable] ([Id], [Number], [SeatingCapacity], [Occupied]) VALUES (N'17bccd20-9365-11e9-a36a-0bad1f89eaac', N'223', 5, 0)
INSERT [dbo].[DiningTable] ([Id], [Number], [SeatingCapacity], [Occupied]) VALUES (N'10280930-9365-11e9-a36a-0bad1f89eaac', N'222', 4, 0)
INSERT [dbo].[DiningTable] ([Id], [Number], [SeatingCapacity], [Occupied]) VALUES (N'0a3adac0-9365-11e9-a36a-0bad1f89eaac', N'221', 4, 0)
INSERT [dbo].[DiningTable] ([Id], [Number], [SeatingCapacity], [Occupied]) VALUES (N'848043d0-9386-11e9-8479-6516af7fbd1e', N'224', 5, 0)
INSERT [dbo].[FoodOrder] ([Id], [TableId], [OrderNumber], [Date], [Completed], [Billed], [Amount], [taxes], [Subtotal], [Discount], [Total], [Remarks], [WaiterId]) VALUES (N'10be3940-9366-11e9-a36a-0bad1f89eaac', N'17bccd20-9365-11e9-a36a-0bad1f89eaac', 1171, CAST(N'2019-06-20T00:00:00.000' AS DateTime), 0, 0, CAST(500 AS Decimal(18, 0)), CAST(60 AS Decimal(18, 0)), CAST(475 AS Decimal(18, 0)), CAST(25 AS Decimal(18, 0)), CAST(535 AS Decimal(18, 0)), N'None', N'0af91cf0-92fd-11e9-b444-cbc6df9cac0b')
INSERT [dbo].[FoodOrder] ([Id], [TableId], [OrderNumber], [Date], [Completed], [Billed], [Amount], [taxes], [Subtotal], [Discount], [Total], [Remarks], [WaiterId]) VALUES (N'07831850-9366-11e9-a36a-0bad1f89eaac', N'0a3adac0-9365-11e9-a36a-0bad1f89eaac', 288, CAST(N'2019-06-20T00:00:00.000' AS DateTime), 0, 0, CAST(1155 AS Decimal(18, 0)), CAST(139 AS Decimal(18, 0)), CAST(1097 AS Decimal(18, 0)), CAST(58 AS Decimal(18, 0)), CAST(1236 AS Decimal(18, 0)), N'None', N'0af91cf0-92fd-11e9-b444-cbc6df9cac0b')
INSERT [dbo].[FoodOrder] ([Id], [TableId], [OrderNumber], [Date], [Completed], [Billed], [Amount], [taxes], [Subtotal], [Discount], [Total], [Remarks], [WaiterId]) VALUES (N'0d1ea450-9366-11e9-a36a-0bad1f89eaac', N'10280930-9365-11e9-a36a-0bad1f89eaac', 967, CAST(N'2019-06-20T00:00:00.000' AS DateTime), 0, 0, CAST(1730 AS Decimal(18, 0)), CAST(208 AS Decimal(18, 0)), CAST(1644 AS Decimal(18, 0)), CAST(87 AS Decimal(18, 0)), CAST(1851 AS Decimal(18, 0)), N'None', N'0af91cf0-92fd-11e9-b444-cbc6df9cac0b')
INSERT [dbo].[FoodOrder] ([Id], [TableId], [OrderNumber], [Date], [Completed], [Billed], [Amount], [taxes], [Subtotal], [Discount], [Total], [Remarks], [WaiterId]) VALUES (N'd426e3d0-9386-11e9-8479-6516af7fbd1e', N'848043d0-9386-11e9-8479-6516af7fbd1e', 1806, CAST(N'2019-06-20T18:11:25.247' AS DateTime), 0, 0, CAST(520 AS Decimal(18, 0)), CAST(62 AS Decimal(18, 0)), CAST(494 AS Decimal(18, 0)), CAST(26 AS Decimal(18, 0)), CAST(556 AS Decimal(18, 0)), N'None', N'e54132e0-92fc-11e9-b444-cbc6df9cac0b')
INSERT [dbo].[Item] ([Id], [CategoryId], [Code], [Name], [Price], [Available], [Description], [UnitId]) VALUES (N'e858e5a0-9369-11e9-a658-3548ed1030d9', N'c46c6cc0-9369-11e9-a658-3548ed1030d9', N'TOM', N'Tomatoes', CAST(10 AS Decimal(18, 0)), 1, NULL, N'dceb6a70-9360-11e9-bfc0-7dd2c67b559c')
INSERT [dbo].[Item] ([Id], [CategoryId], [Code], [Name], [Price], [Available], [Description], [UnitId]) VALUES (N'f157c0e0-9369-11e9-a658-3548ed1030d9', N'c46c6cc0-9369-11e9-a658-3548ed1030d9', N'ONI', N'Onions', CAST(10 AS Decimal(18, 0)), 1, NULL, N'dceb6a70-9360-11e9-bfc0-7dd2c67b559c')
INSERT [dbo].[Item] ([Id], [CategoryId], [Code], [Name], [Price], [Available], [Description], [UnitId]) VALUES (N'eca91c00-9360-11e9-bfc0-7dd2c67b559c', N'8f398b00-935f-11e9-8e44-d9cdafda7319', N'LU', N'Lettuce', CAST(10 AS Decimal(18, 0)), 1, NULL, N'dceb6a70-9360-11e9-bfc0-7dd2c67b559c')
INSERT [dbo].[Item] ([Id], [CategoryId], [Code], [Name], [Price], [Available], [Description], [UnitId]) VALUES (N'42f02a50-9360-11e9-bfc0-7dd2c67b559c', N'7bec2300-935f-11e9-8e44-d9cdafda7319', N'BF', N'Beef', CAST(10 AS Decimal(18, 0)), 1, NULL, N'3effe529-45c0-44c9-a758-f65b176aabb9')
INSERT [dbo].[Item] ([Id], [CategoryId], [Code], [Name], [Price], [Available], [Description], [UnitId]) VALUES (N'f92fe580-9360-11e9-bfc0-7dd2c67b559c', N'8f398b00-935f-11e9-8e44-d9cdafda7319', N'MY', N'Mayo', CAST(15 AS Decimal(18, 0)), 1, NULL, N'dceb6a70-9360-11e9-bfc0-7dd2c67b559c')
INSERT [dbo].[Item] ([Id], [CategoryId], [Code], [Name], [Price], [Available], [Description], [UnitId]) VALUES (N'b47cc4d0-9360-11e9-bfc0-7dd2c67b559c', N'87351550-935f-11e9-8e44-d9cdafda7319', N'SB45', N'4.5" Sesame Bun', CAST(150 AS Decimal(18, 0)), 1, NULL, N'dceb6a70-9360-11e9-bfc0-7dd2c67b559c')
INSERT [dbo].[Item] ([Id], [CategoryId], [Code], [Name], [Price], [Available], [Description], [UnitId]) VALUES (N'9b62fcd0-9360-11e9-bfc0-7dd2c67b559c', N'87351550-935f-11e9-8e44-d9cdafda7319', N'SB40', N'4" Seamame Bun', CAST(120 AS Decimal(18, 0)), 1, NULL, N'dceb6a70-9360-11e9-bfc0-7dd2c67b559c')
INSERT [dbo].[Item] ([Id], [CategoryId], [Code], [Name], [Price], [Available], [Description], [UnitId]) VALUES (N'50e095f0-9360-11e9-bfc0-7dd2c67b559c', N'7bec2300-935f-11e9-8e44-d9cdafda7319', N'FC', N'Fried Chicken', CAST(150 AS Decimal(18, 0)), 1, NULL, N'bb1bac80-935f-11e9-8e44-d9cdafda7319')
INSERT [dbo].[Item] ([Id], [CategoryId], [Code], [Name], [Price], [Available], [Description], [UnitId]) VALUES (N'204d5300-9361-11e9-bfc0-7dd2c67b559c', N'a5d051a0-935f-11e9-8e44-d9cdafda7319', N'SMB', N'Sambal Sauce', CAST(50 AS Decimal(18, 0)), 1, NULL, N'dceb6a70-9360-11e9-bfc0-7dd2c67b559c')
INSERT [dbo].[Item] ([Id], [CategoryId], [Code], [Name], [Price], [Available], [Description], [UnitId]) VALUES (N'30869d30-9361-11e9-bfc0-7dd2c67b559c', N'ace5fa80-935f-11e9-8e44-d9cdafda7319', N'BEF', N'Beef', CAST(200 AS Decimal(18, 0)), 1, N' ', N'dceb6a70-9360-11e9-bfc0-7dd2c67b559c')
INSERT [dbo].[Item] ([Id], [CategoryId], [Code], [Name], [Price], [Available], [Description], [UnitId]) VALUES (N'66971940-9361-11e9-bfc0-7dd2c67b559c', N'ace5fa80-935f-11e9-8e44-d9cdafda7319', N'EG', N'Egg', CAST(10 AS Decimal(18, 0)), 1, NULL, N'dceb6a70-9360-11e9-bfc0-7dd2c67b559c')
INSERT [dbo].[Item] ([Id], [CategoryId], [Code], [Name], [Price], [Available], [Description], [UnitId]) VALUES (N'707c35d0-9361-11e9-bfc0-7dd2c67b559c', N'ace5fa80-935f-11e9-8e44-d9cdafda7319', N'DC', N'Double Cheese', CAST(75 AS Decimal(18, 0)), 1, NULL, N'dceb6a70-9360-11e9-bfc0-7dd2c67b559c')
INSERT [dbo].[Item] ([Id], [CategoryId], [Code], [Name], [Price], [Available], [Description], [UnitId]) VALUES (N'5a3c8fe0-9361-11e9-bfc0-7dd2c67b559c', N'ace5fa80-935f-11e9-8e44-d9cdafda7319', N'BA', N'Bacon', CAST(250 AS Decimal(18, 0)), 1, NULL, N'dceb6a70-9360-11e9-bfc0-7dd2c67b559c')
INSERT [dbo].[Item] ([Id], [CategoryId], [Code], [Name], [Price], [Available], [Description], [UnitId]) VALUES (N'1c65ae00-9360-11e9-8e44-d9cdafda7319', N'7bec2300-935f-11e9-8e44-d9cdafda7319', N'CH', N'Chicken', CAST(10 AS Decimal(18, 0)), 1, N'Chicken', N'3effe529-45c0-44c9-a758-f65b176aabb9')
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'd3952e20-9365-11e9-a36a-0bad1f89eaac', NULL, NULL, 1, CAST(150 AS Decimal(18, 0)), CAST(150 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'a2583320-9365-11e9-a36a-0bad1f89eaac', NULL, NULL, 150, CAST(100 AS Decimal(18, 0)), CAST(15000 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'a90c8220-9365-11e9-a36a-0bad1f89eaac', NULL, NULL, 3, CAST(150 AS Decimal(18, 0)), CAST(450 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'cddec720-9365-11e9-a36a-0bad1f89eaac', NULL, NULL, 1, CAST(120 AS Decimal(18, 0)), CAST(120 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'8defbe30-9365-11e9-a36a-0bad1f89eaac', NULL, NULL, 100, CAST(100 AS Decimal(18, 0)), CAST(10000 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'5d9dca00-9366-11e9-a36a-0bad1f89eaac', N'07831850-9366-11e9-a36a-0bad1f89eaac', N'1c65ae00-9360-11e9-8e44-d9cdafda7319', 100, CAST(10 AS Decimal(18, 0)), CAST(1000 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'710522a0-9366-11e9-a36a-0bad1f89eaac', N'07831850-9366-11e9-a36a-0bad1f89eaac', N'eca91c00-9360-11e9-bfc0-7dd2c67b559c', 1, CAST(10 AS Decimal(18, 0)), CAST(10 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'672647a0-9366-11e9-a36a-0bad1f89eaac', N'07831850-9366-11e9-a36a-0bad1f89eaac', N'9b62fcd0-9360-11e9-bfc0-7dd2c67b559c', 1, CAST(120 AS Decimal(18, 0)), CAST(120 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'6e2d98a0-9366-11e9-a36a-0bad1f89eaac', N'07831850-9366-11e9-a36a-0bad1f89eaac', N'f92fe580-9360-11e9-bfc0-7dd2c67b559c', 1, CAST(15 AS Decimal(18, 0)), CAST(15 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'5694ee30-936d-11e9-aaf5-15a0b0c9f7a6', N'10be3940-9366-11e9-a36a-0bad1f89eaac', N'204d5300-9361-11e9-bfc0-7dd2c67b559c', 1, CAST(50 AS Decimal(18, 0)), CAST(50 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'3662e540-936d-11e9-aaf5-15a0b0c9f7a6', N'0d1ea450-9366-11e9-a36a-0bad1f89eaac', N'e858e5a0-9369-11e9-a658-3548ed1030d9', 1, CAST(10 AS Decimal(18, 0)), CAST(10 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'38698ab0-936d-11e9-aaf5-15a0b0c9f7a6', N'0d1ea450-9366-11e9-a36a-0bad1f89eaac', N'f157c0e0-9369-11e9-a658-3548ed1030d9', 1, CAST(10 AS Decimal(18, 0)), CAST(10 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'e70e5e10-9386-11e9-8479-6516af7fbd1e', N'd426e3d0-9386-11e9-8479-6516af7fbd1e', N'9b62fcd0-9360-11e9-bfc0-7dd2c67b559c', 3, CAST(120 AS Decimal(18, 0)), CAST(360 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'f43a4130-9386-11e9-8479-6516af7fbd1e', N'd426e3d0-9386-11e9-8479-6516af7fbd1e', N'f157c0e0-9369-11e9-a658-3548ed1030d9', 2, CAST(10 AS Decimal(18, 0)), CAST(20 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'fb999d40-9386-11e9-8479-6516af7fbd1e', N'd426e3d0-9386-11e9-8479-6516af7fbd1e', N'204d5300-9361-11e9-bfc0-7dd2c67b559c', 1, CAST(50 AS Decimal(18, 0)), CAST(50 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'eda11b50-9386-11e9-8479-6516af7fbd1e', N'd426e3d0-9386-11e9-8479-6516af7fbd1e', N'f92fe580-9360-11e9-bfc0-7dd2c67b559c', 1, CAST(15 AS Decimal(18, 0)), CAST(15 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'01ed55b0-9387-11e9-8479-6516af7fbd1e', N'd426e3d0-9386-11e9-8479-6516af7fbd1e', N'707c35d0-9361-11e9-bfc0-7dd2c67b559c', 1, CAST(75 AS Decimal(18, 0)), CAST(75 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'fff11e10-9366-11e9-b6e2-7f310a38a6b4', N'0d1ea450-9366-11e9-a36a-0bad1f89eaac', N'66971940-9361-11e9-bfc0-7dd2c67b559c', 1, CAST(10 AS Decimal(18, 0)), CAST(10 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'd8943690-9366-11e9-b6e2-7f310a38a6b4', N'0d1ea450-9366-11e9-a36a-0bad1f89eaac', N'42f02a50-9360-11e9-bfc0-7dd2c67b559c', 100, CAST(10 AS Decimal(18, 0)), CAST(1000 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'ea9a7890-9366-11e9-b6e2-7f310a38a6b4', N'0d1ea450-9366-11e9-a36a-0bad1f89eaac', N'f92fe580-9360-11e9-bfc0-7dd2c67b559c', 1, CAST(15 AS Decimal(18, 0)), CAST(15 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'fe18b490-9366-11e9-b6e2-7f310a38a6b4', N'0d1ea450-9366-11e9-a36a-0bad1f89eaac', N'30869d30-9361-11e9-bfc0-7dd2c67b559c', 1, CAST(200 AS Decimal(18, 0)), CAST(200 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'e8f8fd90-9366-11e9-b6e2-7f310a38a6b4', N'0d1ea450-9366-11e9-a36a-0bad1f89eaac', N'eca91c00-9360-11e9-bfc0-7dd2c67b559c', 1, CAST(10 AS Decimal(18, 0)), CAST(10 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'e30343a0-9366-11e9-b6e2-7f310a38a6b4', N'0d1ea450-9366-11e9-a36a-0bad1f89eaac', N'b47cc4d0-9360-11e9-bfc0-7dd2c67b559c', 1, CAST(150 AS Decimal(18, 0)), CAST(150 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'01dbfe20-9367-11e9-b6e2-7f310a38a6b4', N'0d1ea450-9366-11e9-a36a-0bad1f89eaac', N'5a3c8fe0-9361-11e9-bfc0-7dd2c67b559c', 1, CAST(250 AS Decimal(18, 0)), CAST(250 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'057a5a90-9367-11e9-b6e2-7f310a38a6b4', N'0d1ea450-9366-11e9-a36a-0bad1f89eaac', N'707c35d0-9361-11e9-bfc0-7dd2c67b559c', 1, CAST(75 AS Decimal(18, 0)), CAST(75 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'0ff09390-9367-11e9-b6e2-7f310a38a6b4', N'10be3940-9366-11e9-a36a-0bad1f89eaac', N'50e095f0-9360-11e9-bfc0-7dd2c67b559c', 3, CAST(150 AS Decimal(18, 0)), CAST(450 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'f7f9d450-9351-11e9-b3a1-89bc9d4b3c32', NULL, NULL, 4, CAST(345 AS Decimal(18, 0)), CAST(1380 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'df42ecd0-9351-11e9-b3a1-89bc9d4b3c32', NULL, NULL, 1, CAST(999 AS Decimal(18, 0)), CAST(999 AS Decimal(18, 0)), NULL)
INSERT [dbo].[OrderedItem] ([Id], [OrderId], [ItemId], [Quantity], [Price], [Subtotal], [Remarks]) VALUES (N'8c9ff360-936a-11e9-aa68-c3e1c1562441', N'07831850-9366-11e9-a36a-0bad1f89eaac', N'e858e5a0-9369-11e9-a658-3548ed1030d9', 1, CAST(10 AS Decimal(18, 0)), CAST(10 AS Decimal(18, 0)), NULL)
INSERT [dbo].[Role] ([Id], [Name]) VALUES (N'63703784-37e6-4076-9fcb-044c27e1b0aa', N'Administrator')
INSERT [dbo].[Role] ([Id], [Name]) VALUES (N'bfe4ce68-2741-41e1-a9ff-2c52d8c63114', N'Cashier')
INSERT [dbo].[Role] ([Id], [Name]) VALUES (N'b28aea35-57e0-42be-b935-09efbfaeb8ae', N'Kitchen Head')
INSERT [dbo].[Role] ([Id], [Name]) VALUES (N'95451173-3784-4b21-b02e-3d375a6cf25a', N'Manager')
INSERT [dbo].[Role] ([Id], [Name]) VALUES (N'82e960a0-8a45-11e9-9b2c-a9940556ba7c', N'Waiter')
INSERT [dbo].[Unit] ([Id], [Name], [Code]) VALUES (N'76b83d00-9360-11e9-bfc0-7dd2c67b559c', N'Inches', N'In')
INSERT [dbo].[Unit] ([Id], [Name], [Code]) VALUES (N'dceb6a70-9360-11e9-bfc0-7dd2c67b559c', N'Default', N' ')
INSERT [dbo].[Unit] ([Id], [Name], [Code]) VALUES (N'bb1bac80-935f-11e9-8e44-d9cdafda7319', N'Pieces', N'Pcs')
INSERT [dbo].[Unit] ([Id], [Name], [Code]) VALUES (N'3effe529-45c0-44c9-a758-f65b176aabb9', N'Grams', N'gms')
INSERT [dbo].[User] ([Id], [UserName], [RoleId], [FirstName], [LastName], [Password], [Enabled], [Locked], [CreatedOn], [MobileNumber], [EmailAddress]) VALUES (N'fad27060-92fc-11e9-b444-cbc6df9cac0b', N'rrk', N'82e960a0-8a45-11e9-9b2c-a9940556ba7c', N'Renu', N'Kapadia', N'rrk', 1, 1, CAST(N'2019-06-20T07:14:44.977' AS DateTime), N'8327658745', N'rrk@hotmail.com')
INSERT [dbo].[User] ([Id], [UserName], [RoleId], [FirstName], [LastName], [Password], [Enabled], [Locked], [CreatedOn], [MobileNumber], [EmailAddress]) VALUES (N'e54132e0-92fc-11e9-b444-cbc6df9cac0b', N'rlk', N'82e960a0-8a45-11e9-9b2c-a9940556ba7c', N'Rajen', N'Kapadia', N'rlk', 1, 1, CAST(N'2019-06-20T07:14:08.930' AS DateTime), N'8320301668', N'rlk@gmail.com')
INSERT [dbo].[User] ([Id], [UserName], [RoleId], [FirstName], [LastName], [Password], [Enabled], [Locked], [CreatedOn], [MobileNumber], [EmailAddress]) VALUES (N'0af91cf0-92fd-11e9-b444-cbc6df9cac0b', N'srk', N'82e960a0-8a45-11e9-9b2c-a9940556ba7c', N'Sagar', N'Kapadia', N'srk', 1, 1, CAST(N'2019-06-20T07:15:12.093' AS DateTime), N'9327350853', N'srk@gmaill.com')
INSERT [dbo].[WaiterTable] ([Id], [WaiterId], [TableId]) VALUES (N'0530d221-a3d4-4679-8b4c-0a3ca22ec459', N'0af91cf0-92fd-11e9-b444-cbc6df9cac0b', N'17bccd20-9365-11e9-a36a-0bad1f89eaac')
INSERT [dbo].[WaiterTable] ([Id], [WaiterId], [TableId]) VALUES (N'7e87778b-21fc-4bdc-9661-4864292df1f1', N'0af91cf0-92fd-11e9-b444-cbc6df9cac0b', N'10280930-9365-11e9-a36a-0bad1f89eaac')
INSERT [dbo].[WaiterTable] ([Id], [WaiterId], [TableId]) VALUES (N'93a1f057-57ed-40a7-b413-23663181ddc4', N'0af91cf0-92fd-11e9-b444-cbc6df9cac0b', N'0a3adac0-9365-11e9-a36a-0bad1f89eaac')
INSERT [dbo].[WaiterTable] ([Id], [WaiterId], [TableId]) VALUES (N'054d0911-28aa-4feb-8472-028ba7bc7ec2', N'e54132e0-92fc-11e9-b444-cbc6df9cac0b', N'848043d0-9386-11e9-8479-6516af7fbd1e')
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Category]    Script Date: 23-06-2019 05:05:37 ******/
ALTER TABLE [dbo].[Category] ADD  CONSTRAINT [IX_Category] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Category_Code]    Script Date: 23-06-2019 05:05:37 ******/
ALTER TABLE [dbo].[Category] ADD  CONSTRAINT [IX_Category_Code] UNIQUE NONCLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Category_Name]    Script Date: 23-06-2019 05:05:37 ******/
ALTER TABLE [dbo].[Category] ADD  CONSTRAINT [IX_Category_Name] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_DiningTable]    Script Date: 23-06-2019 05:05:37 ******/
ALTER TABLE [dbo].[DiningTable] ADD  CONSTRAINT [IX_DiningTable] UNIQUE NONCLUSTERED 
(
	[Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FoodOrder]    Script Date: 23-06-2019 05:05:37 ******/
ALTER TABLE [dbo].[FoodOrder] ADD  CONSTRAINT [IX_FoodOrder] UNIQUE NONCLUSTERED 
(
	[Date] ASC,
	[OrderNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Item_Category]    Script Date: 23-06-2019 05:05:37 ******/
ALTER TABLE [dbo].[Item] ADD  CONSTRAINT [IX_Item_Category] UNIQUE NONCLUSTERED 
(
	[CategoryId] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Item_Code]    Script Date: 23-06-2019 05:05:37 ******/
ALTER TABLE [dbo].[Item] ADD  CONSTRAINT [IX_Item_Code] UNIQUE NONCLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Item]    Script Date: 23-06-2019 05:05:37 ******/
CREATE NONCLUSTERED INDEX [IX_Item] ON [dbo].[Item]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Role]    Script Date: 23-06-2019 05:05:37 ******/
ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [IX_Role] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Unit_Code]    Script Date: 23-06-2019 05:05:37 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Unit_Code] ON [dbo].[Unit]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Unit_Name]    Script Date: 23-06-2019 05:05:37 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Unit_Name] ON [dbo].[Unit]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_User_UserName_Unique]    Script Date: 23-06-2019 05:05:37 ******/
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [IX_User_UserName_Unique] UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_WaiterTable]    Script Date: 23-06-2019 05:05:37 ******/
ALTER TABLE [dbo].[WaiterTable] ADD  CONSTRAINT [IX_WaiterTable] UNIQUE NONCLUSTERED 
(
	[TableId] ASC,
	[WaiterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FoodOrder]  WITH CHECK ADD  CONSTRAINT [FK_FoodOrder_DiningTable] FOREIGN KEY([TableId])
REFERENCES [dbo].[DiningTable] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[FoodOrder] CHECK CONSTRAINT [FK_FoodOrder_DiningTable]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_Category]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_Unit] FOREIGN KEY([UnitId])
REFERENCES [dbo].[Unit] ([Id])
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_Unit]
GO
ALTER TABLE [dbo].[OrderedItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderedItem_FoodOrder] FOREIGN KEY([OrderId])
REFERENCES [dbo].[FoodOrder] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderedItem] CHECK CONSTRAINT [FK_OrderedItem_FoodOrder]
GO
ALTER TABLE [dbo].[OrderedItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderedItem_Item] FOREIGN KEY([ItemId])
REFERENCES [dbo].[Item] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[OrderedItem] CHECK CONSTRAINT [FK_OrderedItem_Item]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
ALTER TABLE [dbo].[UserSession]  WITH CHECK ADD  CONSTRAINT [FK_UserSession_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserSession] CHECK CONSTRAINT [FK_UserSession_User]
GO
ALTER TABLE [dbo].[WaiterTable]  WITH CHECK ADD  CONSTRAINT [FK_WaiterTable_DiningTable] FOREIGN KEY([TableId])
REFERENCES [dbo].[DiningTable] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WaiterTable] CHECK CONSTRAINT [FK_WaiterTable_DiningTable]
GO
ALTER TABLE [dbo].[WaiterTable]  WITH CHECK ADD  CONSTRAINT [FK_WaiterTable_User] FOREIGN KEY([WaiterId])
REFERENCES [dbo].[User] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[WaiterTable] CHECK CONSTRAINT [FK_WaiterTable_User]
GO
USE [master]
GO
ALTER DATABASE [RestaurantDB] SET  READ_WRITE 
GO
