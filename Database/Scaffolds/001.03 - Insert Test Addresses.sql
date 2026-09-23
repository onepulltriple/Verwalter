USE AddressDatabase;
GO

SET NOCOUNT ON;
GO

BEGIN TRANSACTION;

BEGIN TRY

    -------------------------------------------------------------------------
    -- Countries
    -------------------------------------------------------------------------

    DECLARE @AustriaId uniqueidentifier =
        '10000000-0000-0000-0000-000000000001';

    DECLARE @GermanyId uniqueidentifier =
        '10000000-0000-0000-0000-000000000002';


    INSERT INTO address.Country
    (
        CountryId,
        IsoAlpha2,
        IsoAlpha3,
        NameEnGb
    )
    VALUES
    (
        @AustriaId,
        'AT',
        'AUT',
        N'Austria'
    ),
    (
        @GermanyId,
        'DE',
        'DEU',
        N'Germany'
    );


    -------------------------------------------------------------------------
    -- Postcodes
    -------------------------------------------------------------------------

    DECLARE @ViennaPostcodeId uniqueidentifier =
        '20000000-0000-0000-0000-000000000001';

    DECLARE @InnsbruckPostcodeId uniqueidentifier =
        '20000000-0000-0000-0000-000000000002';

    DECLARE @HamburgPostcodeId uniqueidentifier =
        '20000000-0000-0000-0000-000000000003';

    DECLARE @KemptenPostcodeId uniqueidentifier =
        '20000000-0000-0000-0000-000000000004';

    DECLARE @BerlinPostcodeId uniqueidentifier =
        '20000000-0000-0000-0000-000000000005';

    DECLARE @MunichPostcodeId uniqueidentifier =
        '20000000-0000-0000-0000-000000000006';


    INSERT INTO address.Postcode
    (
        PostcodeId,
        CountryId,
        Postcode
    )
    VALUES
    (
        @ViennaPostcodeId,
        @AustriaId,
        N'1010'
    ),
    (
        @InnsbruckPostcodeId,
        @AustriaId,
        N'6020'
    ),
    (
        @HamburgPostcodeId,
        @GermanyId,
        N'20095'
    ),
    (
        @KemptenPostcodeId,
        @GermanyId,
        N'87435'
    ),
    (
        @BerlinPostcodeId,
        @GermanyId,
        N'10115'
    ),
    (
        @MunichPostcodeId,
        @GermanyId,
        N'80331'
    );


    -------------------------------------------------------------------------
    -- Cities
    -------------------------------------------------------------------------

    DECLARE @ViennaCityId uniqueidentifier =
        '30000000-0000-0000-0000-000000000001';

    DECLARE @InnsbruckCityId uniqueidentifier =
        '30000000-0000-0000-0000-000000000002';

    DECLARE @HamburgCityId uniqueidentifier =
        '30000000-0000-0000-0000-000000000003';

    DECLARE @KemptenCityId uniqueidentifier =
        '30000000-0000-0000-0000-000000000004';

    DECLARE @BerlinCityId uniqueidentifier =
        '30000000-0000-0000-0000-000000000005';

    DECLARE @MunichCityId uniqueidentifier =
        '30000000-0000-0000-0000-000000000006';


    INSERT INTO address.City
    (
        CityId,
        CountryId,
        NameEnGb
    )
    VALUES
    (
        @ViennaCityId,
        @AustriaId,
        N'Vienna'
    ),
    (
        @InnsbruckCityId,
        @AustriaId,
        N'Innsbruck'
    ),
    (
        @HamburgCityId,
        @GermanyId,
        N'Hamburg'
    ),
    (
        @KemptenCityId,
        @GermanyId,
        N'Kempten'
    ),
    (
        @BerlinCityId,
        @GermanyId,
        N'Berlin'
    ),
    (
        @MunichCityId,
        @GermanyId,
        N'Munich'
    );


    -------------------------------------------------------------------------
    -- Places
    --
    -- One place per city/postcode combination.
    -------------------------------------------------------------------------

    DECLARE @ViennaPlaceId uniqueidentifier =
        '40000000-0000-0000-0000-000000000001';

    DECLARE @InnsbruckPlaceId uniqueidentifier =
        '40000000-0000-0000-0000-000000000002';

    DECLARE @HamburgPlaceId uniqueidentifier =
        '40000000-0000-0000-0000-000000000003';

    DECLARE @KemptenPlaceId uniqueidentifier =
        '40000000-0000-0000-0000-000000000004';

    DECLARE @BerlinPlaceId uniqueidentifier =
        '40000000-0000-0000-0000-000000000005';

    DECLARE @MunichPlaceId uniqueidentifier =
        '40000000-0000-0000-0000-000000000006';


    INSERT INTO address.Place
    (
        PlaceId,
        CountryId,
        PostcodeId,
        CityId,
        NameEnGb
    )
    VALUES
    (
        @ViennaPlaceId,
        @AustriaId,
        @ViennaPostcodeId,
        @ViennaCityId,
        N'Vienna, 1010 Austria'
    ),
    (
        @InnsbruckPlaceId,
        @AustriaId,
        @InnsbruckPostcodeId,
        @InnsbruckCityId,
        N'Innsbruck, 6020 Austria'
    ),
    (
        @HamburgPlaceId,
        @GermanyId,
        @HamburgPostcodeId,
        @HamburgCityId,
        N'Hamburg, 20095 Germany'
    ),
    (
        @KemptenPlaceId,
        @GermanyId,
        @KemptenPostcodeId,
        @KemptenCityId,
        N'Kempten, 87435 Germany'
    ),
    (
        @BerlinPlaceId,
        @GermanyId,
        @BerlinPostcodeId,
        @BerlinCityId,
        N'Berlin, 10115 Germany'
    ),
    (
        @MunichPlaceId,
        @GermanyId,
        @MunichPostcodeId,
        @MunichCityId,
        N'Munich, 80331 Germany'
    );


    -------------------------------------------------------------------------
    -- Address category
    -------------------------------------------------------------------------

    DECLARE @TestAddressCategoryId uniqueidentifier =
        '50000000-0000-0000-0000-000000000001';


    INSERT INTO address.AddressCategory
    (
        AddressCategoryId,
        Code,
        NameEnGb,
        Description
    )
    VALUES
    (
        @TestAddressCategoryId,
        N'TEST_ADDRESS',
        N'Test Address',
        N'Test addresses used during application development.'
    );


    -------------------------------------------------------------------------
    -- Addresses
    -------------------------------------------------------------------------

    INSERT INTO address.Address
    (
        AddressId,
        PlaceId,
        AddressLine1,
        AddressLine2,
        AddressLine3,
        Name1,
        Name2
    )
    VALUES

    -------------------------------------------------------------------------
    -- Vienna: 7 addresses
    -------------------------------------------------------------------------

    (
        '60000000-0000-0000-0000-000000000001',
        @ViennaPlaceId,
        N'1 Test Street',
        NULL,
        NULL,
        N'Anna Berger',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000002',
        @ViennaPlaceId,
        N'12 Examplegasse',
        N'Apartment 2',
        NULL,
        N'Markus Steiner',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000003',
        @ViennaPlaceId,
        N'25 Sampleplatz',
        NULL,
        NULL,
        N'Julia Wagner',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000004',
        @ViennaPlaceId,
        N'31 Developmentstraße',
        N'Floor 2',
        NULL,
        N'Thomas Bauer',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000005',
        @ViennaPlaceId,
        N'44 Testgasse',
        NULL,
        NULL,
        N'Sabine Hofer',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000006',
        @ViennaPlaceId,
        N'58 Examplegasse',
        N'Door 8',
        NULL,
        N'Peter Maier',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000007',
        @ViennaPlaceId,
        N'73 Musterstraße',
        NULL,
        NULL,
        N'Laura Fischer',
        NULL
    ),

    -------------------------------------------------------------------------
    -- Innsbruck: 3 addresses
    -------------------------------------------------------------------------

    (
        '60000000-0000-0000-0000-000000000008',
        @InnsbruckPlaceId,
        N'5 Testweg',
        NULL,
        NULL,
        N'Michael Gruber',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000009',
        @InnsbruckPlaceId,
        N'18 Beispielstraße',
        N'Apartment 4',
        NULL,
        N'Elisabeth Leitner',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000010',
        @InnsbruckPlaceId,
        N'29 Musterweg',
        NULL,
        NULL,
        N'Andreas Moser',
        NULL
    ),

    -------------------------------------------------------------------------
    -- Hamburg: 6 addresses
    -------------------------------------------------------------------------

    (
        '60000000-0000-0000-0000-000000000011',
        @HamburgPlaceId,
        N'3 Teststraße',
        NULL,
        NULL,
        N'Klara Schmidt',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000012',
        @HamburgPlaceId,
        N'15 Beispielweg',
        NULL,
        NULL,
        N'Jan Peters',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000013',
        @HamburgPlaceId,
        N'22 Musterstraße',
        N'Office 3',
        NULL,
        N'Felix Koch',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000014',
        @HamburgPlaceId,
        N'37 Testweg',
        NULL,
        NULL,
        N'Lisa Meyer',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000015',
        @HamburgPlaceId,
        N'48 Beispielstraße',
        N'Floor 1',
        NULL,
        N'Oliver Braun',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000016',
        @HamburgPlaceId,
        N'61 Entwicklungsgasse',
        NULL,
        NULL,
        N'Emma Hoffmann',
        NULL
    ),

    -------------------------------------------------------------------------
    -- Kempten: 4 addresses
    -------------------------------------------------------------------------

    (
        '60000000-0000-0000-0000-000000000017',
        @KemptenPlaceId,
        N'4 Teststraße',
        NULL,
        NULL,
        N'Georg Weber',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000018',
        @KemptenPlaceId,
        N'11 Beispielweg',
        NULL,
        NULL,
        N'Claudia Richter',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000019',
        @KemptenPlaceId,
        N'27 Musterstraße',
        N'Apartment 5',
        NULL,
        N'Stefan Klein',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000020',
        @KemptenPlaceId,
        N'39 Testweg',
        NULL,
        NULL,
        N'Nicole Wolf',
        NULL
    ),

    -------------------------------------------------------------------------
    -- Berlin: 7 addresses
    -------------------------------------------------------------------------

    (
        '60000000-0000-0000-0000-000000000021',
        @BerlinPlaceId,
        N'2 Teststraße',
        NULL,
        NULL,
        N'Daniel Neumann',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000022',
        @BerlinPlaceId,
        N'14 Beispielstraße',
        N'Apartment 3',
        NULL,
        N'Sarah Schwarz',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000023',
        @BerlinPlaceId,
        N'21 Musterweg',
        NULL,
        NULL,
        N'Christoph Zimmermann',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000024',
        @BerlinPlaceId,
        N'33 Testgasse',
        NULL,
        NULL,
        N'Katharina Braun',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000025',
        @BerlinPlaceId,
        N'47 Entwicklungsstraße',
        N'Office 2',
        NULL,
        N'Matthias Hartmann',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000026',
        @BerlinPlaceId,
        N'56 Beispielweg',
        NULL,
        NULL,
        N'Franziska Krüger',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000027',
        @BerlinPlaceId,
        N'68 Teststraße',
        NULL,
        NULL,
        N'Tobias Werner',
        NULL
    ),

    -------------------------------------------------------------------------
    -- Munich: 3 addresses
    -------------------------------------------------------------------------

    (
        '60000000-0000-0000-0000-000000000028',
        @MunichPlaceId,
        N'6 Teststraße',
        NULL,
        NULL,
        N'Johanna Keller',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000029',
        @MunichPlaceId,
        N'19 Beispielweg',
        N'Apartment 7',
        NULL,
        N'Wolfgang Bauer',
        NULL
    ),
    (
        '60000000-0000-0000-0000-000000000030',
        @MunichPlaceId,
        N'42 Musterstraße',
        NULL,
        NULL,
        N'Marie Schuster',
        NULL
    );


    -------------------------------------------------------------------------
    -- Address / Category relationships
    -------------------------------------------------------------------------

    INSERT INTO address.AddressAddressCategory
    (
        AddressId,
        AddressCategoryId
    )
    SELECT
        AddressId,
        @TestAddressCategoryId
    FROM address.Address
    WHERE AddressId BETWEEN
        '60000000-0000-0000-0000-000000000001'
        AND
        '60000000-0000-0000-0000-000000000030';


    COMMIT TRANSACTION;

END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;

END CATCH;
GO
