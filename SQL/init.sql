use repositorydb2

create table AspNetRoles
(
    Id nvarchar(450) not null
        constraint PK_AspNetRoles
            primary key,
    Name nvarchar(256),
    NormalizedName nvarchar(256),
    ConcurrencyStamp nvarchar(max)
)
go

create table AspNetRoleClaims
(
    Id int identity
        constraint PK_AspNetRoleClaims
            primary key,
    RoleId nvarchar(450) not null
        constraint FK_AspNetRoleClaims_AspNetRoles_RoleId
            references AspNetRoles
            on delete cascade,
    ClaimType nvarchar(max),
    ClaimValue nvarchar(max)
)
go

create index IX_AspNetRoleClaims_RoleId
    on AspNetRoleClaims (RoleId)
go

create unique index RoleNameIndex
    on AspNetRoles (NormalizedName)
    where [NormalizedName] IS NOT NULL
go

create table AspNetUsers
(
    Id nvarchar(450) not null
        constraint PK_AspNetUsers
            primary key,
    Year int not null,
    UserName nvarchar(256),
    NormalizedUserName nvarchar(256),
    Email nvarchar(256),
    NormalizedEmail nvarchar(256),
    EmailConfirmed bit not null,
    PasswordHash nvarchar(max),
    SecurityStamp nvarchar(max),
    ConcurrencyStamp nvarchar(max),
    PhoneNumber nvarchar(max),
    PhoneNumberConfirmed bit not null,
    TwoFactorEnabled bit not null,
    LockoutEnd datetimeoffset,
    LockoutEnabled bit not null,
    AccessFailedCount int not null,
    FirstName nvarchar(max) not null,
    Patronymic nvarchar(max),
    Surname nvarchar(max) not null
)
go

create table AspNetUserClaims
(
    Id int identity
        constraint PK_AspNetUserClaims
            primary key,
    UserId nvarchar(450) not null
        constraint FK_AspNetUserClaims_AspNetUsers_UserId
            references AspNetUsers
            on delete cascade,
    ClaimType nvarchar(max),
    ClaimValue nvarchar(max)
)
go

create index IX_AspNetUserClaims_UserId
    on AspNetUserClaims (UserId)
go

create table AspNetUserLogins
(
    LoginProvider nvarchar(450) not null,
    ProviderKey nvarchar(450) not null,
    ProviderDisplayName nvarchar(max),
    UserId nvarchar(450) not null
        constraint FK_AspNetUserLogins_AspNetUsers_UserId
            references AspNetUsers
            on delete cascade,
    constraint PK_AspNetUserLogins
        primary key (LoginProvider, ProviderKey)
)
go

create index IX_AspNetUserLogins_UserId
    on AspNetUserLogins (UserId)
go

create table AspNetUserRoles
(
    UserId nvarchar(450) not null
        constraint FK_AspNetUserRoles_AspNetUsers_UserId
            references AspNetUsers
            on delete cascade,
    RoleId nvarchar(450) not null
        constraint FK_AspNetUserRoles_AspNetRoles_RoleId
            references AspNetRoles
            on delete cascade,
    constraint PK_AspNetUserRoles
        primary key (UserId, RoleId)
)
go

create index IX_AspNetUserRoles_RoleId
    on AspNetUserRoles (RoleId)
go

create table AspNetUserTokens
(
    UserId nvarchar(450) not null
        constraint FK_AspNetUserTokens_AspNetUsers_UserId
            references AspNetUsers
            on delete cascade,
    LoginProvider nvarchar(450) not null,
    Name nvarchar(450) not null,
    Value nvarchar(max),
    constraint PK_AspNetUserTokens
        primary key (UserId, LoginProvider, Name)
)
go

create index EmailIndex
    on AspNetUsers (NormalizedEmail)
go

create unique index UserNameIndex
    on AspNetUsers (NormalizedUserName)
    where [NormalizedUserName] IS NOT NULL
go

create table CourseProjects
(
    Id nvarchar(450) not null
        constraint PK_CourseProjects
            primary key,
    CreationDate datetime2 not null,
    UserId nvarchar(450) not null
        constraint CourseProjects_AspNetUsers_Id_fk
            references AspNetUsers,
    Title nvarchar(max) not null,
    Description nvarchar(max)
)
go

create table Files
(
    Id nvarchar(450) not null
        constraint Files_pk
            primary key nonclustered,
    ProjectId nvarchar(450)
        constraint Files_CourseProjects_Id_fk
            references CourseProjects,
    FilePath nvarchar(max) not null,
    Name nvarchar(max)
)
go

