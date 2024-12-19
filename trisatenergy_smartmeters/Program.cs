using DotNetEnv;

namespace SmartMeterSimulation
{
    /// <summary>
    /// Entry point of the application that runs the smart meter simulation.
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            // Get the current directory (Program.cs location)
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Traverse upwards through the directories until we find the .env file
            string envFilePath = TraverseForEnvFile(currentDirectory);

            if (envFilePath != null)
            {
                Console.WriteLine("Found .env file at: " + envFilePath);
                Env.Load(envFilePath); // Load the .env file
            }
            else
            {
                Console.WriteLine("No .env file found.");
            }

            // SimulateAsync 24 hours (1 day)
            int hours = 24;
            SmartMeter smartMeter = new SmartMeter(hours);
            await smartMeter.SimulateAsync(hours);
        }
        // Method to traverse up the directory tree to find the .env file
        static string TraverseForEnvFile(string startingDirectory)
        {
            string directory = startingDirectory;

            while (directory != null)
            {
                // Look for the .env file in the current directory
                string envFilePath = Path.Combine(directory, ".env");
                if (File.Exists(envFilePath))
                {
                    return envFilePath; // Return the path if found
                }

                // Move up one level in the directory hierarchy
                directory = Directory.GetParent(directory)?.FullName;
            }

            // Return null if no .env file was found after traversing all directories
            return null;
        }
    }
}