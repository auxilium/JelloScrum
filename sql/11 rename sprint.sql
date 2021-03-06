/*
   dinsdag 8 december 200911:10:49
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
EXECUTE sp_rename N'dbo.Sprint.Doel', N'Tmp_Goal_77', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Sprint.Omschrijving', N'Tmp_Description_78', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Sprint.StartDatum', N'Tmp_StartDate_79', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Sprint.EindDatum', N'Tmp_EndDate_80', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Sprint.BeschikbareUren', N'Tmp_AvailableTime_81', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Sprint.WerkDagen', N'Tmp_WorkDays_82', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Sprint.IsAfgesloten', N'Tmp_IsClosed_83', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Sprint.Tmp_Goal_77', N'Goal', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Sprint.Tmp_Description_78', N'Description', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Sprint.Tmp_StartDate_79', N'StartDate', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Sprint.Tmp_EndDate_80', N'EndDate', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Sprint.Tmp_AvailableTime_81', N'AvailableTime', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Sprint.Tmp_WorkDays_82', N'WorkDays', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Sprint.Tmp_IsClosed_83', N'IsClosed', 'COLUMN' 
GO
COMMIT
