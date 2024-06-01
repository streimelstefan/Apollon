//-----------------------------------------------------------------------
// <copyright file="Maybe.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib
{
    /// <summary>
    /// Represents a maybe type that can either contain a value or an error. Similar to the Maybe type in Haskell.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <typeparam name="TU">The type of the string.</typeparam>
    public class Maybe<T, TU>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Maybe{T, U}"/> class.
        /// </summary>
        /// <param name="value">The value of the maybe type if one exists.</param>
        public Maybe(T value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Maybe{T, U}"/> class.
        /// </summary>
        /// <param name="error">The error of the maybe type if one exists.</param>
        public Maybe(TU error)
        {
            this.Error = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Maybe{T, U}"/> class.
        /// </summary>
        /// <param name="value">The value if one exists.</param>
        /// <param name="error">The error if one exists.</param>
        /// <exception cref="ArgumentException">Is thrown if both value and error are null or not null at the same time.</exception>
        public Maybe(T? value, TU? error)
        {
            if (value == null && error == null)
            {
                throw new ArgumentException("Value and error are not allowed to be null at the same time.");
            }

            if (value != null && error != null)
            {
                throw new ArgumentException("Value and error are not allowed to be set at the same time.");
            }

            this.Value = value;
            this.Error = error;
        }

        /// <summary>
        /// Gets the value of the maybe if one exists.
        /// </summary>
        public T? Value { get; private set; }

        /// <summary>
        /// Gets the error of the maybe if one exists.
        /// </summary>
        public TU? Error { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not the maybe is an error.
        /// </summary>
        public bool IsError => this.Error != null;

        /// <summary>
        /// Gets a value indicating whether or not the maybe is a success.
        /// </summary>
        public bool IsSuccess => this.Value != null;
    }
}
