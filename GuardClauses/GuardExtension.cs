#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuardClauses
{
    public static class GuardExtension
    {
        public static T Null<T>(this IGuardClause guardClauseClause, T input, string parameterName, string? message = null)
        {
            if (input is null)
            {
                if (string.IsNullOrEmpty(message))
                {
                    throw new ArgumentNullException(parameterName);
                }
                throw new ArgumentNullException(message, (Exception?)null);
            }

            return input;
        }

        public static string NullOrEmpty(this IGuardClause guardClause, string input, string parameterName, string? message = null)
        {
            Guard.Against.Null(input, parameterName);
            if (input == string.Empty)
            {
                throw new ArgumentNullException(message ?? $"Input {parameterName} was empty.", parameterName);
            }

            return input;
        }

        public static string NullOrWhiteSpace(this IGuardClause guardClause, string input, string parameterName, string? message = null)
        {
            Guard.Against.NullOrEmpty(input, parameterName);
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException(message ?? $"Input {parameterName} was empty.", parameterName);
            }

            return input;
        }

        public static Guid NullOrEmpty(this IGuardClause guardClause, Guid input, string parameterName, string? message = null)
        {
            Guard.Against.Null(input, parameterName);
            if (input == Guid.Empty)
            {
                throw new ArgumentNullException(message ?? $"Input {parameterName} was empty.", parameterName);
            }

            return input;
        }

        public static int NegativeOrZero(this IGuardClause guardClause, int input, string parameterName, string? message = null)
        {
            return NegativeOrZero<int>(guardClause, input, parameterName, message);
        }

        public static double NegativeOrZero(this IGuardClause guardClause, double input, string parameterName, string? message = null)
        {
            return NegativeOrZero<double>(guardClause, input, parameterName, message);
        }

        public static IEnumerable<T> NullOrEmpty<T>(this IGuardClause guardClause, IEnumerable<T> input, string parameterName, string? message = null)
        {
            Guard.Against.Null(input, parameterName);
            if (!input.Any())
            {
                throw new ArgumentNullException(message ?? $"Required input {parameterName} was empty.", parameterName);
            }

            return input;
        }


        private static T NegativeOrZero<T>(this IGuardClause guardClause, T input, string parameterName, string? message = null)
            where T : struct, IComparable
        {
            if (input.CompareTo(default(T)) <= 0)
            {
                throw new ArgumentNullException(message ?? $"Input {parameterName} cannot be zero or negative.", parameterName);
            }

            return input;
        }
    }
}
