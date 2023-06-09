USE [travel_rewards]
GO
/****** Object:  Table [dbo].[account]    Script Date: 1/3/2023 2:53:26 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[account](
	[acc_id] [uniqueidentifier] NOT NULL,
	[acc_username] [varchar](255) NOT NULL,
	[acc_password] [varchar](255) NOT NULL,
	[acc_disable] [bit] NOT NULL,
	[acc_currentSession] [varchar](2047) NULL,
	[acc_role] [int] NOT NULL,
	[deleted_at] [datetime] NULL,
	[deleted_by] [varchar](255) NULL,
	[created_at] [datetime] NULL,
	[created_by] [varchar](255) NULL,
	[modified_at] [datetime] NULL,
	[modified_by] [varchar](255) NULL,
 CONSTRAINT [PK_account] PRIMARY KEY CLUSTERED 
(
	[acc_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[account_details]    Script Date: 1/3/2023 2:53:26 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[account_details](
	[accDetails_id] [uniqueidentifier] NOT NULL,
	[accDetails_firstName] [varchar](255) NULL,
	[accDetails_lastName] [varchar](255) NULL,
	[accDetails_gender] [int] NULL,
	[accDetails_profilePicture] [varbinary](max) NULL,
	[accDetails_totalPoints] [int] NULL,
	[accDetails_phoneCountryCode] [varchar](255) NULL,
	[accDetails_phoneNumber] [varchar](255) NULL,
	[accDetails_addressCountry] [varchar](255) NULL,
	[accDetails_addressCity] [varchar](255) NULL,
	[accDetails_addressZipCode] [varchar](255) NULL,
	[accDetails_addressStreet] [varchar](255) NULL,
	[deleted_at] [datetime] NULL,
	[deleted_by] [varchar](255) NULL,
	[created_at] [datetime] NULL,
	[created_by] [varchar](255) NULL,
	[modified_at] [datetime] NULL,
	[modified_by] [varchar](255) NULL,
	[acc_id] [uniqueidentifier] NULL,
	[accDetails_age] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[accDetails_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[account_employment]    Script Date: 1/3/2023 2:53:26 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[account_employment](
	[accEmp_id] [uniqueidentifier] NOT NULL,
	[accEmp_status] [varchar](255) NULL,
	[accEmp_occupation] [varchar](255) NULL,
	[accEmp_location] [varchar](255) NULL,
	[accEmp_startTime] [int] NULL,
	[accEmp_endTime] [int] NULL,
	[accEmp_annualSalary] [int] NULL,
	[deleted_at] [datetime] NULL,
	[deleted_by] [varchar](255) NULL,
	[created_at] [datetime] NULL,
	[created_by] [varchar](255) NULL,
	[modified_at] [datetime] NULL,
	[modified_by] [varchar](255) NULL,
	[acc_id] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[accEmp_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[account_extra]    Script Date: 1/3/2023 2:53:26 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[account_extra](
	[accExtra_id] [uniqueidentifier] NOT NULL,
	[accExtra_driverLicense] [varchar](255) NULL,
	[accExtra_mobilityImpaired] [bit] NOT NULL,
	[accExtra_householdPosition] [varchar](255) NULL,
	[accExtra_numberOfVehicles] [int] NULL,
	[deleted_at] [datetime] NULL,
	[deleted_by] [varchar](255) NULL,
	[created_at] [datetime] NULL,
	[created_by] [varchar](255) NULL,
	[modified_at] [datetime] NULL,
	[modified_by] [varchar](255) NULL,
	[acc_id] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[accExtra_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[account_points_txn]    Script Date: 1/3/2023 2:53:26 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[account_points_txn](
	[accPointsTxn_id] [uniqueidentifier] NOT NULL,
	[accPointsTxn_amt] [int] NOT NULL,
	[deleted_at] [datetime] NULL,
	[deleted_by] [varchar](255) NULL,
	[created_at] [datetime] NULL,
	[created_by] [varchar](255) NULL,
	[modified_at] [datetime] NULL,
	[modified_by] [varchar](255) NULL,
	[acc_id] [uniqueidentifier] NULL,
	[reward_id] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[accPointsTxn_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[account_triggered_survey]    Script Date: 1/3/2023 2:53:26 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[account_triggered_survey](
	[accTrigger_id] [uniqueidentifier] NOT NULL,
	[gps_logIds] [varchar](max) NULL,
	[status] [int] NULL,
	[survey_id] [uniqueidentifier] NULL,
	[acc_id] [uniqueidentifier] NULL,
	[trigger_id] [uniqueidentifier] NULL,
	[deleted_at] [datetime] NULL,
	[deleted_by] [varchar](255) NULL,
	[created_at] [datetime] NULL,
	[created_by] [varchar](255) NULL,
	[modified_at] [datetime] NULL,
	[modified_by] [varchar](255) NULL,
	[expire_by] [datetime] NULL,
	[gps_id] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[accTrigger_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[gps_logs]    Script Date: 1/3/2023 2:53:26 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[gps_logs](
	[gps_id] [uniqueidentifier] NOT NULL,
	[latitude] [decimal](10, 7) NOT NULL,
	[longitude] [decimal](10, 7) NOT NULL,
	[accuracy] [decimal](20, 15) NULL,
	[altitude] [decimal](21, 15) NOT NULL,
	[altitudeAccuracy] [decimal](20, 15) NULL,
	[heading] [decimal](18, 15) NULL,
	[speed] [decimal](19, 16) NULL,
	[gpsTimestamp] [datetime] NOT NULL,
	[deleted_at] [datetime] NULL,
	[deleted_by] [varchar](255) NULL,
	[created_at] [datetime] NULL,
	[created_by] [varchar](255) NULL,
	[modified_at] [datetime] NULL,
	[modified_by] [varchar](255) NULL,
	[acc_id] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[gps_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[response]    Script Date: 1/3/2023 2:53:26 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[response](
	[res_id] [uniqueidentifier] NOT NULL,
	[res_noOfQns] [int] NOT NULL,
	[response_disable] [bit] NOT NULL,
	[deleted_at] [datetime] NULL,
	[deleted_by] [varchar](255) NULL,
	[created_at] [datetime] NULL,
	[created_by] [varchar](255) NULL,
	[modified_at] [datetime] NULL,
	[modified_by] [varchar](255) NULL,
	[survey_id] [uniqueidentifier] NULL,
	[acc_id] [uniqueidentifier] NULL,
	[by_publicTransport] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[res_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[response_question]    Script Date: 1/3/2023 2:53:26 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[response_question](
	[resQns_id] [uniqueidentifier] NOT NULL,
	[resQns_type] [int] NOT NULL,
	[resQns_string] [varchar](255) NULL,
	[resQns_int] [int] NULL,
	[resQns_decimal] [decimal](20, 5) NULL,
	[deleted_at] [datetime] NULL,
	[deleted_by] [varchar](255) NULL,
	[created_at] [datetime] NULL,
	[created_by] [varchar](255) NULL,
	[modified_at] [datetime] NULL,
	[modified_by] [varchar](255) NULL,
	[res_id] [uniqueidentifier] NULL,
	[qns_id] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[resQns_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rewards]    Script Date: 1/3/2023 2:53:26 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rewards](
	[reward_id] [uniqueidentifier] NOT NULL,
	[reward_name] [varchar](255) NULL,
	[reward_desc] [varchar](255) NULL,
	[reward_qty] [int] NULL,
	[reward_img] [varbinary](max) NULL,
	[reward_points] [int] NULL,
	[deleted_at] [datetime] NULL,
	[deleted_by] [varchar](255) NULL,
	[created_at] [datetime] NULL,
	[created_by] [varchar](255) NULL,
	[modified_at] [datetime] NULL,
	[modified_by] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[reward_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[survey]    Script Date: 1/3/2023 2:53:26 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[survey](
	[survey_id] [uniqueidentifier] NOT NULL,
	[survey_noOfQns] [int] NOT NULL,
	[survey_title] [varchar](255) NOT NULL,
	[survey_disable] [bit] NOT NULL,
	[survey_points] [int] NOT NULL,
	[survey_desc] [varchar](255) NOT NULL,
	[deleted_at] [datetime] NULL,
	[deleted_by] [varchar](255) NULL,
	[created_at] [datetime] NULL,
	[created_by] [varchar](255) NULL,
	[modified_at] [datetime] NULL,
	[modified_by] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[survey_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[survey_question]    Script Date: 1/3/2023 2:53:26 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[survey_question](
	[qns_id] [uniqueidentifier] NOT NULL,
	[qns] [varchar](255) NOT NULL,
	[qns_type] [int] NOT NULL,
	[qns_order] [int] NOT NULL,
	[qns_option01] [varchar](255) NULL,
	[qns_option02] [varchar](255) NULL,
	[qns_option03] [varchar](255) NULL,
	[qns_option04] [varchar](255) NULL,
	[qns_option05] [varchar](255) NULL,
	[qns_option06] [varchar](255) NULL,
	[qns_option07] [varchar](255) NULL,
	[qns_option08] [varchar](255) NULL,
	[deleted_at] [datetime] NULL,
	[deleted_by] [varchar](255) NULL,
	[created_at] [datetime] NULL,
	[created_by] [varchar](255) NULL,
	[modified_at] [datetime] NULL,
	[modified_by] [varchar](255) NULL,
	[survey_id] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[qns_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[survey_trigger]    Script Date: 1/3/2023 2:53:26 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[survey_trigger](
	[trigger_id] [uniqueidentifier] NOT NULL,
	[lat_long] [varchar](max) NULL,
	[trigger_radius] [int] NULL,
	[trigger_cooldown] [int] NULL,
	[deleted_at] [datetime] NULL,
	[deleted_by] [varchar](255) NULL,
	[created_at] [datetime] NULL,
	[created_by] [varchar](255) NULL,
	[modified_at] [datetime] NULL,
	[modified_by] [varchar](255) NULL,
	[survey_id] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[trigger_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[survey_trigger_log]    Script Date: 1/3/2023 2:53:26 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[survey_trigger_log](
	[survey_trigger_log_id] [uniqueidentifier] NOT NULL,
	[log_date_time] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[survey_trigger_log_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[account] ADD  DEFAULT (newid()) FOR [acc_id]
GO
ALTER TABLE [dbo].[account_details] ADD  DEFAULT (newid()) FOR [accDetails_id]
GO
ALTER TABLE [dbo].[account_employment] ADD  DEFAULT (newid()) FOR [accEmp_id]
GO
ALTER TABLE [dbo].[account_extra] ADD  DEFAULT (newid()) FOR [accExtra_id]
GO
ALTER TABLE [dbo].[account_points_txn] ADD  DEFAULT (newid()) FOR [accPointsTxn_id]
GO
ALTER TABLE [dbo].[account_triggered_survey] ADD  DEFAULT (newid()) FOR [accTrigger_id]
GO
ALTER TABLE [dbo].[gps_logs] ADD  DEFAULT (newid()) FOR [gps_id]
GO
ALTER TABLE [dbo].[response] ADD  DEFAULT (newid()) FOR [res_id]
GO
ALTER TABLE [dbo].[response_question] ADD  DEFAULT (newid()) FOR [resQns_id]
GO
ALTER TABLE [dbo].[rewards] ADD  DEFAULT (newid()) FOR [reward_id]
GO
ALTER TABLE [dbo].[survey] ADD  DEFAULT (newid()) FOR [survey_id]
GO
ALTER TABLE [dbo].[survey_question] ADD  DEFAULT (newid()) FOR [qns_id]
GO
ALTER TABLE [dbo].[survey_trigger] ADD  DEFAULT (newid()) FOR [trigger_id]
GO
ALTER TABLE [dbo].[survey_trigger_log] ADD  DEFAULT (newid()) FOR [survey_trigger_log_id]
GO
ALTER TABLE [dbo].[account_details]  WITH CHECK ADD FOREIGN KEY([acc_id])
REFERENCES [dbo].[account] ([acc_id])
GO
ALTER TABLE [dbo].[account_employment]  WITH CHECK ADD FOREIGN KEY([acc_id])
REFERENCES [dbo].[account] ([acc_id])
GO
ALTER TABLE [dbo].[account_extra]  WITH CHECK ADD FOREIGN KEY([acc_id])
REFERENCES [dbo].[account] ([acc_id])
GO
ALTER TABLE [dbo].[account_points_txn]  WITH CHECK ADD FOREIGN KEY([acc_id])
REFERENCES [dbo].[account] ([acc_id])
GO
ALTER TABLE [dbo].[account_points_txn]  WITH CHECK ADD FOREIGN KEY([reward_id])
REFERENCES [dbo].[rewards] ([reward_id])
GO
ALTER TABLE [dbo].[account_triggered_survey]  WITH CHECK ADD FOREIGN KEY([acc_id])
REFERENCES [dbo].[account] ([acc_id])
GO
ALTER TABLE [dbo].[account_triggered_survey]  WITH CHECK ADD FOREIGN KEY([gps_id])
REFERENCES [dbo].[gps_logs] ([gps_id])
GO
ALTER TABLE [dbo].[account_triggered_survey]  WITH CHECK ADD FOREIGN KEY([survey_id])
REFERENCES [dbo].[survey] ([survey_id])
GO
ALTER TABLE [dbo].[account_triggered_survey]  WITH CHECK ADD FOREIGN KEY([trigger_id])
REFERENCES [dbo].[survey_trigger] ([trigger_id])
GO
ALTER TABLE [dbo].[gps_logs]  WITH CHECK ADD FOREIGN KEY([acc_id])
REFERENCES [dbo].[account] ([acc_id])
GO
ALTER TABLE [dbo].[response]  WITH CHECK ADD FOREIGN KEY([acc_id])
REFERENCES [dbo].[account] ([acc_id])
GO
ALTER TABLE [dbo].[response]  WITH CHECK ADD FOREIGN KEY([survey_id])
REFERENCES [dbo].[survey] ([survey_id])
GO
ALTER TABLE [dbo].[response_question]  WITH CHECK ADD FOREIGN KEY([qns_id])
REFERENCES [dbo].[survey_question] ([qns_id])
GO
ALTER TABLE [dbo].[response_question]  WITH CHECK ADD FOREIGN KEY([res_id])
REFERENCES [dbo].[response] ([res_id])
GO
ALTER TABLE [dbo].[survey_question]  WITH CHECK ADD FOREIGN KEY([survey_id])
REFERENCES [dbo].[survey] ([survey_id])
GO
ALTER TABLE [dbo].[survey_trigger]  WITH CHECK ADD FOREIGN KEY([survey_id])
REFERENCES [dbo].[survey] ([survey_id])
GO

/****** Inserting admin account into account table ******/
INSERT INTO account (acc_id,acc_username,acc_password,acc_disable,acc_role)
VALUES (NEWID(),'admin','$2a$11$XOBclTqITmN7ox8zwu2Abe4TQCCy1sFcjjkG/3tuXiFIwCoNfIvjG',0,2);
GO
