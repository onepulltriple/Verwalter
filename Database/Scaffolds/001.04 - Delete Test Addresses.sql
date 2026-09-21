USE AddressDatabase;
GO

SET NOCOUNT ON;
GO

BEGIN TRANSACTION;

BEGIN TRY

    -- 1. Many-to-many relationships
    DELETE FROM address.AddressAddressCategory
    WHERE AddressId BETWEEN
        '60000000-0000-0000-0000-000000000001'
        AND
        '60000000-0000-0000-0000-000000000030';


    -- 2. Addresses
    DELETE FROM address.Address
    WHERE AddressId BETWEEN
        '60000000-0000-0000-0000-000000000001'
        AND
        '60000000-0000-0000-0000-000000000030';


    -- 3. Places
    DELETE FROM address.Place
    WHERE PlaceId BETWEEN
        '40000000-0000-0000-0000-000000000001'
        AND
        '40000000-0000-0000-0000-000000000006';


    -- 4. Postcodes
    DELETE FROM address.Postcode
    WHERE PostcodeId BETWEEN
        '20000000-0000-0000-0000-000000000001'
        AND
        '20000000-0000-0000-0000-000000000006';


    -- 5. Cities
    DELETE FROM address.City
    WHERE CityId BETWEEN
        '30000000-0000-0000-0000-000000000001'
        AND
        '30000000-0000-0000-0000-000000000006';


    -- 6. Address category
    DELETE FROM address.AddressCategory
    WHERE AddressCategoryId =
        '50000000-0000-0000-0000-000000000001';


    -- 7. Countries
    DELETE FROM address.Country
    WHERE CountryId BETWEEN
        '10000000-0000-0000-0000-000000000001'
        AND
        '10000000-0000-0000-0000-000000000002';


    COMMIT TRANSACTION;

END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;

END CATCH;
GO
