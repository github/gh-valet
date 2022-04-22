using System.Collections.Specialized;
using Valet.Interfaces;

namespace Valet.Services;

using System;
using System.Diagnostics;
using System.Threading.Tasks;

public class ProcessService : IProcessService
{
    public Task<bool> RunAsync(
        string filename,
        string arguments,
        string? cwd = null,
        IEnumerable<(string, string)>? environmentVariables = null,
        bool output = true)
    {
        var tcs = new TaskCompletionSource<bool>();
        var cts = new CancellationTokenSource();

        var startInfo = new ProcessStartInfo
        {
            FileName = filename,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            WorkingDirectory = cwd,
            CreateNoWindow = true
        };

        if (environmentVariables != null)
        {
            foreach (var (key, value) in environmentVariables)
            {
                startInfo.EnvironmentVariables.Add(key, value);
            }
        }

        var process = new Process
        {
            StartInfo = startInfo,
            EnableRaisingEvents = true,
        };

        void OnProcessExited(object? sender, EventArgs args)
        {
            process.Exited -= OnProcessExited;

            cts.Cancel();
            if (process.ExitCode == 0)
            {
                tcs.TrySetResult(true);
            }
            else
            {
                var error = process.StandardError.ReadToEnd();
                tcs.TrySetException(new Exception(error));
            }

            process.Dispose();
        }

        process.Exited += OnProcessExited;
        process.Start();

        Read(process.StandardOutput, output, cts.Token);
        Read(process.StandardError, output, cts.Token);

        return tcs.Task;
    }
    
    private static void Read(StreamReader reader, bool output, CancellationToken ctx)
    {
        if (!output) return;

        Task.Run(() =>
        {
            while (true)
            {
                int current;
                while ((current = reader.Read()) >= 0)
                    Console.Write((char)current);
            }
        }, ctx);
    }
}