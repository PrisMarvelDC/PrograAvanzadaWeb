--DROP DATABASE ProyectoWebAvanzado 
--CREATE DATABASE [ProyectoWebAvanzado]
--GO
USE [ProyectoWebAvanzado]
---
CREATE TABLE AspNetRoles (
    Id NVARCHAR(450) PRIMARY KEY,
    Name NVARCHAR(256),
    NormalizedName NVARCHAR(256),
    ConcurrencyStamp NVARCHAR(MAX)
);
GO
---
CREATE TABLE AspNetUsers (
    Id NVARCHAR(450) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellidos NVARCHAR(100) NOT NULL,
    Cedula NVARCHAR(20) NOT NULL,
    Telefono NVARCHAR(15) NOT NULL,
    UserName NVARCHAR(256),
    NormalizedUserName NVARCHAR(256),
    Email NVARCHAR(256),
    NormalizedEmail NVARCHAR(256),
    EmailConfirmed BIT,
    PasswordHash NVARCHAR(MAX),
    SecurityStamp NVARCHAR(MAX),
    ConcurrencyStamp NVARCHAR(MAX),
    PhoneNumber NVARCHAR(MAX),
    PhoneNumberConfirmed BIT,
    TwoFactorEnabled BIT,
    LockoutEnd DATETIMEOFFSET,
    LockoutEnabled BIT,
    AccessFailedCount INT
);
GO
---
CREATE TABLE AspNetRoleClaims (
    Id INT PRIMARY KEY IDENTITY(1,1),
    RoleId NVARCHAR(450) NOT NULL,
    ClaimType NVARCHAR(MAX),
    ClaimValue NVARCHAR(MAX),
    FOREIGN KEY (RoleId) REFERENCES AspNetRoles(Id) ON DELETE CASCADE
);
GO
---
CREATE TABLE AspNetUserClaims (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId NVARCHAR(450) NOT NULL,
    ClaimType NVARCHAR(MAX),
    ClaimValue NVARCHAR(MAX),
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);
GO
---
CREATE TABLE AspNetUserLogins (
    LoginProvider NVARCHAR(128) NOT NULL,
    ProviderKey NVARCHAR(128) NOT NULL,
    ProviderDisplayName NVARCHAR(MAX),
    UserId NVARCHAR(450) NOT NULL,
    PRIMARY KEY (LoginProvider, ProviderKey),
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);
GO
---
CREATE TABLE AspNetUserRoles (
    UserId NVARCHAR(450) NOT NULL,
    RoleId NVARCHAR(450) NOT NULL,
    PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    FOREIGN KEY (RoleId) REFERENCES AspNetRoles(Id) ON DELETE CASCADE
);
GO
---
CREATE TABLE AspNetUserTokens (
    UserId NVARCHAR(450) NOT NULL,
    LoginProvider NVARCHAR(128) NOT NULL,
    Name NVARCHAR(128) NOT NULL,
    Value NVARCHAR(MAX),
    PRIMARY KEY (UserId, LoginProvider, Name),
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);
GO
---
CREATE TABLE [dbo].[TReceta](
    [Id] [bigint] IDENTITY(1, 1) NOT NULL,
	[UsuarioId] [nvarchar](450) NOT NULL,
    [Nombre] [varchar](100) NOT NULL,
    [Descripcion] [varchar](200) NOT NULL,
    [Instrucciones] [varchar](MAX) NOT NULL,
    [Categoria] [varchar](100) NOT NULL,
	[Ingredientes] [varchar](500) NOT NULL,
    CONSTRAINT [PK_TReceta] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (
        PAD_INDEX = OFF,
        STATISTICS_NORECOMPUTE = OFF,
        IGNORE_DUP_KEY = OFF,
        ALLOW_ROW_LOCKS = ON,
        ALLOW_PAGE_LOCKS = ON,
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
    ) ON [PRIMARY],
	CONSTRAINT [FK_TReceta_AspNetUsers] FOREIGN KEY([UsuarioId]) REFERENCES [dbo].[AspNetUsers] ([Id])
) ON [PRIMARY]
GO
---
	CREATE TABLE [dbo].[TCurso](
		[Id] [bigint] IDENTITY(1, 1) NOT NULL,
		[Nombre] [varchar](100) NOT NULL,
		[Descripcion] [varchar](200) NOT NULL,
		[Profesor] [varchar](100) NOT NULL,
		[UsuarioId] [nvarchar](450) NOT NULL,
		CONSTRAINT [PK_TCurso] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (
			PAD_INDEX = OFF,
			STATISTICS_NORECOMPUTE = OFF,
			IGNORE_DUP_KEY = OFF,
			ALLOW_ROW_LOCKS = ON,
			ALLOW_PAGE_LOCKS = ON,
			OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
		) ON [PRIMARY],
		CONSTRAINT [FK_TCurso_AspNetUsers] FOREIGN KEY([UsuarioId]) REFERENCES [dbo].[AspNetUsers] ([Id])
	) ON [PRIMARY]
