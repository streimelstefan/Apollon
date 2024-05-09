using Apollon.Lib.Unification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution;

/// <summary>
/// CHS steht für conductive hypothesis set.
/// Es speichert verschiedene Literale, die während des Lösungsverfahrens als wahr angesehen werden.
/// Als Set hat es die Eigenschaft, dass alle Literale, die eingegeben werden, einzigartig sind.Das bedeutet, dass nur neue Literale in das Set hinzugefügt werden.Es gibt somit keine doppelten Literale im Set.Der check ob zwei Literale gleich sind muss Mithilfe von Unification gemacht werden.
/// Anders zu Standard Sets muss bei der CHS die Reihenfolge von Literale geregelt sein. In dieser Hinsicht verhält sich der CHS wie ein Stack.
/// </summary>
public class CHS
{
    public List<Literal> Literals { get; private set; } //List does preserve Order, as written on MSDN List<T> Class.
    private IUnifier Unifier = new Unifier();

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

    public void Add(Literal literal)
    {
        if (Literals.Where(l => Unifier.Unify(l, literal).IsSuccess).Any()) // if there is another literal in the chs that can be unified.
        {
            throw new ArgumentException("Literal already in CHS."); // Check is proffiecient, as shown in Tests.
        }

        Literals.Add(literal);
    }

    public void AddIfNotExists(Literal literal)
    {
        try
        {
            Add(literal);
        }
        catch (Exception)
        {
        }
    }

    public void SafeUnion(CHS chs)
    {
        foreach (var literal in chs.Literals)
        {
            AddIfNotExists(literal);
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
}
