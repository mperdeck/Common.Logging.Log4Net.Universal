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
using Common.Logging;

namespace Common.Logging.Log4Net.Universal
{
    /// <summary>
    /// Serilog factory adapter.
    /// </summary>
    public class Log4NetFactoryAdapter : ILoggerFactoryAdapter
    {
        public ILog GetLogger(Type type)
        {
            return new Log4NetLogger(log4net.LogManager.GetLogger(type));
        }

        public ILog GetLogger(string name)
        {
            return new Log4NetLogger(log4net.LogManager.GetLogger(name));
        }
    }
}

