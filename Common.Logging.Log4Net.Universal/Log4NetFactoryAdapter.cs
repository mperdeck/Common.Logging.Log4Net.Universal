// Copyright 2017 Mattijs Perdeck
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Common.Logging.Configuration;
using System.IO;

namespace Common.Logging.Log4Net.Universal {
    /// <summary>
    /// Serilog factory adapter.
    /// </summary>
    public class Log4NetFactoryAdapter : ILoggerFactoryAdapter {

        public Log4NetFactoryAdapter() : this(null) {}

        public Log4NetFactoryAdapter(NameValueCollection properties) {
            string configType = ArgUtils.GetValue(properties, "configType", string.Empty).ToUpper();
            string configFile = ArgUtils.GetValue(properties, "configFile", string.Empty);

            // app-relative path?
            if (configFile.StartsWith("~/") || configFile.StartsWith("~\\")) {
                configFile = string.Format("{0}/{1}", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('/', '\\'), configFile.Substring(2));
            }

            if (configType == "FILE" || configType == "FILE-WATCH") {
                if (configFile == string.Empty) {
                    throw new ConfigurationException("Configuration property 'configFile' must be set for log4Net configuration of type 'FILE' or 'FILE-WATCH'.");
                }

                if (!File.Exists(configFile)) {
                    throw new ConfigurationException("log4net configuration file '" + configFile + "' does not exists");
                }
            }

            switch (configType) {
                case "INLINE":
                    log4net.Config.XmlConfigurator.Configure();
                    break;
                case "FILE":
                    log4net.Config.XmlConfigurator.Configure(new FileInfo(configFile));
                    break;
                case "FILE-WATCH":
                    log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(configFile));
                    break;
                case "EXTERNAL":
                    // Log4net will be configured outside of Common.Logging
                default:
                    //log4net.Config.XmlConfigurator.Configure();
                    break;
            }
        }

        public ILog GetLogger(Type type) {
            return new Log4NetLogger(log4net.LogManager.GetLogger(type));
        }

        public ILog GetLogger(string name) {
            return new Log4NetLogger(log4net.LogManager.GetLogger(name));
        }
    }
}

