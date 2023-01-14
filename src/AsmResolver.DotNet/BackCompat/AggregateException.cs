using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AsmResolver.DotNet.BackCompat;

#if NET35
/// <summary>
/// Polyfill for the AggregateException class in .NET 3.5.
/// </summary>
public class AggregateException : Exception
{
    private const string InnerExceptionPrefix = " ---> ";

    private readonly Exception[] _innerExceptions;

    internal AggregateException(string message, IEnumerable<Exception> innerExceptions) : base(message)
    {
        _innerExceptions = innerExceptions.ToArray();
    }

    /// <summary>Gets a message that describes the exception.</summary>
    public override string Message
    {
        get
        {
            if (_innerExceptions.Length == 0)
            {
                return base.Message;
            }

            var sb = new StringBuilder();
            sb.Append(base.Message);
            sb.Append(' ');
            foreach (var t in _innerExceptions)
            {
                sb.Append('(');
                sb.Append(t.Message);
                sb.Append(") ");
            }
            sb.Length--;
            return sb.ToString();
        }
    }

    /// <summary>
    /// Creates and returns a string representation of the current <see cref="AggregateException"/>.
    /// </summary>
    /// <returns>A string representation of the current exception.</returns>
    public override string ToString()
    {
        var text = new StringBuilder();
        text.Append(base.ToString());

        for (int i = 0; i < _innerExceptions.Length; i++)
        {
            if (_innerExceptions[i] == InnerException)
                continue; // Already logged in base.ToString()

            text.Append('\n' + InnerExceptionPrefix);
            text.AppendFormat(CultureInfo.InvariantCulture, "(Inner Exception #{0})", i);
            text.Append(_innerExceptions[i]);
            text.Append("<---");
            text.AppendLine();
        }

        return text.ToString();
    }
}
#endif
