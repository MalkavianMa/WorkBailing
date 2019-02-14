USE [FAULTVISTA]
GO

/****** Object:  Table [dbo].[ThunderDatasHistory]    Script Date: 09/01/2017 14:17:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ThunderDatasHistory](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[time] [datetime] NULL,
	[lng] [float] NULL,
	[lat] [float] NULL,
	[TDcurrent] [float] NULL,
	[isBack] [int] NULL,
	[saveTime] [datetime] NULL,
 CONSTRAINT [PK_ThunderDatasHistory] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


