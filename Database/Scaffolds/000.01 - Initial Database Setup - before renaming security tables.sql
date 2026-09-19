/*
    Address Database
    =================

    Schemas:
      - address  : geographic/address domain
      - security : users, groups, permissions

    Design ideas:
      - GUID primary keys for entity tables
      - NEWSEQUENTIALID() for entity identifiers
      - AddressId primary key is NONCLUSTERED
      - Address is CLUSTERED by PlaceId
      - Junction tables use composite primary keys instead of GUIDs
      - Composite key order follows the assumed primary lookup direction
      - Most tables have rowversion for change detection
      - No cascading deletes
      - UTC timestamps
      - Nullable ISO codes use filtered unique indexes
      - The triplet (CityId, PostcodeId, CountryId) which comprises a Place should always be a unique triplet
      - Don't validate whether a particular city/postcode/country combination is geographically legitimate
*/
USE MASTER
GO

DROP DATABASE IF EXISTS AddressDatabase;
CREATE DATABASE AddressDatabase;
GO

-- workaround to enable creation of database diagrams in SSMS
ALTER AUTHORIZATION ON DATABASE::AddressDatabase TO sa;

USE AddressDatabase;
GO

CREATE SCHEMA address;
GO

CREATE SCHEMA security;
GO


CREATE TABLE address.Country
(
    CountryId uniqueidentifier NOT NULL
        CONSTRAINT DF_Country_CountryId_NewSequentialId
        DEFAULT NEWSEQUENTIALID(),
    CONSTRAINT PK_Country
        PRIMARY KEY CLUSTERED (CountryId),

    IsoAlpha2 char(2) NULL,
    IsoAlpha3 char(3) NULL,
    NameEnGb nvarchar(100) NOT NULL,
    Active bit NOT NULL
        CONSTRAINT DF_Country_Active
        DEFAULT 1,
    CreatedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_Country_CreatedUtc
        DEFAULT SYSUTCDATETIME(),
    ModifiedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_Country_ModifiedUtc
        DEFAULT SYSUTCDATETIME(),
    RowVersion rowversion NOT NULL
);
GO

CREATE UNIQUE INDEX UX_Country_IsoAlpha2
    ON address.Country (IsoAlpha2)
    WHERE IsoAlpha2 IS NOT NULL;
GO

CREATE UNIQUE INDEX UX_Country_IsoAlpha3
    ON address.Country (IsoAlpha3)
    WHERE IsoAlpha3 IS NOT NULL;
GO


CREATE TABLE address.Postcode
(
    PostcodeId uniqueidentifier NOT NULL
        CONSTRAINT DF_Postcode_PostcodeId_NewSequentialId
        DEFAULT NEWSEQUENTIALID(),
    CONSTRAINT PK_Postcode
        PRIMARY KEY NONCLUSTERED (PostcodeId),

    CountryId uniqueidentifier NOT NULL,
    Postcode nvarchar(50) NOT NULL,
    CreatedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_Postcode_CreatedUtc
        DEFAULT SYSUTCDATETIME(),
    ModifiedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_Postcode_ModifiedUtc
        DEFAULT SYSUTCDATETIME(),
    RowVersion rowversion NOT NULL,
    CONSTRAINT FK_Postcode_Country
        FOREIGN KEY (CountryId)
        REFERENCES address.Country(CountryId),

    -- a postcode (the actual text of the postcode) can only exist once per country
    CONSTRAINT UQ_Postcode_Country_Postcode
        UNIQUE (CountryId, Postcode),

    -- a PostcodeId belongs to only one country
    -- candidate key required by the composite FK in the place table
    CONSTRAINT UQ_Postcode_Country_PostcodeId
        UNIQUE (CountryId, PostcodeId)
);
GO

    CREATE CLUSTERED INDEX CX_Postcode_Country
        ON address.Postcode (CountryId);
GO


