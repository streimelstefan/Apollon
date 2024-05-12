﻿using Apollon.Lib.DualRules;
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
public class NMRCheckerGenerator : INMRCheckerGenerator
{
    public CheckRule[] GenerateNMRCheckRules(PreprocessedStatement[] preprocessedStatements)
    {
        // Can preprocessedStatements be null? 
        var olonRules = preprocessedStatements.Where(x => x.IsOlonRule);

        var nmrCheckRules = new List<CheckRule>();

        int counter = 1;

        foreach (var olonRule in olonRules)
        {
            nmrCheckRules.AddRange(GenerateRulesForOlonRule(olonRule, counter));
            counter++;
        }

        return nmrCheckRules.ToArray();
    }

    private CheckRule[] GenerateRulesForOlonRule(PreprocessedStatement olonRule, int counterIndex)
    {
        string placeHolderName = "chk";
        var nmrCheckRules = new List<CheckRule>();

        var negator = new DualRuleGenerator();

        var linkingVariables = negator.GetAllVariablesNotInHead(olonRule);
        var ruleProcessed = negator.MoveAtomsFromHeadToBody(olonRule, linkingVariables);
        
        // Not sure if is negative can just be set to false.
        var ruleHead = new Literal(new Atoms.Atom(placeHolderName + counterIndex.ToString() + counterIndex.ToString(), ruleProcessed.Head.Atom.ParamList), true, false);

        // All Body part Rules
        for (int i = 0; i < ruleProcessed.Body.Length; i++)
        {
            var bodyParts = new BodyPart[i + 1];

            for (int j = 0; j < i; j++)
            {
                bodyParts[j] = (BodyPart)ruleProcessed.Body[j].Clone();
            }

            bodyParts[i] = (BodyPart)ruleProcessed.Body[i].Clone();
            negator.SwitchNegation(bodyParts[i]);
            nmrCheckRules.Add(new CheckRule((Literal)ruleHead.Clone(), bodyParts));
        }

        // Move head to Body Rule
        var tmpList = ruleProcessed.Body.ToList();
        tmpList.Add(new BodyPart(olonRule.Head, null));

        nmrCheckRules.Add(new CheckRule(ruleHead, tmpList.ToArray()));

        BodyPart body;
        // Add the overruling Rule
        if (linkingVariables.Count > 0)
        {
            // We need a forall rule here. 
            body = negator.BuildForAllBody(ruleHead, linkingVariables);
            
            //Adds the forall rule itself
            nmrCheckRules.Add(new CheckRule(new Literal(new Atoms.Atom(placeHolderName + counterIndex.ToString() + counterIndex.ToString(), olonRule.Head.Atom.ParamList), true, false), body));
        }

        // That feels so fcking wrong.
        body = new BodyPart(new Literal(new Atoms.Atom(placeHolderName + counterIndex.ToString() + counterIndex.ToString(), nmrCheckRules.Last().Head.Atom.ParamList), true, false), null);

        nmrCheckRules.Add(new CheckRule(new Literal(new Atoms.Atom(placeHolderName + counterIndex.ToString(), nmrCheckRules.Last().Head.Atom.ParamList), true, false), body));
        
        return nmrCheckRules.ToArray();
    }
}