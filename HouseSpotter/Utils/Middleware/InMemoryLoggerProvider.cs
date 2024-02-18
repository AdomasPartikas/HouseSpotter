using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

public class InMemoryLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentQueue<string> _logMessages = new ConcurrentQueue<string>();

    public ILogger CreateLogger(string categoryName)
    {
        return new InMemoryLogger(_logMessages);
    }

    public void Dispose() { }

    public ConcurrentQueue<string> LogMessages => _logMessages;
}

public class InMemoryLogger : ILogger
{
    private readonly ConcurrentQueue<string> _logMessages;

    public InMemoryLogger(ConcurrentQueue<string> logMessages)
    {
        _logMessages = logMessages;
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        _logMessages.Enqueue(formatter(state, exception));
    }
}