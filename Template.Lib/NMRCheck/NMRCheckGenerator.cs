using Apollon.Lib.DualRules;
using Apollon.Lib.Graph;
using Apollon.Lib.Resolution;
using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.NMRCheck;

/// <summary>
/// The way i understood it is that we just take all OLON Rules and negate the body once, then stick the head to the body. 
/// So if p :- not q. is an OLON Rule, then we would create these rules:
/// not(nmr_check11) :- q.
/// not(nmr_check11) :- not q, p.
/// not(nmr_check1) :- not(nmr_check11).
/// </summary>
public class NMRCheckGenerator : INMRCheckGenerator
{
    public Statement[] GenerateNMRCheckRules(PreprocessedStatement[] preprocessedStatements, Program program)
    {
        // Can preprocessedStatements be null? 
        var olonRules = preprocessedStatements.Where(x => x.IsOlonRule).ToList();

        var nmrCheckRules = new List<Statement>();
        var generalRules = new List<Statement>();
        int counter = 1;

        //// Generate constraints for negated literals
        //var equalCheck = new LiteralParamCountEqualizer();

        //var seenElements = new List<Literal>();
        //var allLiterals = new List<Literal>(program.AllLiterals.ToList());

        //foreach (var literal in allLiterals)
        //{
        //    var negatedLiteral = (Literal)literal.Clone();
        //    negatedLiteral.IsNegative = !negatedLiteral.IsNegative;

        //    var statement = new PreprocessedStatement(new Statement(null, new BodyPart[] {new BodyPart(literal, null), new BodyPart(negatedLiteral, null)}), false, false);
        //    olonRules.Add(statement);
        //}

        //olonRules.Add(new PreprocessedStatement(new Statement(null, new BodyPart[] { new BodyPart(new Literal(new Atoms.Atom("a", new Atoms.AtomParam[1] { new Atoms.AtomParam(new Literal(new Atoms.Atom("X"), false, false)) }), false, false), null), new BodyPart(new Literal(new Atoms.Atom("a", new Atoms.AtomParam[1] { new Atoms.AtomParam(new Literal(new Atoms.Atom("X"), false, false)) }), false, true), null) }), false, false));

        // Generate for classic OLON Rules
        foreach (var olonRule in olonRules)
        {
            nmrCheckRules.AddRange(GenerateRulesForOlonRule(olonRule, counter));
            counter++;

            generalRules.Add(nmrCheckRules.Last());
        }


        //GenerateRulesForNegation(doubleLiteralList.First(), counter++);

        // Generate the NMR Rule
        var generalRule = GenerateGeneralRule(generalRules);
        nmrCheckRules.Add(generalRule);

        return nmrCheckRules.ToArray();
    }

    private CheckRule[] GenerateRulesForNegation(Literal literal, int counterIndex)
    {
        var nmrCheckRules = new List<CheckRule>();

        var literalClone = (Literal)literal.Clone();
        literalClone.IsNAF = true;
        literalClone.Atom.ParamList = new Atoms.AtomParam[1] { new Atoms.AtomParam(new Literal(new Atoms.Atom("X"), false, false)) };

        var ruleHead = new Literal(new Atoms.Atom("chk" + counterIndex.ToString() + counterIndex.ToString(), new Atoms.AtomParam[] {new Atoms.AtomParam(new Literal(new Atoms.Atom("X"), false, false))}), true, false);
        var ruleBody = new BodyPart[] {new BodyPart(literalClone, null)};

        nmrCheckRules.Add(new CheckRule(ruleHead, ruleBody));

        return nmrCheckRules.ToArray();
    }

