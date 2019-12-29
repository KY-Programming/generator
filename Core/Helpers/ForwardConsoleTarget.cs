using System;
using KY.Core;

namespace KY.Generator
{
    internal class ForwardConsoleTarget : LogTarget
    {
        public override void Write(LogEntry entry)
        {
            if (!Logger.Console.IsConsoleAvailable)
            {
                return;
            }
            string formattedMessage;
            if (entry.Type == LogType.Error)
            {
                formattedMessage = $">> {entry.CustomType} occurred. {entry.Message}";
            }
            else
            {
                formattedMessage = $">> {entry.Message}";
            }
            Console.WriteLine(formattedMessage);
        }
    }
}