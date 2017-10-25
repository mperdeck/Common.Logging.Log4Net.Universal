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
using Common.Logging.Factory;

namespace Common.Logging.Log4Net.Universal
{
    public class Log4NetLogger : AbstractLogger
    {
        private readonly log4net.ILog _logger;

        public Log4NetLogger(log4net.ILog logger)
        {
            this._logger = logger;
        }

        public override bool IsDebugEnabled
        {
            get
            {
                return this._logger.IsDebugEnabled;
            }
        }

        public override bool IsErrorEnabled
        {
            get
            {
                return this._logger.IsErrorEnabled;
            }
        }

        public override bool IsFatalEnabled
        {
            get
            {
                return this._logger.IsFatalEnabled;
            }
        }

        public override bool IsInfoEnabled
        {
            get
            {
                return this._logger.IsInfoEnabled;
            }
        }

        // Note that log4net ILog interface doesn't have a Trace level
        public override bool IsTraceEnabled
        {
            get
            {
                return this._logger.IsDebugEnabled;
            }
        }

        public override bool IsWarnEnabled
        {
            get
            {
                return this._logger.IsWarnEnabled;
            }
        }

        /// <summary> Actually sends the <paramref name="message" /> to the underlying log system. </summary>
        /// <param name="level"> the level of this log event. </param>
        /// <param name="message"> the message to log. </param>
        /// <param name="exception"> the exception to log (may be null) </param>
        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {
            switch (level)
            {
                // Note that log4net ILog interface doesn't have a Trace level
                case LogLevel.Trace:
                    _logger.Debug(message, exception);
                    break;
                case LogLevel.Debug:
                    _logger.Debug(message, exception);
                    break;
                case LogLevel.Info:
                    _logger.Info(message, exception);
                    break;
                case LogLevel.Warn:
                    _logger.Warn(message, exception);
                    break;
                case LogLevel.Error:
                    _logger.Error(message, exception);
                    break;
                case LogLevel.Fatal:
                    _logger.Fatal(message, exception);
                    break;
                default:
                    _logger.Fatal(message, exception);
                    break;
            }
        }

        /// <summary>
        /// Returns the thread-specific context for variables
        /// </summary>
        public override IVariablesContext ThreadVariablesContext
        {
            get { return new Log4NetThreadVariablesContext(); }
        }

        /// <summary>
        /// Returns the global context for variables
        /// </summary>
        public override IVariablesContext GlobalVariablesContext
        {
            get { return new Log4NetGlobalVariablesContext(); }
        }
    }
}

