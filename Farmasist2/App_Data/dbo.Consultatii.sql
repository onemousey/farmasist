CREATE TABLE [dbo].[Consultatii] (
    [NrCrt]        INT           NOT NULL,
    [NrProgramare] INT           NOT NULL,
    [IDMedic]      INT           NOT NULL,
    [CNP]          VARCHAR (13)  NOT NULL,
    [Diagnostic]   VARCHAR (MAX) NULL,
    [IDTratament]  INT           NULL,
    PRIMARY KEY CLUSTERED ([NrCrt] ASC),
    CONSTRAINT [FK_Consultatii_Medici] FOREIGN KEY ([IDMedic]) REFERENCES [dbo].[Medici] ([IDMedic]),
    CONSTRAINT [FK_Consultatii_Pacienti] FOREIGN KEY ([CNP]) REFERENCES [dbo].[Pacienti] ([CNP]),
    CONSTRAINT [FK_Consultatii_Programari] FOREIGN KEY ([NrProgramare]) REFERENCES [dbo].[Programari] ([NrProgramare])
);