CREATE TABLE address.City
(
    CityId uniqueidentifier NOT NULL
        CONSTRAINT DF_City_CityId_NewSequentialId
        DEFAULT NEWSEQUENTIALID(),
    CONSTRAINT PK_City
        PRIMARY KEY NONCLUSTERED (CityId),

    CountryId uniqueidentifier NOT NULL,
    NameEnGb nvarchar(100) NOT NULL,
    CreatedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_City_CreatedUtc
        DEFAULT SYSUTCDATETIME(),
    ModifiedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_City_ModifiedUtc
        DEFAULT SYSUTCDATETIME(),
    RowVersion rowversion NOT NULL,
    CONSTRAINT FK_City_Country
        FOREIGN KEY (CountryId)
        REFERENCES address.Country(CountryId),
    -- (a city's NameEnGb is allowed to occur more than once per country, therefore no constraint here)

    -- a CityId belongs to only one country 
    -- candidate key required by the composite FK in the place table
    CONSTRAINT UQ_City_Country_City
        UNIQUE (CountryId, CityId)

);
GO

    CREATE CLUSTERED INDEX CX_City_Country
        ON address.City (CountryId);
GO


CREATE TABLE address.Place
(
    PlaceId uniqueidentifier NOT NULL
        CONSTRAINT DF_Place_PlaceId_NewSequentialId
        DEFAULT NEWSEQUENTIALID(),
    CONSTRAINT PK_Place
        PRIMARY KEY NONCLUSTERED (PlaceId),

    CountryId uniqueidentifier NOT NULL,
    PostcodeId uniqueidentifier NOT NULL,
    CityId uniqueidentifier NOT NULL,
    NameEnGb nvarchar(100) NULL,
    CreatedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_Place_CreatedUtc
        DEFAULT SYSUTCDATETIME(),
    ModifiedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_Place_ModifiedUtc
        DEFAULT SYSUTCDATETIME(),
    RowVersion rowversion NOT NULL,

    CONSTRAINT FK_Place_Postcode
        FOREIGN KEY (CountryId, PostcodeId)
        REFERENCES address.Postcode(CountryId, PostcodeId),
    CONSTRAINT FK_Place_City
        FOREIGN KEY (CountryId, CityId)
        REFERENCES address.City(CountryId, CityId),

    -- the postcode, city, and place must all belong to the same country
    CONSTRAINT UQ_Place_Country_Postcode_City
        UNIQUE (CountryId, PostcodeId, CityId)
);
GO

CREATE CLUSTERED INDEX CX_Place_PostcodeId
    ON address.Place (PostcodeId);
GO


CREATE TABLE address.Address
(
    AddressId uniqueidentifier NOT NULL
        CONSTRAINT DF_Address_AddressId_NewSequentialId
        DEFAULT NEWSEQUENTIALID(),
    CONSTRAINT PK_Address
        PRIMARY KEY NONCLUSTERED (AddressId),

    PlaceId uniqueidentifier NOT NULL,
    AddressLine1 nvarchar(200) NOT NULL,
    AddressLine2 nvarchar(200) NULL,
    AddressLine3 nvarchar(200) NULL,
    Name1 nvarchar(200) NULL,
    Name2 nvarchar(200) NULL,
    CreatedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_Address_CreatedUtc
        DEFAULT SYSUTCDATETIME(),
    ModifiedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_Address_ModifiedUtc
        DEFAULT SYSUTCDATETIME(),
    RowVersion rowversion NOT NULL,

    CONSTRAINT FK_Address_Place
        FOREIGN KEY (PlaceId)
        REFERENCES address.Place(PlaceId)
);
GO

-- addresses belonging to the same place are clustered together
CREATE CLUSTERED INDEX CX_Address_Place
    ON address.Address (PlaceId);
GO


CREATE TABLE address.AddressCategory
(
    AddressCategoryId uniqueidentifier NOT NULL
        CONSTRAINT DF_AddressCategory_AddressCategoryId_NewSequentialId
        DEFAULT NEWSEQUENTIALID(),
    CONSTRAINT PK_AddressCategory
        PRIMARY KEY CLUSTERED (AddressCategoryId),

    Code nvarchar(50) NOT NULL,
    NameEnGb nvarchar(100) NOT NULL,
    Description nvarchar(1000) NULL,
    Active bit NOT NULL
        CONSTRAINT DF_AddressCategory_Active
        DEFAULT 1,
    CreatedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_AddressCategory_CreatedUtc
        DEFAULT SYSUTCDATETIME(),
    ModifiedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_AddressCategory_ModifiedUtc
        DEFAULT SYSUTCDATETIME(),
    RowVersion rowversion NOT NULL,

    CONSTRAINT UQ_AddressCategory_Code
        UNIQUE (Code)
);
GO


CREATE TABLE address.AddressAddressCategory
(
    AddressId uniqueidentifier NOT NULL,
    AddressCategoryId uniqueidentifier NOT NULL,
    RowVersion rowversion NOT NULL,

    -- AddressCategoryId first because category -> addresses is assumed to be the primary lookup direction
    CONSTRAINT PK_AddressAddressCategory
        PRIMARY KEY CLUSTERED
        (
            AddressCategoryId,
            AddressId
        ),
    CONSTRAINT FK_AddressAddressCategory_Address
        FOREIGN KEY (AddressId)
        REFERENCES address.Address(AddressId),
    CONSTRAINT FK_AddressAddressCategory_AddressCategory
        FOREIGN KEY (AddressCategoryId)
        REFERENCES address.AddressCategory(AddressCategoryId)
);
GO


CREATE TABLE security.AppUser
(
    UserId uniqueidentifier NOT NULL
        CONSTRAINT DF_AppUser_UserId_NewSequentialId
        DEFAULT NEWSEQUENTIALID(),
    CONSTRAINT PK_AppUser
        PRIMARY KEY CLUSTERED (UserId),

    Username nvarchar(100)
        COLLATE Latin1_General_100_CS_AS NOT NULL,
    EmailAddress nvarchar(100) NULL,
    PasswordHash nvarchar(255) NOT NULL,
    Active bit NOT NULL
        CONSTRAINT DF_AppUser_Active
        DEFAULT 1,
    LastLoginUtc datetime2(7) NULL,
    CreatedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_AppUser_CreatedUtc
        DEFAULT SYSUTCDATETIME(),
    ModifiedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_AppUser_ModifiedUtc
        DEFAULT SYSUTCDATETIME(),
    RowVersion rowversion NOT NULL,

    CONSTRAINT UQ_AppUser_Username
        UNIQUE (Username)
);
GO


CREATE TABLE security.AppGroup
(
    GroupId uniqueidentifier NOT NULL
        CONSTRAINT DF_AppGroup_GroupId
        DEFAULT NEWSEQUENTIALID(),
    CONSTRAINT PK_AppGroup
        PRIMARY KEY CLUSTERED (GroupId),

    Code nvarchar(50) NOT NULL,
    NameEnGb nvarchar(100) NOT NULL,
    Description nvarchar(1000) NULL,
    Active bit NOT NULL
        CONSTRAINT DF_AppGroup_Active
        DEFAULT 1,
    CreatedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_AppGroup_CreatedUtc
        DEFAULT SYSUTCDATETIME(),
    ModifiedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_AppGroup_ModifiedUtc
        DEFAULT SYSUTCDATETIME(),
    RowVersion rowversion NOT NULL,

    CONSTRAINT UQ_AppGroup_Code
        UNIQUE (Code)
);
GO


CREATE TABLE security.Permission
(
    PermissionId uniqueidentifier NOT NULL
        CONSTRAINT DF_Permission_PermissionId
        DEFAULT NEWSEQUENTIALID(),
    CONSTRAINT PK_Permission
        PRIMARY KEY CLUSTERED (PermissionId),

    Code nvarchar(50) NOT NULL,
    NameEnGb nvarchar(100) NOT NULL,
    Description nvarchar(1000) NULL,
    Active bit NOT NULL
        CONSTRAINT DF_Permission_Active
        DEFAULT 1,
    CreatedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_Permission_CreatedUtc
        DEFAULT SYSUTCDATETIME(),
    ModifiedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_Permission_ModifiedUtc
        DEFAULT SYSUTCDATETIME(),
    RowVersion rowversion NOT NULL,

    CONSTRAINT UQ_Permission_Code
        UNIQUE (Code)
);
GO


CREATE TABLE security.UserGroup
(
    UserId uniqueidentifier NOT NULL,
    GroupId uniqueidentifier NOT NULL,
    RowVersion rowversion NOT NULL,

    -- GroupId first because group -> users is assumed to be the primary lookup direction
    CONSTRAINT PK_UserGroup
        PRIMARY KEY CLUSTERED
        (
            GroupId,
            UserId
        ),
    CONSTRAINT FK_UserGroup_User
        FOREIGN KEY (UserId)
        REFERENCES security.AppUser(UserId),
    CONSTRAINT FK_UserGroup_Group
        FOREIGN KEY (GroupId)
        REFERENCES security.AppGroup(GroupId)
);
GO


CREATE TABLE security.UserPermission
(
    UserId uniqueidentifier NOT NULL,
    PermissionId uniqueidentifier NOT NULL,
    RowVersion rowversion NOT NULL,

    -- PermissionId first because permission -> users is assumed to be the primary lookup direction.
    CONSTRAINT PK_UserPermission
        PRIMARY KEY CLUSTERED
        (
            PermissionId,
            UserId
        ),
    CONSTRAINT FK_UserPermission_User
        FOREIGN KEY (UserId)
        REFERENCES security.AppUser(UserId),
    CONSTRAINT FK_UserPermission_Permission
        FOREIGN KEY (PermissionId)
        REFERENCES security.Permission(PermissionId)
);
GO


CREATE TABLE security.GroupPermission
(
    GroupId uniqueidentifier NOT NULL,
    PermissionId uniqueidentifier NOT NULL,
    RowVersion rowversion NOT NULL,

    -- GroupId first because group -> permissions is assumed to be the primary lookup direction.
    CONSTRAINT PK_GroupPermission
        PRIMARY KEY CLUSTERED
        (
            GroupId,
            PermissionId
        ),
    CONSTRAINT FK_GroupPermission_Group
        FOREIGN KEY (GroupId)
        REFERENCES security.AppGroup(GroupId),
    CONSTRAINT FK_GroupPermission_Permission
        FOREIGN KEY (PermissionId)
        REFERENCES security.Permission(PermissionId)
);
GO


CREATE TABLE security.UserAddress
(
    UserId uniqueidentifier NOT NULL,
    AddressId uniqueidentifier NOT NULL,
    RowVersion rowversion NOT NULL,
    
    CONSTRAINT PK_UserAddress
        PRIMARY KEY CLUSTERED
        (
            UserId,
            AddressId
        ),
    CONSTRAINT FK_UserAddress_User
        FOREIGN KEY (UserId)
        REFERENCES security.AppUser(UserId),
    CONSTRAINT FK_UserAddress_Address
        FOREIGN KEY (AddressId)
        REFERENCES address.Address(AddressId)
);
GO


CREATE TABLE security.UserCategory
(
    UserCategoryId uniqueidentifier NOT NULL
        CONSTRAINT DF_UserCategory_UserCategoryId_NewSequentialId
        DEFAULT NEWSEQUENTIALID(),
    CONSTRAINT PK_UserCategory
        PRIMARY KEY CLUSTERED (UserCategoryId),

    Code nvarchar(50) NOT NULL,
    NameEnGb nvarchar(100) NOT NULL,
    Description nvarchar(1000) NULL,
    Active bit NOT NULL
        CONSTRAINT DF_UserCategory_Active
        DEFAULT 1,
    CreatedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_UserCategory_CreatedUtc
        DEFAULT SYSUTCDATETIME(),
    ModifiedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_UserCategory_ModifiedUtc
        DEFAULT SYSUTCDATETIME(),
    RowVersion rowversion NOT NULL,

    CONSTRAINT UQ_UserCategory_Code
        UNIQUE (Code)
);
GO


CREATE TABLE security.UserUserCategory
(
    UserId uniqueidentifier NOT NULL,
    UserCategoryId uniqueidentifier NOT NULL,
    RowVersion rowversion NOT NULL,
    
    -- UserCategoryId first because category -> users is assumed to be the primary lookup direction
    CONSTRAINT PK_UserUserCategory
        PRIMARY KEY CLUSTERED
        (
            UserCategoryId,
            UserId
        ),
    CONSTRAINT FK_UserUserCategory_User
        FOREIGN KEY (UserId)
        REFERENCES security.AppUser(UserId),
    CONSTRAINT FK_UserUserCategory_UserCategory
        FOREIGN KEY (UserCategoryId)
        REFERENCES security.UserCategory(UserCategoryId)
);
GO