    private CheckRule[] GenerateRulesForOlonRule(PreprocessedStatement olonRule, int counterIndex)
    {
        string placeHolderName = "chk";
        var nmrCheckRules = new List<CheckRule>();

        // Since NMR Check rule Generation follows similar rules to OLON Rules, we can reuse some of the functionality.
        var dualRulesFunctions = new DualRuleGenerator();

        List<Term> linkingVariables;
        Statement ruleProcessed;

        // Check whether the rule is a constraint or not and process it accordingly.
        if (olonRule.Head != null)
        {
            linkingVariables = dualRulesFunctions.GetAllVariablesNotInHead(olonRule);
            ruleProcessed = dualRulesFunctions.MoveAtomsFromHeadToBody(olonRule, linkingVariables);
        }
        else
        {
            linkingVariables = new List<Term>();
            ruleProcessed = olonRule;

            var variableExtractor = new VariableExtractor();
            var variables = variableExtractor.ExtractVariablesFrom(ruleProcessed);

            foreach (var variable in variables)
            {
                if (!linkingVariables.Contains(variable))
                {
                    linkingVariables.Add(variable);
                }
            }
        }

        // Not sure if is negative can just be set to false.
        var atomParamList = new List<Atoms.AtomParam>();
        foreach (var variable in linkingVariables)
        {
            atomParamList.Add(new Atoms.AtomParam(new Literal(new Atoms.Atom(variable.Value), false, false)));
        }

        var ruleHead = new Literal(new Atoms.Atom(placeHolderName + counterIndex.ToString() + counterIndex.ToString(), ruleProcessed.Head?.Atom.ParamList ?? atomParamList.ToArray()), true, false);

        // All Body part Rules
        for (int i = 0; i < ruleProcessed.Body.Length; i++)
        {
            var bodyParts = new BodyPart[i + 1];

            for (int j = 0; j < i; j++)
            {
                bodyParts[j] = (BodyPart)ruleProcessed.Body[j].Clone();
            }

            bodyParts[i] = (BodyPart)ruleProcessed.Body[i].Clone();
            dualRulesFunctions.SwitchNegation(bodyParts[i]);
            nmrCheckRules.Add(new CheckRule((Literal)ruleHead, bodyParts));
        }

        if (ruleProcessed.Head != null)
        {
            // Move head to Body Rule
            var tmpList = ruleProcessed.Body.ToList();
            tmpList.Add(new BodyPart(olonRule.Head, null));
            nmrCheckRules.Add(new CheckRule(ruleHead, tmpList.ToArray()));
        }

        BodyPart body;
        // Add the overruling Rule
        if (linkingVariables.Count > 0)
        {
            // We need a forall rule here. 
            body = dualRulesFunctions.BuildForAllBody(ruleHead, linkingVariables);

            // Adds the forall rule itself
            nmrCheckRules.Add(new CheckRule(new Literal(new Atoms.Atom(placeHolderName + counterIndex.ToString() + counterIndex.ToString(), olonRule.Head?.Atom.ParamList), true, false), body));
        }

        var paramList = nmrCheckRules.Last().Head.Atom.ParamList ?? new Atoms.AtomParam[0];

        // That feels so fcking wrong.
        body = new BodyPart(new Literal(new Atoms.Atom(placeHolderName + counterIndex.ToString() + counterIndex.ToString(), paramList), true, false), null);

        nmrCheckRules.Add(new CheckRule(new Literal(new Atoms.Atom(placeHolderName + counterIndex.ToString(), paramList), true, false), body));

        return nmrCheckRules.ToArray();
    }

    private Statement GenerateGeneralRule(List<Statement> rules)
    {
        var bodyParts = new List<BodyPart>();
        var dualRules = new DualRuleGenerator();

        foreach (var rule in rules)
        {
            if (rule.Head.Atom.ParamList.Length == 0)
            {
                bodyParts.Add(new BodyPart(rule.Head, null));
            }
            else
            {
                var body = dualRules.BuildForAllBody(rule.Head, rule.Head.Atom.ParamList.Select(p => p.Term).ToList());
                bodyParts.Add(body);
            }
        }


        return new Statement(new Literal(new Atoms.Atom("nmr_check"), false, false), bodyParts.ToArray());
    }
}
