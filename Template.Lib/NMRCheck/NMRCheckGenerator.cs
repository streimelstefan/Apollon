﻿//-----------------------------------------------------------------------
// <copyright file="NMRCheckGenerator.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.NMRCheck;
using Apollon.Lib.Atoms;
using Apollon.Lib.DualRules;
using Apollon.Lib.Graph;
using Apollon.Lib.Resolution;
using Apollon.Lib.Rules;

/// <summary>
/// The way i understood it is that we just take all OLON Rules and negate the body once, then stick the head to the body.
/// So if p :- not q. is an OLON Rule, then we would create these rules:
/// not(nmr_check11) :- q.
/// not(nmr_check11) :- not q, p.
/// not(nmr_check1) :- not(nmr_check11).
/// </summary>
public class NMRCheckGenerator : INMRCheckGenerator
{
    /// <summary>
    /// Generates the NMRCheck rules for the given program.
    /// </summary>
    /// <param name="preprocessedStatements">The preprocessed statments that should be used to create the rules.</param>
    /// <param name="program">The program from which the statements originated from.</param>
    /// <returns>The check rules that were generated.</returns>
    public Statement[] GenerateNMRCheckRules(PreprocessedStatement[] preprocessedStatements, Program program)
    {
        // Can preprocessedStatements be null?
        List<PreprocessedStatement> olonRules = preprocessedStatements.Where(x => x.IsOlonRule).OrderBy(s => s.Head?.Atom.Name ?? "").OrderBy(s => s.Head != null).ToList();

        List<Statement> nmrCheckRules = new();
        List<Statement> generalRules = new();
        int counter = 1;

        // Generate for classic OLON Rules
        foreach (PreprocessedStatement? olonRule in olonRules)
        {
            nmrCheckRules.AddRange(this.GenerateRulesForOlonRule(olonRule, counter));
            counter++;

            generalRules.Add(nmrCheckRules.Last());
        }

        // Generate constraints for negated literals
        LiteralParamCountEqualizer equalCheck = new();

        List<Literal> seenElements = new();
        List<Literal> allLiterals = new(program.AllLiterals.ToList());

        foreach (Literal literal in allLiterals)
        {
            Literal literalClone = (Literal)literal.Clone();
            literalClone.IsNegative = !literalClone.IsNegative;

            if (seenElements.Contains(literal) || seenElements.Contains(literalClone))
            {
                continue;
            }

            // Element has not been seen yet
            Literal? duplicateLiteral = allLiterals.FirstOrDefault(x => equalCheck.AreEqual(x, literalClone));

            if (duplicateLiteral != null && seenElements.FirstOrDefault(x => x.Atom.Name == duplicateLiteral.Atom.Name) == null)
            {
                nmrCheckRules.AddRange(this.GenerateRulesForNegation(literal, counter));
                generalRules.Add(nmrCheckRules.Last());
            }

            counter++;
            seenElements.Add(literal);
        }

        // Generate the NMR Rule
        Statement generalRule = this.GenerateGeneralRule(generalRules);
        nmrCheckRules.Add(generalRule);

        return nmrCheckRules.ToArray();
    }

