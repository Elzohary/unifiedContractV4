-- Create ClientMaterials table
CREATE TABLE [Resource].[ClientMaterials] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [GroupCode] NVARCHAR(50) NOT NULL,
    [SEQ] NVARCHAR(50) NOT NULL,
    [MaterialMasterCode] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(500) NOT NULL,
    [Unit] NVARCHAR(50) NOT NULL,
    [ClientId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL,
    [CreatedById] UNIQUEIDENTIFIER NULL,
    [LastModifiedAt] DATETIME2 NULL,
    [LastModifiedById] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_ClientMaterials] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClientMaterials_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Client].[Clients] ([Id]) ON DELETE NO ACTION
);

-- Add ClientMaterialId column to ReceivableMaterials table
ALTER TABLE [Resource].[ReceivableMaterials]
ADD [ClientMaterialId] UNIQUEIDENTIFIER NULL;

-- Add foreign key from ReceivableMaterials to ClientMaterials
ALTER TABLE [Resource].[ReceivableMaterials]
ADD CONSTRAINT [FK_ReceivableMaterials_ClientMaterials_ClientMaterialId] 
FOREIGN KEY ([ClientMaterialId]) REFERENCES [Resource].[ClientMaterials] ([Id]) ON DELETE NO ACTION;

-- Create indexes for ClientMaterials table
CREATE INDEX [IX_ClientMaterials_ClientId] ON [Resource].[ClientMaterials] ([ClientId]);
CREATE INDEX [IX_ClientMaterials_MaterialMasterCode] ON [Resource].[ClientMaterials] ([MaterialMasterCode]);
CREATE UNIQUE INDEX [IX_ClientMaterials_ClientId_MaterialMasterCode] ON [Resource].[ClientMaterials] ([ClientId], [MaterialMasterCode]);

-- Create index for ReceivableMaterials table to optimize lookups
CREATE INDEX [IX_ReceivableMaterials_ClientMaterialId] ON [Resource].[ReceivableMaterials] ([ClientMaterialId]); 