//-----------------------------------------------------------------------
// <copyright file="IEqualizer.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Graph
{
    /// <summary>
    /// A interface that comparse two objects of type T.
    /// </summary>
    /// <typeparam name="T">The type of the objects to compare.</typeparam>
    public interface IEqualizer<T>
    {
        /// <summary>
        /// Compares two objects of type T and returns whether they are equal.
        /// </summary>
        /// <param name="first">The first object to compare.</param>
        /// <param name="second">The second object to compare.</param>
        /// <returns>Whether or not the objects are equal.</returns>
        bool AreEqual(T first, T second);
    }
}
