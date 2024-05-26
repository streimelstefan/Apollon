using Apollon.Lib.Linker;
using Apollon.Lib.Resolution.CoSLD;
using Apollon.Lib.Rules;
using Apollon.Lib.Unification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution.CallStackAndCHS;

/// <summary>
/// CHS steht für conductive hypothesis set.
/// Es speichert verschiedene Literale, die während des Lösungsverfahrens als wahr angesehen werden.
/// Als Set hat es die Eigenschaft, dass alle Literale, die eingegeben werden, einzigartig sind.Das bedeutet, dass nur neue Literale in das Set hinzugefügt werden.Es gibt somit keine doppelten Literale im Set.Der check ob zwei Literale gleich sind muss Mithilfe von Unification gemacht werden.
/// Anders zu Standard Sets muss bei der CHS die Reihenfolge von Literale geregelt sein. In dieser Hinsicht verhält sich der CHS wie ein Stack.
/// </summary>
public class CHS : ICloneable
{
    public List<Literal> Literals { get; private set; } //List does preserve Order, as written on MSDN List<T> Class.
    private IUnifier Unifier = new ExactUnifier();
    private VariableExtractor variableExtractor = new VariableExtractor();
    private VariableLinker variableLinker = new VariableLinker();

    public bool IsEmpty
    {
        get
        {
            return Literals.Count == 0;
        }
    }

    public CHS()
    {
        Literals = new List<Literal>();
    }

    public CHS(IEnumerable<Literal> literals)
    {
        Literals = new List<Literal>(literals);
    }

    public void Add(Literal literal, SubstitutionGroups subGroups)
    {
        // if (literal.Atom.Name.StartsWith("_"))
        // {
        //     return;
        // }
        if (Literals.Where(l => Unifier.Unify(l, literal).IsSuccess).Any()) // if there is another literal in the chs that can be unified.
        {
            throw new ArgumentException("Literal already in CHS."); // Check is proffiecient, as shown in Tests.
        }

        Literals.Add(literal);

        //this.variableLinker.LinkVariables(new Statement(literal));
        //var literalVariables = this.variableExtractor.ExtractVariablesFrom(literal);
        //
        //// replaces the gotten variable names with the names of their substitution groups.
        //foreach (var variable in literalVariables)
        //{
        //    variable.Value = subGroups.GetSubstitionGroupNameOf(variable);
        //}
    }

    public void AddIfNotExists(Literal literal, SubstitutionGroups subGroups)
    {
        try
        {
            Add(literal, subGroups);
        }
        catch (Exception)
        {
        }
    }

    public void SafeUnion(CHS chs, SubstitutionGroups substitutionGroups)
    {
        foreach (var literal in chs.Literals)
        {
            AddIfNotExists(literal, substitutionGroups);
        }
    }


    public Literal Peek()
    {
        return Literals[Literals.Count - 1] ?? throw new InvalidOperationException("Cannot Peek as CHS is empty!");
    }

    public Literal Pop()
    {
        var literal = Literals[Literals.Count - 1] ?? throw new InvalidOperationException("Cannot Pop as CHS is empty!");
        Literals.RemoveAt(Literals.Count - 1);
        return literal;
    }

    public bool Empty()
    {
        return Literals.Count == 0;
    }

    public IEnumerable<Literal> GetLiterals()
    {
        return Literals;
    }

    public override string ToString()
    {
        return $"{{ ({string.Join("), (", Literals.Select(l => l.ToString()))}) }}";
    }

    public object Clone()
    {
        return new CHS(Literals.Select(l => (Literal)l.Clone()));
    }
}
