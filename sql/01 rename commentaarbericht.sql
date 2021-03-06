/*
   maandag 7 december 200911:05:37
   User: 
   Server: ILJA\SQLEXPRESS
   Database: JelloScrum
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
EXECUTE sp_rename N'dbo.CommentaarBericht.Tekst', N'Tmp_Text', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.CommentaarBericht.Datum', N'Tmp_Date_1', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.CommentaarBericht.Gebruiker', N'Tmp_User_2', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.CommentaarBericht.Tmp_Text', N'Text', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.CommentaarBericht.Tmp_Date_1', N'Date', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.CommentaarBericht.Tmp_User_2', N'JelloScrumUser', 'COLUMN' 
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
EXECUTE sp_rename N'dbo.CommentaarBericht', N'Comment', 'OBJECT' 
GO
COMMIT
