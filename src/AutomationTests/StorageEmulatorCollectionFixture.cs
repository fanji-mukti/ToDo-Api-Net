namespace AutomationTests
{
    using AutomationTests.TestHelpers;
    using Xunit;

    [CollectionDefinition(nameof(StorageEmulatorCollectionFixture))]
    public sealed class StorageEmulatorCollectionFixture : ICollectionFixture<AzureStorageEmulator>
    {
    }
}