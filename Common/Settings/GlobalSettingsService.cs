﻿using System;
using System.Diagnostics;
using KY.Core;
using KY.Core.DataAccess;
using KY.Generator.Models;
using KY.Generator.Statistics;
using Newtonsoft.Json;

namespace KY.Generator.Settings
{
    public class GlobalSettingsService
    {
        private GlobalSettings cache;
        private readonly string fileName;

        public GlobalSettingsService(IEnvironment environment)
        {
            this.fileName = FileSystem.Combine(environment.ApplicationData, "global.settings.json");
        }

        public GlobalSettings Read()
        {
            if (this.cache == null)
            {
                Stopwatch stopwatch = new();
                stopwatch.Start();
                if (FileSystem.FileExists(this.fileName))
                {
                    this.cache = JsonConvert.DeserializeObject<GlobalSettings>(FileSystem.ReadAllText(this.fileName));
                }
                this.cache ??= new GlobalSettings();
                stopwatch.Stop();
                Logger.Trace($"Global settings read in {stopwatch.ElapsedMilliseconds} ms");
            }
            return this.cache;
        }

        public void Write()
        {
            this.cache.AssertIsNotNull(null, "No data found. Use Read() method first");
            FileSystem.WriteAllText(this.fileName, JsonConvert.SerializeObject(this.cache));
        }

        public void ReadAndWrite(Action<GlobalSettings> action)
        {
            action(this.Read());
            this.Write();
        }
    }
}
