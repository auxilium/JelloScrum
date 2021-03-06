/*
   maandag 7 december 200917:33:08
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
EXECUTE sp_rename N'dbo.Task.Titel', N'Tmp_Title_49', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Task.Omschrijving', N'Tmp_Description_50', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Task.Status', N'Tmp_State_51', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Task.Schatting', N'Tmp_Estimation_52', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Task.Behandelaar', N'Tmp_AssignedUser_53', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Task.DatumAfgesloten', N'Tmp_DateClosed_54', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Task.Tmp_Title_49', N'Title', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Task.Tmp_Description_50', N'Description', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Task.Tmp_State_51', N'State', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Task.Tmp_Estimation_52', N'Estimation', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Task.Tmp_AssignedUser_53', N'AssignedUser', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Task.Tmp_DateClosed_54', N'DateClosed', 'COLUMN' 
GO
COMMIT
