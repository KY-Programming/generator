using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Command;
using KY.Generator.Extensions;
using KY.Generator.Models;
using KY.Generator.Settings;
using KY.Generator.Templates;
using Newtonsoft.Json;

namespace KY.Generator.Statistics
{
    public class StatisticsService
    {
        private readonly IEnvironment environment;
        private readonly GlobalSettingsService globalSettingsService;
        public Statistic Data { get; }

        public StatisticsService(IEnvironment environment, GlobalSettingsService globalSettingsService)
        {
            this.environment = environment;
            this.globalSettingsService = globalSettingsService;
            Assembly callingAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
            this.Data = new Statistic { Version = callingAssembly.GetName().Version.ToString() };
            Logger.Added += this.OnLoggerAdded;
        }

        private void OnLoggerAdded(object sender, EventArgs<LogEntry> args)
        {
            if (args.Value.Type != LogType.Trace)
            {
                this.Data.Errors.Add(args.Value.Message);
            }
        }

        public void ProgramStart(DateTime? start = null)
        {
            this.Data.Start = start ?? DateTime.Now;
        }

        public void InitializationEnd()
        {
            this.Data.InitializationEnd = DateTime.Now;
            Logger.Trace($"Initialized {this.Data.InitializedModules} modules in {this.Data.InitializationDuration.Format()}");
        }

        public void RunEnd(Guid outputId, string name)
        {
            this.Data.Id = outputId;
            this.Data.Name = name;
            this.Data.License = this.globalSettingsService.Read().License;
            this.Data.RunEnd = DateTime.Now;
            Logger.Trace($"Executed {this.Data.RanCommands.Count} commands in {this.Data.RunDuration.Format()}");
            CommandStatistic slowestCommand = this.Data.RanCommands.OrderByDescending(x => x.Duration).FirstOrDefault();
            if (slowestCommand != null)
            {
                Logger.Trace($"Slowest command \"{slowestCommand.Command}\" took {slowestCommand.Duration.Format()}");
            }
        }

        public void GenerateEnd(long lines)
        {
            this.Data.OutputLines = lines;
            this.Data.OutputEnd = DateTime.Now;
            Logger.Trace($"Generated {this.Data.OutputLines} lines of code in {this.Data.OutputDuration.Format()}");
        }

        public void ProgramEnd(int filesCount)
        {
            this.Data.GeneratedFiles = filesCount;
            this.Data.End = DateTime.Now;
            Logger.Trace($"Generated {this.Data.GeneratedFiles} files ({this.Data.OutputLines} lines) in {this.Data.Duration.Format()}");
        }

        public Measurement StartMeasurement()
        {
            return new Measurement();
        }

        public void Measure(Measurement measurement, IGeneratorCommand command)
        {
            measurement.Stop();
            this.Data.RanCommands.Add(new CommandStatistic { Command = command.Names.First(), Duration = measurement.Elapsed });
        }

        public void Count(FileTemplate file)
        {
            this.Data.CountFilesByLanguage.AddIfNotExists(file.Options.Language.Name, 0);
            this.Data.CountFilesByLanguage[file.Options.Language.Name]++;
        }

        public Statistic Read(string fileName)
        {
            string filePath = FileSystem.Combine(this.environment.ApplicationData, fileName);
            if (!FileSystem.FileExists(filePath))
            {
                Logger.Trace($"File not found {fileName}...");
                return null;
            }
            Logger.Trace($"Read file {fileName}...");
            return JsonConvert.DeserializeObject<Statistic>(FileSystem.ReadAllText(filePath));
        }

        public string Write()
        {
            string fileName = $"{this.Data.Start:yyyy-MM-dd-hh-mm-ss-fff}-{this.Data.Id}.statistics.json";
            FileSystem.CreateDirectory(this.environment.ApplicationData);
            FileSystem.WriteAllText(this.environment.ApplicationData, fileName, JsonConvert.SerializeObject(this.Data));
            return fileName;
        }

        public void Anonymize(Statistic statistic)
        {
            List<string> anonymousErrors = statistic.Errors.Select(Anonymizer.ReplaceAll).ToList();
            statistic.Errors.Clear();
            statistic.Errors.AddRange(anonymousErrors);
        }

        public void Submit(Statistic statistic)
        {
            try
            {
                // statistic.RanCommands.Clear();
                this.SendCommand("", statistic);
            }
            catch (Exception exception)
            {
                Logger.Warning(exception.Message + Environment.NewLine + exception.StackTrace);
            }
        }

        public void Enable(List<Guid> ids)
        {
            this.globalSettingsService.ReadAndWrite(settings => settings.StatisticsEnabled = true);
            this.SendCommand("enable", ids);
        }

        public void Disable(List<Guid> ids)
        {
            this.globalSettingsService.ReadAndWrite(settings => settings.StatisticsEnabled = false);
            this.SendCommand("disable", ids);
        }

        private void SendCommand(string command, object data)
        {
            byte[] content = data == default ? Array.Empty<byte>() : Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
#if DEBUG
            string baseUri = "http://localhost:8003/api/v1/statistics";
#else
            string baseUri = "https://generator.ky-programming.de/api/v1/statistics";
#endif
            HttpWebRequest request = WebRequest.CreateHttp($"{baseUri}/{command}");
            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "application/json";
            request.ContentLength = content.Length;
            using Stream stream = request.GetRequestStream();
            stream.Write(content);
                request.GetResponse();
            }

        public void Cleanup()
        {
            string[] files = FileSystem.GetFiles(this.environment.ApplicationData, "*.statistics.json");
            foreach (string file in files)
            {
                FileSystem.DeleteFile(file);
            }
        }
    }
}
