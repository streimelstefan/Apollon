namespace Apollon.Lib;
using Apollon.Lib.Atoms;

/// <summary>
/// Jede Variable wird mit einer PVL (prohibited value list) verbunden. Solange diese Liste leer ist ist die Variable unbound. Sobald sich Werte in dieser Liste befinden ist diese Variable negatively constrained.
/// Eine PVL darf keine Variablen enthalten die selber negatively constrained sind(Also eine selber eine PVL mit Werten haben.
/// Die PVL wie ihr Name vermuten lässt ist eine Liste an Werten die diese Variable nicht annehmen darf.
/// </summary>
public class PVL : IEquatable<PVL>, ICloneable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PVL"/> class.
    /// </summary>
    public PVL()
    {
        this.Values = new List<AtomParam>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PVL"/> class.
    /// </summary>
    /// <param name="atomParams">An enumerable of atomParams.</param>
    public PVL(IEnumerable<AtomParam> atomParams)
    {
        this.Values = new List<AtomParam>(atomParams);
    }

    // Linking to the variable should happen on the variable side.
    private List<AtomParam> Values { get; set; } // Add NegativalyContrained() method to Term class?

    /// <summary>
    /// Unions two PVLs.
    /// </summary>
    /// <param name="first">the first PVL.</param>
    /// <param name="second">The second PVL.</param>
    public static void Union(PVL first, PVL second)
    {
        foreach (var param in first.Values)
        {
            if (second.Values.Where(p => p.Equals(param)).Any())
            {
                continue;
            }

            second.Values.Add(param);
        }

        foreach (var param in second.Values)
        {
            if (first.Values.Where(p => p.Equals(param)).Any())
            {
                continue;
            }

            first.Values.Add(param);
        }
    }

    /// <summary>
    /// Gets the values of the PVL.
    /// </summary>
    /// <returns>Returns an IEnumerable containing all Terms.</returns>
    public IEnumerable<AtomParam> GetValues()
    {
        return this.Values;
    }

    /// <summary>
    /// Adds a Term to the PVL.
    /// </summary>
    /// <param name="value">The Value that should be added to the PVL.</param>
    /// <exception cref="ArgumentException">Is Thrown if Value is already in PVL or is negatively constrained.</exception>
    public void AddValue(AtomParam value)
    {
        var valueToUse = (AtomParam)value.Clone();
        valueToUse.ConvertToTermIfPossible();
        if (this.Values.Contains(valueToUse))
        {
            throw new ArgumentException("Value already in PVL.");
        }

        if (valueToUse.Term != null && valueToUse.Term.IsNegativelyConstrained())
        {
            throw new ArgumentException("Value is negatively constrained and can therefore not be added.");
        }

        this.Values.Add(valueToUse);
    }

    /// <summary>
    /// Returns whether the PVL is empty.
    /// </summary>
    /// <returns>A bool representing whether the Values are Empty or not.</returns>
    public bool Empty()
    {
        return this.Values.Count == 0;
    }

    /// <summary>
    /// Clones the PVL.
    /// </summary>
    /// <returns>An object consisting of the cloned PVL.</returns>
    public object Clone()
    {
        return new PVL(this.Values.Select(p => new AtomParam(p.Literal, p.Term)));
    }

    /// <summary>
    /// Clears the PVL.
    /// </summary>
    public void Clear()
    {
        this.Values.Clear();
    }

    /// <summary>
    /// Converts the string to a PVL.
    /// </summary>
    /// <returns>A string representing the current state of the PVL.</returns>
    public override string ToString()
    {
        return $"\\{string.Join(" \\", this.Values.Select(p => p.ToString()))}";
    }

    /// <summary>
    /// Determines whether the PVL is equal to another PVL.
    /// </summary>
    /// <param name="other">The PVL that should be checked equality for.</param>
    /// <returns>A boolean determining whether the PVL is equal to another PVL.</returns>
    public bool Equals(PVL? other)
    {
        if (other == null)
        {
            return false;
        }

        if (other.Values.Count() != this.Values.Count())
        {
            return false;
        }

        for (int i = 0; i < this.Values.Count; i++)
        {
            if (!this.Values[i].Equals(other.Values[i]))
            {
                return false;
            }
        }

        return true;
    }
}
