using System.Diagnostics;

namespace ReaperKing.Core
{
    public static class ShellHelper
    {
        public static int Run(string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            
            process.Start();
            process.WaitForExit();
            return process.ExitCode;
        }
    }
}