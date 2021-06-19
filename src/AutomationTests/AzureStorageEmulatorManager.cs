namespace AutomationTests
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    public static class AzureStorageEmulatorManager
    {
        private const string EmulatorDirectoryPath = @"C:\Program Files (x86)\Microsoft SDKs\Azure\Storage Emulator";
        private const string NewEmulatorName = "AzureStorageEmulator.exe";
        private const string OldEmulatorName = "WAStorageEmulator.exe";

        public static bool IsProcessRunning()
        {
            bool status;

            using (var process = Process.Start(Create(ProcessCommand.Status)))
            {
                if (process == null)
                {
                    throw new InvalidOperationException("Unable to start process.");
                }

                status = GetStatus(process);
                process.WaitForExit();

            }

            return status;
        }

        public static void StartStorageEmulator()
        {
            if (!IsProcessRunning())
            {
                ExecuteProcess(ProcessCommand.Init);
                ExecuteProcess(ProcessCommand.Start)
            }
        }

        public static void StopStorageEmulator()
        {
            if (IsProcessRunning())
            {
                ExecuteProcess(ProcessCommand.Stop);
            }
        }

        private static void ExecuteProcess(ProcessCommand command)
        {
            string error;

            using (var process = Process.Start(Create(command)))
            {
                if (process == null)
                {
                    throw new InvalidOperationException("Unable to start process");
                }

                error = GetError(process);
                process.WaitForExit();
            }

            if (!string.IsNullOrEmpty(error))
            {
                throw new InvalidOperationException(error);
            }
        }

        private static string GetError(Process process)
        {
            var output = process.StandardError.ReadToEnd();
            return output.Split(':').Select(part => part.Trim()).Last();
        }

        private static bool GetStatus(Process process)
        {
            var output = process.StandardOutput.ReadToEnd();
            var isRunningLine = output
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .SingleOrDefault(line => line.StartsWith("IsRunning", StringComparison.OrdinalIgnoreCase));

            if (isRunningLine == null)
            {
                return false;
            }

            return bool.Parse(isRunningLine.Split(':').Select(part => part.Trim()).Last());
        }

        private static ProcessStartInfo Create(ProcessCommand command)
        {
            string filepath = Path.Combine(EmulatorDirectoryPath, NewEmulatorName);

            if (!File.Exists(filepath))
            {
                filepath = Path.Combine(EmulatorDirectoryPath, OldEmulatorName);
            }

            return new ProcessStartInfo
            {
                FileName = filepath,
                Arguments = GetCommandArgument(),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            string GetCommandArgument()
            {
                return command == ProcessCommand.Init ?
                    @"init /server (localdb)\MSSQLLocalDb /forceCreate" :
                    command.ToString().ToUpperInvariant();
            }
        }
    }

    internal enum ProcessCommand
    { 
        Start,
        Stop,
        Status,
        Init
    }
}