namespace AutomationTests.DataAccessLayerTests
{
    using System;
    using AutomationTests.TestHelpers;

    internal sealed class AzureTableRepositorySteps : IDisposable
    {
        private readonly AzureStorageEmulator storageEmulator;

        private bool disposed;

        public AzureTableRepositorySteps(AzureStorageEmulator storageEmulator)
        {
            this.storageEmulator = storageEmulator;
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.storageEmulator.Dispose();
            this.disposed = true;
        }
    }
}