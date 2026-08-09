using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using KY.Core;

namespace KY.Generator
{
    /// <summary>
    /// Shows an OS notification. Used for the asynchronous generation, which runs detached from the build and has no
    /// console, no MSBuild and no exit code anybody looks at.
    /// <para>
    /// Everything here is best effort - a machine without a notification daemon, a session without a desktop or a
    /// blocked shell must never turn into a second failure on top of the one being reported.
    /// </para>
    /// </summary>
    public static class DesktopNotification
    {
        public static void ShowError(string title, string message)
        {
            try
            {
                ProcessStartInfo startInfo = Build(title, message);
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                Process.Start(startInfo);
            }
            catch (Exception exception)
            {
                Logger.Trace($"Could not show the desktop notification. {exception.Message}");
            }
        }

        private static ProcessStartInfo Build(string title, string message)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // A balloon tip is shown as a toast on Windows 10 and later and needs nothing but the assemblies
                // that ship with the framework - a real toast needs a registered application user model id.
                string script = "Add-Type -AssemblyName System.Windows.Forms, System.Drawing;"
                                + "$icon = New-Object System.Windows.Forms.NotifyIcon;"
                                + "$icon.Icon = [System.Drawing.SystemIcons]::Error;"
                                + "$icon.Visible = $true;"
                                + $"$icon.ShowBalloonTip(10000, '{Escape(title)}', '{Escape(message)}', 'Error');"
                                + "Start-Sleep -Seconds 10;"
                                + "$icon.Dispose()";
                return new ProcessStartInfo("powershell.exe", $"-NoProfile -NonInteractive -WindowStyle Hidden -Command \"{script}\"");
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return new ProcessStartInfo("osascript", $"-e 'display notification \"{message.Replace("\"", "'")}\" with title \"{title.Replace("\"", "'")}\"'");
            }
            return new ProcessStartInfo("notify-send", $"-u critical \"{title.Replace("\"", "'")}\" \"{message.Replace("\"", "'")}\"");
        }

        /// <summary>
        /// Escapes for a PowerShell single quoted string, where a single quote is doubled and nothing else is special.
        /// </summary>
        private static string Escape(string value)
        {
            return value?.Replace("'", "''").Replace("\r", " ").Replace("\n", " ") ?? string.Empty;
        }
    }
}
