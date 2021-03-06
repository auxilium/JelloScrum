/*
   maandag 7 december 200917:07:55
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
EXECUTE sp_rename N'dbo.Story.Titel', N'Tmp_Title_37', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Story.Omschrijving', N'Tmp_Description_38', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Story.Notitie', N'Tmp_Note_39', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Story.Schatting', N'Tmp_Estimation_40', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Story.ProductBacklogPrioriteit', N'Tmp_ProductBacklogPriority_41', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Story.AangemaaktDoor', N'Tmp_CreatedBy_42', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Story.Tmp_Title_37', N'Title', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Story.Tmp_Description_38', N'Description', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Story.Tmp_Note_39', N'Note', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Story.Tmp_Estimation_40', N'Estimation', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Story.Tmp_ProductBacklogPriority_41', N'ProductBacklogPriority', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Story.Tmp_CreatedBy_42', N'CreatedBy', 'COLUMN' 
GO
COMMIT
