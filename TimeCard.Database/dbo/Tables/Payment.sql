CREATE TABLE [dbo].[Payment] (
    [payId]        INT             IDENTITY (1, 1) NOT NULL,
    [contractorId] INT             NOT NULL,
    [jobId]        INT             NOT NULL,
    [amount]       NUMERIC (18, 2) NOT NULL,
    [payDate]      DATETIME        NOT NULL,
    [checkNo]      INT             NOT NULL,
    CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED ([payId] ASC)
);

