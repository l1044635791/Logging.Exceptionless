using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;

namespace Microsoft.Extensions.Logging.Exceptionless {
    public static class ExceptionlessLoggerFactoryExtensions {
        public static ILoggingBuilder AddExceptionless (this ILoggingBuilder builder, Action<ExceptionlessLoggerOptions> options = null) {
            var option = new ExceptionlessLoggerOptions ();  
            options?.Invoke (option);

            builder.Services.AddSingleton (option);
            builder.Services.AddSingleton<ILoggerProvider, ExceptionlessLoggerProvider> ();
            return builder;
        }

        public static ILoggerFactory AddExceptionless (this ILoggerFactory factory) {
            return AddExceptionless (factory, LogLevel.Information);
        }

        public static ILoggerFactory AddExceptionless (this ILoggerFactory factory, LogLevel minLevel) {
            return AddExceptionless (
                factory,
                o => o.Filter = (_, logLevel) => logLevel >= minLevel);
        }
        
        public static ILoggerFactory AddExceptionless (this ILoggerFactory factory, Action<ExceptionlessLoggerOptions> options = null) {
            var option = new ExceptionlessLoggerOptions ();
            options?.Invoke (option);
            factory.AddProvider (new ExceptionlessLoggerProvider (option));
            return factory;
        }
    }
}