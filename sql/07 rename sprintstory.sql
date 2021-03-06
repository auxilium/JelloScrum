/*
   maandag 7 december 200917:37:32
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
EXECUTE sp_rename N'dbo.SprintStory.SprintBacklogPrioriteit', N'Tmp_SprintBacklogPriority_61', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.SprintStory.Schatting', N'Tmp_Estimation_62', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.SprintStory.Tmp_SprintBacklogPriority_61', N'SprintBacklogPriority', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.SprintStory.Tmp_Estimation_62', N'Estimation', 'COLUMN' 
GO
COMMIT
