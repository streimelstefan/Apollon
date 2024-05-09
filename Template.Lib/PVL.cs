using Apollon.Lib.Atoms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Apollon.Lib;

/// <summary>
/// Jede Variable wird mit einer PVL (prohibited value list) verbunden. Solange diese Liste leer ist ist die Variable unbound. Sobald sich Werte in dieser Liste befinden ist diese Variable negatively constrained.
/// Eine PVL darf keine Variablen enthalten die selber negatively constrained sind(Also eine selber eine PVL mit Werten haben.
/// Die PVL wie ihr Name vermuten lässt ist eine Liste an Werten die diese Variabel nicht annehmen darf. 
/// </summary>
public class PVL
{
    /// Linking to the variable should happen on the variable side.

    private List<AtomParam> Values { get; set; } // Add NegativalyContrained() method to Term class?

    /// <summary>
    /// Initializes a new instance of the <see cref="PVL"/> class.
    /// </summary>
    public PVL()
    {
        Values = new List<AtomParam>();
    }

    /// <summary>
    /// Gets the values of the PVL.
    /// </summary>
    /// <returns>Returns an IEnumerable containing all Terms.</returns>
    public IEnumerable<AtomParam> GetValues()
    {
        return Values;
    }

    /// <summary>
    /// Adds a Term to the PVL.
    /// </summary>
    /// <param name="value">The Value that should be added to the PVL.</param>
    /// <exception cref="ArgumentException">Is Thrown if Value is already in PVL or is negatively constrained.</exception>
    public void AddValue(AtomParam value)
    {
        if (Values.Contains(value))
        {
            throw new ArgumentException("Value already in PVL.");
        }

        if (value.Term != null && value.Term.IsNegativelyConstrained())
        {
            throw new ArgumentException("Value is negatively constrained and can therefore not be added.");
        }

        Values.Add(value);  
    }

    public static void Union(PVL first, PVL second)
    {
        foreach (var param in first.Values)
        {
            if (second.Values.Where(p => p.Equals(param)).Any()) continue;

            second.Values.Add(param);
        }

        foreach (var param in second.Values)
        {
            if (first.Values.Where(p => p.Equals(param)).Any()) continue;

            first.Values.Add(param);
        }
    }

    /// <summary>
    /// Returns whether the PVL is empty.
    /// </summary>
    /// <returns>A bool representing whether the Values are Empty or not.</returns>
    public bool Empty()
    {
        return Values.Count == 0;
    }
}
