/*
    AddressDatabase
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
    AppUserId uniqueidentifier NOT NULL
        CONSTRAINT DF_AppUser_AppUserId_NewSequentialId
        DEFAULT NEWSEQUENTIALID(),
    CONSTRAINT PK_AppUser
        PRIMARY KEY CLUSTERED (AppUserId),

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
    AppGroupId uniqueidentifier NOT NULL
        CONSTRAINT DF_AppGroup_AppGroupId_NewSequentialId
        DEFAULT NEWSEQUENTIALID(),
    CONSTRAINT PK_AppGroup
        PRIMARY KEY CLUSTERED (AppGroupId),

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


CREATE TABLE security.AppPermission
(
    AppPermissionId uniqueidentifier NOT NULL
        CONSTRAINT DF_AppPermission_AppPermissionId_NewSequentialId
        DEFAULT NEWSEQUENTIALID(),
    CONSTRAINT PK_AppPermission
        PRIMARY KEY CLUSTERED (AppPermissionId),

    Code nvarchar(50) NOT NULL,
    NameEnGb nvarchar(100) NOT NULL,
    Description nvarchar(1000) NULL,
    Active bit NOT NULL
        CONSTRAINT DF_AppPermission_Active
        DEFAULT 1,
    CreatedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_AppPermission_CreatedUtc
        DEFAULT SYSUTCDATETIME(),
    ModifiedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_AppPermission_ModifiedUtc
        DEFAULT SYSUTCDATETIME(),
    RowVersion rowversion NOT NULL,

    CONSTRAINT UQ_AppPermission_Code
        UNIQUE (Code)
);
GO


CREATE TABLE security.AppUserAppGroup
(
    AppUserId uniqueidentifier NOT NULL,
    AppGroupId uniqueidentifier NOT NULL,
    RowVersion rowversion NOT NULL,

    -- AppGroupId first because group -> users is assumed to be the primary lookup direction
    CONSTRAINT PK_AppUserAppGroup
        PRIMARY KEY CLUSTERED
        (
            AppGroupId,
            AppUserId
        ),
    CONSTRAINT FK_AppUserAppGroup_AppUser
        FOREIGN KEY (AppUserId)
        REFERENCES security.AppUser(AppUserId),
    CONSTRAINT FK_AppUserAppGroup_AppGroup
        FOREIGN KEY (AppGroupId)
        REFERENCES security.AppGroup(AppGroupId)
);
GO


CREATE TABLE security.AppUserAppPermission
(
    AppUserId uniqueidentifier NOT NULL,
    AppPermissionId uniqueidentifier NOT NULL,
    RowVersion rowversion NOT NULL,

    -- AppPermissionId first because app permission -> users is assumed to be the primary lookup direction
    CONSTRAINT PK_AppUserAppPermission
        PRIMARY KEY CLUSTERED
        (
            AppPermissionId,
            AppUserId
        ),
    CONSTRAINT FK_AppUserAppPermission_AppUser
        FOREIGN KEY (AppUserId)
        REFERENCES security.AppUser(AppUserId),
    CONSTRAINT FK_AppUserAppPermission_AppPermission
        FOREIGN KEY (AppPermissionId)
        REFERENCES security.AppPermission(AppPermissionId)
);
GO


CREATE TABLE security.AppGroupAppPermission
(
    AppGroupId uniqueidentifier NOT NULL,
    AppPermissionId uniqueidentifier NOT NULL,
    RowVersion rowversion NOT NULL,

    -- AppGroupId first because group -> app permissions is assumed to be the primary lookup direction
    CONSTRAINT PK_AppGroupAppPermission
        PRIMARY KEY CLUSTERED
        (
            AppGroupId,
            AppPermissionId
        ),
    CONSTRAINT FK_AppGroupAppPermission_AppGroup
        FOREIGN KEY (AppGroupId)
        REFERENCES security.AppGroup(AppGroupId),
    CONSTRAINT FK_AppGroupAppPermission_AppPermission
        FOREIGN KEY (AppPermissionId)
        REFERENCES security.AppPermission(AppPermissionId)
);
GO


CREATE TABLE security.AppUserAddress
(
    AppUserId uniqueidentifier NOT NULL,
    AddressId uniqueidentifier NOT NULL,
    RowVersion rowversion NOT NULL,
    
    CONSTRAINT PK_AppUserAddress
        PRIMARY KEY CLUSTERED
        (
            AppUserId,
            AddressId
        ),
    CONSTRAINT FK_AppUserAddress_AppUser
        FOREIGN KEY (AppUserId)
        REFERENCES security.AppUser(AppUserId),
    CONSTRAINT FK_AppUserAddress_Address
        FOREIGN KEY (AddressId)
        REFERENCES address.Address(AddressId)
);
GO


CREATE TABLE security.AppUserCategory
(
    AppUserCategoryId uniqueidentifier NOT NULL
        CONSTRAINT DF_AppUserCategory_AppUserCategoryId_NewSequentialId
        DEFAULT NEWSEQUENTIALID(),
    CONSTRAINT PK_AppUserCategory
        PRIMARY KEY CLUSTERED (AppUserCategoryId),

    Code nvarchar(50) NOT NULL,
    NameEnGb nvarchar(100) NOT NULL,
    Description nvarchar(1000) NULL,
    Active bit NOT NULL
        CONSTRAINT DF_AppUserCategory_Active
        DEFAULT 1,
    CreatedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_AppUserCategory_CreatedUtc
        DEFAULT SYSUTCDATETIME(),
    ModifiedUtc datetime2(7) NOT NULL
        CONSTRAINT DF_AppUserCategory_ModifiedUtc
        DEFAULT SYSUTCDATETIME(),
    RowVersion rowversion NOT NULL,

    CONSTRAINT UQ_AppUserCategory_Code
        UNIQUE (Code)
);
GO


CREATE TABLE security.AppUserAppUserCategory
(
    AppUserId uniqueidentifier NOT NULL,
    AppUserCategoryId uniqueidentifier NOT NULL,
    RowVersion rowversion NOT NULL,
    
    -- AppUserCategoryId first because category -> appusers is assumed to be the primary lookup direction
    CONSTRAINT PK_AppUserAppUserCategory
        PRIMARY KEY CLUSTERED
        (
            AppUserCategoryId,
            AppUserId
        ),
    CONSTRAINT FK_AppUserAppUserCategory_AppUser
        FOREIGN KEY (AppUserId)
        REFERENCES security.AppUser(AppUserId),
    CONSTRAINT FK_AppUserAppUserCategory_AppUserCategory
        FOREIGN KEY (AppUserCategoryId)
        REFERENCES security.AppUserCategory(AppUserCategoryId)
);
GO

DELETE FROM [security].[AppUser]
INSERT INTO [security].[AppUser]
	(Username, PasswordHash, Active)
	VALUES
	('admin','$2a$11$lLCjSYzZ7oE/7k1xDyn17eHPGyXgM0e7AiCQ7g934I6FrADoQytBe', 1)

SELECT * FROM [security].[AppUser] ORDER BY [AppUserId]