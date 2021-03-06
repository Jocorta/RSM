USE [master]
GO

/****** Object:  Database [RSM]    Script Date: 11/27/2017 11:08:04 PM ******/
CREATE DATABASE [RSM]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RSM', FILENAME = N'C:\Users\jorge\RSM.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RSM_log', FILENAME = N'C:\Users\jorge\RSM_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO

ALTER DATABASE [RSM] SET COMPATIBILITY_LEVEL = 130
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RSM].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [RSM] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [RSM] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [RSM] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [RSM] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [RSM] SET ARITHABORT OFF 
GO

ALTER DATABASE [RSM] SET AUTO_CLOSE ON 
GO

ALTER DATABASE [RSM] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [RSM] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [RSM] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [RSM] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [RSM] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [RSM] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [RSM] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [RSM] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [RSM] SET  ENABLE_BROKER 
GO

ALTER DATABASE [RSM] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [RSM] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [RSM] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [RSM] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [RSM] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [RSM] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [RSM] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [RSM] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [RSM] SET  MULTI_USER 
GO

ALTER DATABASE [RSM] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [RSM] SET DB_CHAINING OFF 
GO

ALTER DATABASE [RSM] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [RSM] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [RSM] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [RSM] SET QUERY_STORE = OFF
GO

USE [RSM]
GO

ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO

ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO

ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO

ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO

ALTER DATABASE [RSM] SET  READ_WRITE 
GO

