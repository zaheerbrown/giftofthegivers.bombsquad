# Zaheer Part 2 - Azure Functions + Web Integration

## What was added/fixed
- Added `GiftOfTheGivers.Functions` (.NET 10 isolated Azure Functions project).
- Added HTTP trigger `GenerateTaxCertificate`.
- The web donation flow POSTs donation details to the function and stores the returned certificate number.
- Added a donation Create view and corrected donation list fields.
- Fixed duplicated/broken `Program.cs`, DbSets, namespaces, Identity roles, and role seeding.

## Run locally in Visual Studio 2026
1. Open `giftofthegivers.slnx`.
2. Right-click the solution > **Configure Startup Projects**.
3. Choose **Multiple startup projects**.
4. Set both `giftofthegivers` and `GiftOfTheGivers.Functions` to **Start**.
5. Ensure Azurite is available/running (the Functions project uses `UseDevelopmentStorage=true`).
6. Start the solution.
7. Functions should show `GenerateTaxCertificate` at a URL similar to `http://localhost:7071/api/GenerateTaxCertificate`.
8. Open that URL in a browser (GET) to prove the function is alive.
9. In the web app go to `/Donation/Create`, submit a donation, and confirm the certificate page appears with a `GOTG-TAX-...` number.

## Postman POST test
POST to `http://localhost:7071/api/GenerateTaxCertificate`
Content-Type: application/json

```json
{
  "donationId": 101,
  "donorName": "Mogamat Zaheer Brown",
  "donorEmail": "zaheer@example.com",
  "amount": 500.00,
  "currency": "ZAR",
  "taxReferenceNumber": "TEST-001"
}
```

## Publish
Publish the `GiftOfTheGivers.Functions` project from Visual Studio to an Azure Function App. After deployment, copy the deployed function base URL (ending in `/api/`) into `AzureFunctions:BaseUrl` for the web app's Azure configuration/app settings, then test the donation flow again.

## Screenshots to capture
1. Solution Explorer showing both projects.
2. `GenerateTaxCertificate.cs` showing `[Function]`, `[HttpTrigger]`, validation and returned certificate.
3. Local Functions console showing the local URL.
4. Browser/Postman successful GET/POST response.
5. Web app certificate page proving integration.
6. Visual Studio Publish success / Azure Function App overview.
7. Deployed Function URL and successful deployed response.
