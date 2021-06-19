namespace AutomationTests.DataAccessLayerTests
{
    using System;

    using AutomationTests.TestHelpers;
    using Xunit;

    [Collection(nameof(StorageEmulatorCollectionFixture))]
    public sealed class AzureTableRepositoryTests : IDisposable
    {
        private readonly AzureTableRepositorySteps steps;

        private bool disposed;

        public AzureTableRepositoryTests(AzureStorageEmulator storageEmulator)
        {
            this.steps = new AzureTableRepositorySteps(storageEmulator);
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.steps.Dispose();
            this.disposed = true;
        }
    }
}