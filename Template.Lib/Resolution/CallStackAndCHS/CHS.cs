namespace Apollon.Lib.Resolution.CallStackAndCHS;

using Apollon.Lib.Linker;
using Apollon.Lib.Resolution.CoSLD;
using Apollon.Lib.Unification;
using static System.Net.Mime.MediaTypeNames;

/// <summary>
/// The CHS is part of the Co SLD Resolution algorithm. It contains all the literals that can be assumed true at a specific time.
/// </summary>
public class CHS : ICloneable
{
    private IUnifier unifier = new ExactUnifier();
    private VariableExtractor variableExtractor = new VariableExtractor();
    private VariableLinker variableLinker = new VariableLinker();

    /// <summary>
    /// Initializes a new instance of the <see cref="CHS"/> class.
    /// </summary>
    public CHS()
    {
        this.Literals = new List<Literal>();
    }

    /// <summary>
    /// Gets the List of all Literals in the CHS.
    /// </summary>
    public List<Literal> Literals { get; private set; } // List does preserve Order, as written on MSDN List<T> Class.

    /// <summary>
    /// Gets a value indicating whether the CHS is empty.
    /// </summary>
    public bool IsEmpty
    {
        get
        {
            return this.Literals.Count == 0;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CHS"/> class.
    /// </summary>
    /// <param name="literals">An Enumerable of all Literals that should be entered into the CHS.</param>
    public CHS(IEnumerable<Literal> literals)
    {
        this.Literals = new List<Literal>(literals);
    }

    /// <summary>
    /// Adds the given Literal to the CHS.
    /// </summary>
    /// <param name="literal">The Literal that should be added to the list.</param>
    /// <param name="subGroups">All SubstitutionGroups for the current Literal.</param>
    /// <exception cref="ArgumentException">Is thrown when there is another literal in the chs that can be unified.</exception>
    public void Add(Literal literal, SubstitutionGroups subGroups)
    {
        // if (literal.Atom.Name.StartsWith("_"))
        // {
        //     return;
        // }
        if (this.Literals.Where(l => this.unifier.Unify(l, literal).IsSuccess).Any()) // if there is another literal in the chs that can be unified.
        {
            throw new ArgumentException("Literal already in CHS."); // Check is proffiecient, as shown in Tests.
        }

        this.Literals.Add(literal);

        // this.variableLinker.LinkVariables(new Statement(literal));
        // var literalVariables = this.variableExtractor.ExtractVariablesFrom(literal);
        //
        //   replaces the gotten variable names with the names of their substitution groups.
        // foreach (var variable in literalVariables)
        // {
        //    variable.Value = subGroups.GetSubstitionGroupNameOf(variable);
        // }
    }

    /// <summary>
    /// Adds the given Literal to the CHS if it does not already exist.
    /// </summary>
    /// <param name="literal">The Literal that should be added to the list.</param>
    /// <param name="subGroups">All SubstitutionGroups for the current Literal.</param>
    public void AddIfNotExists(Literal literal, SubstitutionGroups subGroups)
    {
        try
        {
            this.Add(literal, subGroups);
        }
        catch (Exception)
        {
        }
    }

    /// <summary>
    /// Adds the given CHS to the current CHS and skipping all Literals that are already in the CHS.
    /// </summary>
    /// <param name="chs">The CHS that should be added.</param>
    /// <param name="substitutionGroups">All SubstitutionGroups for the current CHS.</param>
    public void SafeUnion(CHS chs, SubstitutionGroups substitutionGroups)
    {
        foreach (var literal in chs.Literals)
        {
            this.AddIfNotExists(literal, substitutionGroups);
        }
    }

    /// <summary>
    /// Returns the next item of the CHS.
    /// </summary>
    /// <returns>Returns the next item of the CHS without removing it.</returns>
    /// <exception cref="InvalidOperationException">Is thrown when the CHS is empty.</exception>
    public Literal Peek()
    {
        return this.Literals[this.Literals.Count - 1] ?? throw new InvalidOperationException("Cannot Peek as CHS is empty!");
    }

    /// <summary>
    /// Returns the next item of the CHS.
    /// </summary>
    /// <returns>Returns the next item of the CHS without removing it.</returns>
    /// <exception cref="InvalidOperationException">Is thrown when the CHS is empty.</exception>
    public Literal Pop()
    {
        var literal = this.Literals[this.Literals.Count - 1] ?? throw new InvalidOperationException("Cannot Pop as CHS is empty!");
        this.Literals.RemoveAt(this.Literals.Count - 1);
        return literal;
    }

    /// <summary>
    /// Checks if the CHS is empty.
    /// </summary>
    /// <returns>Returns a boolean representing whether or not the CHS is empty.</returns>
    public bool Empty()
    {
        return this.Literals.Count == 0;
    }

    /// <summary>
    /// Returns all Literals in the CHS.
    /// </summary>
    /// <returns>Returns an IEnumerable containing all Literals.</returns>
    public IEnumerable<Literal> GetLiterals()
    {
        return this.Literals;
    }

    /// <summary>
    /// Converts the CHS to a string.
    /// </summary>
    /// <returns>Returns a string representation of the CHS.</returns>
    public override string ToString()
    {
        return $"{{ ({string.Join("), (", this.Literals.Select(l => l.ToString()))}) }}";
    }

    /// <summary>
    /// Clones the CHS.
    /// </summary>
    /// <returns>Returns a Clone of the CHS.</returns>
    public object Clone()
    {
        return new CHS(this.Literals.Select(l => (Literal)l.Clone()));
    }
}
