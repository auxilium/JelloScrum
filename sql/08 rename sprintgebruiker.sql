/*
   dinsdag 8 december 200910:32:26
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
EXECUTE sp_rename N'dbo.SprintGebruiker', N'SprintUser', 'OBJECT' 
GO
EXECUTE sp_rename N'dbo.SprintUser.SprintRol', N'Tmp_SprintRole_65', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.SprintUser.Gebruiker', N'Tmp_JelloScrumUser_66', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.SprintUser.Tmp_SprintRole_65', N'SprintRole', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.SprintUser.Tmp_JelloScrumUser_66', N'JelloScrumUser', 'COLUMN' 
GO
COMMIT
