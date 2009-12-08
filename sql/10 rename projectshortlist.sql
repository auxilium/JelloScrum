/*
   dinsdag 8 december 200910:57:17
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
EXECUTE sp_rename N'dbo.ProjectShortList.Gebruiker', N'Tmp_JelloScrumUser_75', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.ProjectShortList.Tmp_JelloScrumUser_75', N'JelloScrumUser', 'COLUMN' 
GO
COMMIT