    private CheckRule[] GenerateRulesForNegation(Literal literal, int counterIndex)
    {
        string placeHolderName = "_chk";

        List<CheckRule> nmrCheckRules = new();

        AtomParam[] literalParams = literal.Atom.ParamList;

        DualRuleGenerator dualRulesFunctions = new();

        // Create RuleHead that is used for the NMR Check Rule; Does still have the same parameters as the original literal.
        Literal ruleHead = new(new Atoms.Atom(placeHolderName + counterIndex.ToString() + counterIndex.ToString(), literalParams), true, false);

        // Body for the first check rule; Is NAF negated and normal negated.
        BodyPart[] ruleBody = new BodyPart[] { new(new Literal(literal.Atom, true, true), null) };

        nmrCheckRules.Add(new CheckRule(ruleHead, ruleBody));

        // Body for the second check rule; First part is normal negated, second part is NAF negated.
        ruleBody = new BodyPart[] { new(new Literal(literal.Atom, false, true), null), new(new Literal(literal.Atom, true, false), null) };

        nmrCheckRules.Add(new CheckRule(ruleHead, ruleBody));

        Literal ruleHeadWithoutParams = new(new Atoms.Atom(placeHolderName + counterIndex.ToString() + counterIndex.ToString(), new Atoms.AtomParam[0]), true, false);

        List<Term> forallVariables = new();

        foreach (AtomParam param in literalParams)
        {
            if (param.Term == null)
            {
                continue;
            }

            forallVariables.Add(param.Term);
        }

        BodyPart bodyPart = dualRulesFunctions.BuildForAllBody(ruleHead, forallVariables);

        ruleBody = new BodyPart[] { bodyPart };

        nmrCheckRules.Add(new CheckRule(ruleHeadWithoutParams, ruleBody));

        bodyPart = new BodyPart(new Literal(new Atoms.Atom(placeHolderName + counterIndex.ToString() + counterIndex.ToString(), new Atoms.AtomParam[0]), true, false), null);

        nmrCheckRules.Add(new CheckRule(new Literal(new Atoms.Atom(placeHolderName + counterIndex.ToString(), new Atoms.AtomParam[0]), true, false), bodyPart));

        return nmrCheckRules.ToArray();
    }

    private CheckRule[] GenerateRulesForOlonRule(PreprocessedStatement olonRule, int counterIndex)
    {
        string placeHolderName = "_chk";
        List<CheckRule> nmrCheckRules = new();

        // Since NMR Check rule Generation follows similar rules to OLON Rules, we can reuse some of the functionality.
        DualRuleGenerator dualRulesFunctions = new();

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

            VariableExtractor variableExtractor = new();
            HashSet<Term> variables = variableExtractor.ExtractVariablesFrom(ruleProcessed);

            foreach (Term variable in variables)
            {
                if (!linkingVariables.Contains(variable))
                {
                    linkingVariables.Add(variable);
                }
            }
        }

        // Not sure if is negative can just be set to false.
        List<AtomParam> atomParamList = new();
        foreach (Term variable in linkingVariables)
        {
            atomParamList.Add(new AtomParam(variable));
        }

        Literal ruleHead = new(new Atoms.Atom(placeHolderName + counterIndex.ToString() + counterIndex.ToString(), ruleProcessed.Head?.Atom.ParamList ?? atomParamList.ToArray()), true, false);

        // All Body part Rules
        for (int i = 0; i < ruleProcessed.Body.Length; i++)
        {
            BodyPart[] bodyParts = new BodyPart[i + 1];

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
            List<BodyPart> tmpList = ruleProcessed.Body.ToList();
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
            nmrCheckRules.Add(
                new CheckRule(
                    new Literal(
                        new Atom(
                            placeHolderName + counterIndex.ToString() + counterIndex.ToString(),
                            olonRule.Head?.Atom.ParamList ?? new AtomParam[0]),
                        true,
                        false),
                    body));
        }

        AtomParam[] paramList = nmrCheckRules.Last().Head!.Atom.ParamList ?? new Atoms.AtomParam[0];

        // That feels so fcking wrong.
        body = new BodyPart(new Literal(new Atoms.Atom(placeHolderName + counterIndex.ToString() + counterIndex.ToString(), paramList), true, false), null);

        nmrCheckRules.Add(new CheckRule(new Literal(new Atoms.Atom(placeHolderName + counterIndex.ToString(), paramList), true, false), body));

        return nmrCheckRules.ToArray();
    }

    private Statement GenerateGeneralRule(List<Statement> rules)
    {
        List<BodyPart> bodyParts = new();
        DualRuleGenerator dualRules = new();

        foreach (Statement rule in rules)
        {
            if (rule.Head!.Atom.ParamList.Length == 0)
            {
                bodyParts.Add(new BodyPart(rule.Head, null));
            }
            else
            {
                BodyPart body = dualRules
                    .BuildForAllBody(rule.Head, rule.Head.Atom.ParamList.Select(p => p.Term).Where(t => t != null).Cast<Term>().ToList());
                bodyParts.Add(body);
            }
        }

        return new Statement(new Literal(new Atoms.Atom("_nmr_check"), false, false), bodyParts.ToArray());
    }
}
