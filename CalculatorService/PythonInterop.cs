using System;
using System.Diagnostics;
using System.IO;

namespace CalculatorService
{
    public static class PythonInterop
    {
        private static string fileName = null;
        public static string Execute(string arguments, string workingDirectoryRelativePath = "Python\\")
        {
            string result;
            if (fileName == null)
            {
                fileName = "python";//Startup.Configuration.GetSection("PythonExePath").Value;
                var fileExists = File.Exists(fileName);
                Console.WriteLine();
                Console.WriteLine("** PythonInterop First Run **");
                Console.WriteLine("WorkingDirectory: " + AppDomain.CurrentDomain.BaseDirectory + workingDirectoryRelativePath);
                Console.WriteLine("FileName: " + fileName);
                Console.WriteLine("FileExists: " + fileExists);
                Console.WriteLine("Arguments: " + arguments);
                Console.WriteLine();
            }
            try
            {
                ProcessStartInfo start = new ProcessStartInfo();
                start.WorkingDirectory = GetPlatformSpecificPath(AppDomain.CurrentDomain.BaseDirectory + workingDirectoryRelativePath);
                start.FileName = fileName;// start.WorkingDirectory + "python.exe";
                start.Arguments = arguments;
                start.UseShellExecute = false;
                start.RedirectStandardOutput = true;
                start.RedirectStandardError = true;

                using (Process process = Process.Start(start))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        result = reader.ReadToEnd();
                    }
                    using (StreamReader reader = process.StandardError)
                    {
                        string error = reader.ReadToEnd();
                        if (!string.IsNullOrEmpty(error)) result += "\n" + error;
                    }
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null) ex = ex.InnerException;
                result = ex.Message + "\n\n" + ex.StackTrace;
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                File.Create(AppDomain.CurrentDomain.BaseDirectory + workingDirectoryRelativePath + "working.dir").Close();
            }
            return result;
        }

        private static string GetPlatformSpecificPath(string path)
        {
            if (Path.DirectorySeparatorChar == '\\') return path;
            else return path.Replace('\\', Path.DirectorySeparatorChar);
        }
    }
}