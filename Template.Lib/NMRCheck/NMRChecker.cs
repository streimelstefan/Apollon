using Apollon.Lib.DualRules;
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
public class NMRChecker : INMRChecker
{
    public CheckRule[] GenerateNMRCheckRules(PreprocessedStatement[] preprocessedStatements)
    {
        // Can preprocessedStatements be null? 
        var olonRules = preprocessedStatements.Where(x => x.IsOlonRule);

        var nmrCheckRules = new List<CheckRule>();

        foreach (var olonRule in olonRules)
        {
            nmrCheckRules.AddRange(GenerateRulesForOlonRule(olonRule, 1));
        }

        return nmrCheckRules.ToArray();
    }

    private CheckRule[] GenerateRulesForOlonRule(PreprocessedStatement olonRule, int counterIndex)
    {
        string placeHolderName = "nmr_check";
        var nmrCheckRules = new List<CheckRule>();
        // Not sure if is negative can just be set to false.
        var ruleHead = new Literal(new Atoms.Atom(placeHolderName + counterIndex.ToString() + counterIndex.ToString()), true, false);

        var negator = new DualRuleGenerator();

        var tmp = negator.GetAllVariablesNotInHead(olonRule);
        var ruleProcessed = negator.MoveAtomsFromHeadToBody(olonRule, tmp);

        // All Body parts
        for (int i = 0; i < olonRule.Body.Length; i++)
        {
            var bodyParts = new BodyPart[i + 1];

            for (int j = 0; j < i; j++)
            {
                bodyParts[j] = (BodyPart)olonRule.Body[j].Clone();
            }

            bodyParts[i] = (BodyPart)olonRule.Body[i].Clone();
            negator.SwitchNegation(bodyParts[i]);
            nmrCheckRules.Add(new CheckRule((Literal)ruleHead.Clone(), bodyParts));
        }

        // Move head to Body
        var tmpList = olonRule.Body.ToList();
        tmpList.Add(new BodyPart(olonRule.Head, null));

        nmrCheckRules.Add(new CheckRule(ruleHead, tmpList.ToArray()));

        // Add the overruling Rule
        nmrCheckRules.Add(new CheckRule(new Literal(new Atoms.Atom(placeHolderName + counterIndex.ToString()), true, false), new BodyPart(new Literal(new Atoms.Atom("nmr_check" + counterIndex.ToString() + counterIndex.ToString()), true, false), null)));
        
        return nmrCheckRules.ToArray();
    }
}
