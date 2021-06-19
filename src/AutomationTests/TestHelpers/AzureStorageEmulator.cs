namespace AutomationTests.TestHelpers
{
    using System;
    using Microsoft.Azure.Cosmos.Table;

    public sealed class AzureStorageEmulator : IDisposable
    {
        private const string EmulatorConnectionString = "UserDevelopmentStorage=true";

        private bool disposed;

        public AzureStorageEmulator()
        {
            var storageAccount = CloudStorageAccount.Parse(EmulatorConnectionString);
            this.TableClient = new GuardTableClient(storageAccount.CreateCloudTableClient());

            if (AzureStorageEmulatorManager.IsProcessRunning())
            {
                AzureStorageEmulatorManager.StopStorageEmulator();
            }

            AzureStorageEmulatorManager.StartStorageEmulator();
        }

        public GuardTableClient TableClient { get; set; }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.TableClient.Dispose();
            this.disposed = true;
        }
    }
}