GO
---
--	CREATE TABLE [dbo].[TUsuario](
--		[Id] [bigint] IDENTITY(1, 1) NOT NULL,
--		[Identification] [varchar](20) NOT NULL,
--		[Nombre] [varchar](100) NOT NULL,
--		[Apellidos] [varchar](100) NOT NULL,
--		[Correo] [varchar](50) NOT NULL,
--		[Telefono] [varchar](15) NOT NULL,
--		CONSTRAINT [PK_TUsuario] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (
--			PAD_INDEX = OFF,
--			STATISTICS_NORECOMPUTE = OFF,
--			IGNORE_DUP_KEY = OFF,
--			ALLOW_ROW_LOCKS = ON,
--			ALLOW_PAGE_LOCKS = ON,
--			OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
--		) ON [PRIMARY]
--	) ON [PRIMARY]
--GO
---
CREATE TABLE [dbo].[TCursoUsuario](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CursoId] [bigint] NOT NULL,
    [UsuarioId] [nvarchar](450) NOT NULL,
    CONSTRAINT [PK_TCursoUsuario] PRIMARY KEY CLUSTERED ([CursoId] ASC, [UsuarioId] ASC) WITH (
        PAD_INDEX = OFF,
        STATISTICS_NORECOMPUTE = OFF,
        IGNORE_DUP_KEY = OFF,
        ALLOW_ROW_LOCKS = ON,
        ALLOW_PAGE_LOCKS = ON,
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
    ) ON [PRIMARY],
    CONSTRAINT [FK_TCursoUsuario_TCurso] FOREIGN KEY([CursoId]) REFERENCES [dbo].[TCurso] ([Id]),
    CONSTRAINT [FK_TCursoUsuario_AspNetUsers] FOREIGN KEY([UsuarioId]) REFERENCES [dbo].[AspNetUsers] ([Id])
) ON [PRIMARY]
GO
---
CREATE TABLE [dbo].[TCursoReceta](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CursoId] [bigint] NOT NULL,
    [RecetaId] [bigint] NOT NULL,
    CONSTRAINT [PK_TCursoReceta] PRIMARY KEY CLUSTERED ([CursoId] ASC, [RecetaId] ASC),
    CONSTRAINT [FK_TCursoReceta_TCurso] FOREIGN KEY([CursoId]) REFERENCES [dbo].[TCurso] ([Id]),
    CONSTRAINT [FK_TCursoReceta_TReceta] FOREIGN KEY([RecetaId]) REFERENCES [dbo].[TReceta] ([Id])
) ON [PRIMARY]
GO
---
CREATE TABLE [dbo].[TLogErrores](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
    [FechaHora] [datetime] NOT NULL DEFAULT(GETDATE()),
    [Usuario] [varchar](100),
    [Modulo] [varchar](100),
    [DescripcionError] [varchar](MAX) NOT NULL,
    [InformacionAdicional] [varchar](MAX),
    CONSTRAINT [PK_TLogErrores] PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]
GO
---

DROP TABLE [dbo].[TCursoUsuario]
DROP TABLE [dbo].[TCursoReceta]

--
--Quitar constraint
alter table TCursoReceta drop constraint FK_TCursoReceta_TReceta
drop table TReceta
--
alter table TCursoReceta drop constraint FK_TCursoReceta_TCurso
drop table TCurso
--
--
--
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) 
VALUES ('1', 'Administrador', 'ADMINISTRADOR', NULL);
 
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) 
VALUES ('2', 'Profesor', 'PROFESOR', NULL);
 
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) 
VALUES ('3', 'Estudiante', 'ESTUDIANTE', NULL);