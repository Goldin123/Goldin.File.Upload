USE [master]
GO
/****** Object:  Database [FileManager]    Script Date: 2024/10/08 12:15:57 ******/
CREATE DATABASE [FileManager]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'FileManager', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER03\MSSQL\DATA\FileManager.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'FileManager_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER03\MSSQL\DATA\FileManager_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [FileManager] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FileManager].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FileManager] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FileManager] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FileManager] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FileManager] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FileManager] SET ARITHABORT OFF 
GO
ALTER DATABASE [FileManager] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [FileManager] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FileManager] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FileManager] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FileManager] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FileManager] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FileManager] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FileManager] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FileManager] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FileManager] SET  DISABLE_BROKER 
GO
ALTER DATABASE [FileManager] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FileManager] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FileManager] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FileManager] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FileManager] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FileManager] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FileManager] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FileManager] SET RECOVERY FULL 
GO
ALTER DATABASE [FileManager] SET  MULTI_USER 
GO
ALTER DATABASE [FileManager] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FileManager] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FileManager] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FileManager] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [FileManager] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [FileManager] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'FileManager', N'ON'
GO
ALTER DATABASE [FileManager] SET QUERY_STORE = ON
GO
ALTER DATABASE [FileManager] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [FileManager]
GO
/****** Object:  UserDefinedTableType [dbo].[DataFileType]    Script Date: 2024/10/08 12:15:57 ******/
CREATE TYPE [dbo].[DataFileType] AS TABLE(
	[Name] [varchar](100) NULL,
	[Type] [varchar](100) NULL,
	[Search] [bit] NULL,
	[LibraryFilter] [bit] NULL,
	[Visible] [bit] NULL
)
GO
/****** Object:  Table [dbo].[DataFile]    Script Date: 2024/10/08 12:15:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataFile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [varchar](255) NULL,
	[Name] [varchar](100) NULL,
	[Type] [varchar](50) NULL,
	[Search] [bit] NULL,
	[LibraryFilter] [bit] NULL,
	[Visible] [bit] NULL,
	[UploadedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DataFile] ADD  DEFAULT (getdate()) FOR [UploadedAt]
GO
/****** Object:  StoredProcedure [dbo].[sp_AddDataFileRecord]    Script Date: 2024/10/08 12:15:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_AddDataFileRecord]
    @FileName NVARCHAR(100),
    @Name NVARCHAR(100),
    @Type NVARCHAR(50),
    @Search BIT,
    @LibraryFilter BIT,
    @Visible BIT
AS
BEGIN
    SET NOCOUNT ON;

    -- Start a transaction
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Insert data into DataFile table
        INSERT INTO DataFile (FileName, Name, Type, Search, LibraryFilter, Visible)
        VALUES (@FileName, @Name, @Type, @Search, @LibraryFilter, @Visible);

        -- Commit the transaction if successful
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Roll back the transaction if an error occurs
        ROLLBACK TRANSACTION;

        -- Error handling logic
        DECLARE @ErrorMessage NVARCHAR(4000), 
                @ErrorSeverity INT, 
                @ErrorState INT;

        -- Get error information
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        -- Log the error or return it as a message (depending on your logging strategy)
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        
        -- Optionally, you could insert the error details into an error logging table here if needed.
        -- Example: 
        -- INSERT INTO ErrorLog (ErrorMessage, ErrorSeverity, ErrorState, ErrorTime) 
        -- VALUES (@ErrorMessage, @ErrorSeverity, @ErrorState, GETDATE());

        RETURN; -- Exit the procedure in case of error
    END CATCH;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllDataFiles]    Script Date: 2024/10/08 12:15:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  create   PROCEDURE [dbo].[sp_GetAllDataFiles]
    AS
BEGIN
    BEGIN TRY
		SELECT [Id]
		    ,[FileName]
		    ,[Name]
		    ,[Type]
		    ,[Search]
		    ,[LibraryFilter]
		    ,[Visible]
		    ,[UploadedAt]
		FROM [FileManager].[dbo].[DataFile] WITH (NOLOCK)
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_GetDataFileByFilename]    Script Date: 2024/10/08 12:15:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetDataFileByFilename]
    @FileName NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        SELECT 
            id,
            fileName,
            name,
            type,
            search,
            libraryFilter,
            visible
        FROM 
            DataFiles
        WHERE 
            fileName = @FileName;
    END TRY
    BEGIN CATCH
        -- Handle the error
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        -- Optionally log the error, or re-throw it
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[sp_SaveDataFile]    Script Date: 2024/10/08 12:15:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_SaveDataFile]
    @FileName NVARCHAR(255),
    @DataFileRows dbo.DataFileType READONLY
AS
BEGIN
    SET NOCOUNT ON;

    -- Start a transaction
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Insert into the DataFile table
        INSERT INTO DataFile (FileName, Name, Type, Search, LibraryFilter, Visible)
        SELECT @FileName, Name, Type, Search, LibraryFilter, Visible
        FROM @DataFileRows;

        -- Commit the transaction if successful
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Roll back the transaction if an error occurs
        ROLLBACK TRANSACTION;

        -- Capture and throw the error details
        DECLARE @ErrorMessage NVARCHAR(4000),
                @ErrorSeverity INT,
                @ErrorState INT;

        -- Retrieve the error information
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        -- Re-throw the error with custom handling
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);

        -- Optionally, log the error to a table or another mechanism for tracking
        -- INSERT INTO ErrorLog (ErrorMessage, ErrorSeverity, ErrorState, ErrorDate)
        -- VALUES (@ErrorMessage, @ErrorSeverity, @ErrorState, GETDATE());
    END CATCH
END;

GO
USE [master]
GO
ALTER DATABASE [FileManager] SET  READ_WRITE 
GO
