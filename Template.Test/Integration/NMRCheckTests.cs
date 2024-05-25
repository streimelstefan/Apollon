using Apollon.Lib.Graph;
using Apollon.Lib.NMRCheck;
using Apollon.Lib.OLON;
using Apollon.Lib.Rules;
using AppollonParser;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Test.Integration;

[TestFixture]
public class NMRCheckTests
{
    private ApollonParser _parser = new ApollonParser();

    [Test]
    public void ShouldGenerateNMRRulesBasic()
    {
        var program = _parser.ParseFromFile("../../../TestPrograms/BasicNMR.apo");

        var callGraph = new CallGraphBuilder(new LiteralParamCountEqualizer()).BuildCallGraph(program);

        var olonSet = OlonDetector.DetectOlonIn(callGraph);

        var processedRules = new RuleMetadataSetter(callGraph, olonSet).SetMetadataOn(program.RuleTypesAsStatements.ToArray());

        NMRCheckGenerator nmrChecker = new NMRCheckGenerator();

        var nmrCheckRules = nmrChecker.GenerateNMRCheckRules(processedRules.Where(x => x.IsOlonRule).ToArray(), program);
        var nmrCheckRulesString = nmrCheckRules.Select(x => x.ToString()).ToArray();

        Assert.AreEqual(8, nmrCheckRules.Length);
        Assert.Contains("not _chk22() :- not q().", nmrCheckRulesString);
        Assert.Contains("not _chk22() :- q(), not d().", nmrCheckRulesString);
        Assert.Contains("not _chk22() :- q(), d(), p().", nmrCheckRulesString);
        Assert.Contains("not _chk2() :- not _chk22().", nmrCheckRulesString);
        Assert.Contains("not _chk11() :- p().", nmrCheckRulesString);
        Assert.Contains("not _chk11() :- not p(), q().", nmrCheckRulesString);
        Assert.Contains("not _chk1() :- not _chk11().", nmrCheckRulesString);
        Assert.AreEqual("_nmr_check() :- not _chk1(), not _chk2().", nmrCheckRulesString[7]);
    }

    [Test]
    public void ShouldGenerateForAll()
    {
        var program = _parser.ParseFromFile("../../../TestPrograms/NMRWithForall.apo");

        var callGraph = new CallGraphBuilder(new LiteralParamCountEqualizer()).BuildCallGraph(program);

        var olonSet = OlonDetector.DetectOlonIn(callGraph);

        var processedRules = new RuleMetadataSetter(callGraph, olonSet).SetMetadataOn(program.RuleTypesAsStatements.ToArray());

        NMRCheckGenerator nmrChecker = new NMRCheckGenerator();

        var nmrCheckRules = nmrChecker.GenerateNMRCheckRules(processedRules.Where(x => x.IsOlonRule).ToArray(), program);
        var nmrCheckRulesString = nmrCheckRules.Select(x => x.ToString()).ToArray();

        Assert.AreEqual(10, nmrCheckRules.Length);
        Assert.Contains("not _chk11(X, Z) :- not b(X, Z).", nmrCheckRulesString);
        Assert.Contains("not _chk11(X, Z) :- b(X, Z), c(Z, X).", nmrCheckRulesString);
        Assert.Contains("not _chk11(X, Z) :- b(X, Z), not c(Z, X), a(X).", nmrCheckRulesString);
        Assert.Contains("not _chk11(X) :- forall(Z, not _chk11(X, Z)).", nmrCheckRulesString);
        Assert.Contains("not _chk1(X) :- not _chk11(X).", nmrCheckRulesString);
        Assert.Contains("not _chk22(X, V/0) :- V/0 != b().", nmrCheckRulesString);
        Assert.Contains("not _chk22(X, V/0) :- V/0 = b(), not a(X).", nmrCheckRulesString);
        Assert.Contains("not _chk22(X, V/0) :- V/0 = b(), a(X), c(X, b).", nmrCheckRulesString);
        Assert.Contains("not _chk2(X, V/0) :- not _chk22(X, V/0).", nmrCheckRulesString);
        Assert.AreEqual("_nmr_check() :- forall(X, not _chk1(X)), forall(X, forall(V/0, not _chk2(X, V/0))).", nmrCheckRulesString[9]);
    }

    [Test]
    public void ShouldGenerateCorrectConstraintRules()
    {
        var program = _parser.ParseFromFile("../../../TestPrograms/OLONRuleByConstraintRule.apo");

        var callGraph = new CallGraphBuilder(new LiteralParamCountEqualizer()).BuildCallGraph(program);

        var olonSet = OlonDetector.DetectOlonIn(callGraph);

        var processedRules = new RuleMetadataSetter(callGraph, olonSet).SetMetadataOn(program.RuleTypesAsStatements.ToArray());

        NMRCheckGenerator nmrChecker = new NMRCheckGenerator();

        var nmrCheckRules = nmrChecker.GenerateNMRCheckRules(processedRules.Where(x => x.IsOlonRule).ToArray(), program);
        var nmrCheckRulesString = nmrCheckRules.Select(x => x.ToString()).ToArray();

        Assert.AreEqual(3, nmrCheckRules.Length);
        Assert.Contains("not _chk11() :- not b().", nmrCheckRulesString);
        Assert.Contains("not _chk1() :- not _chk11().", nmrCheckRulesString);
        Assert.Contains("_nmr_check() :- not _chk1().", nmrCheckRulesString);
    }

    [Test]
    public void ShouldGenerateCorrectConstraintRules2()
    {
        var program = _parser.ParseFromFile("../../../TestPrograms/NMRRuleWithConstraint.apo");

        var callGraph = new CallGraphBuilder(new LiteralParamCountEqualizer()).BuildCallGraph(program);

        var olonSet = OlonDetector.DetectOlonIn(callGraph);

        var processedRules = new RuleMetadataSetter(callGraph, olonSet).SetMetadataOn(program.RuleTypesAsStatements.ToArray());

        NMRCheckGenerator nmrChecker = new NMRCheckGenerator();

        var nmrCheckRules = nmrChecker.GenerateNMRCheckRules(processedRules.Where(x => x.IsOlonRule).ToArray(), program);
        var nmrCheckRulesString = nmrCheckRules.Select(x => x.ToString()).ToArray();

        Assert.AreEqual(3, nmrCheckRules.Length);
        Assert.Contains("not _chk11() :- not a(3).", nmrCheckRulesString);
        Assert.Contains("not _chk1() :- not _chk11().", nmrCheckRulesString);
        Assert.Contains("_nmr_check() :- not _chk1().", nmrCheckRulesString);
    }

    [Test]
    public void ShouldGenerateCorrectRulesBasedOnNegation()
    {
        var program = _parser.ParseFromFile("../../../TestPrograms/NMRRuleWithNegation.apo");

        var callGraph = new CallGraphBuilder(new LiteralParamCountEqualizer()).BuildCallGraph(program);

        var olonSet = OlonDetector.DetectOlonIn(callGraph);

        var processedRules = new RuleMetadataSetter(callGraph, olonSet).SetMetadataOn(program.RuleTypesAsStatements.ToArray());

        NMRCheckGenerator nmrChecker = new NMRCheckGenerator();

        var nmrCheckRules = nmrChecker.GenerateNMRCheckRules(processedRules.Where(x => x.IsOlonRule).ToArray(), program);
        var nmrCheckRulesString = nmrCheckRules.Select(x => x.ToString()).ToArray();

        Assert.AreEqual(5, nmrCheckRules.Length);
        Assert.Contains("not _chk11(X) :- not -a(X).", nmrCheckRulesString);
        Assert.Contains("not _chk11(X) :- -a(X), not a(X).", nmrCheckRulesString);
        Assert.Contains("not _chk11() :- forall(X, not _chk11(X)).", nmrCheckRulesString);
        Assert.Contains("not _chk1() :- not _chk11().", nmrCheckRulesString);
        Assert.Contains("_nmr_check() :- not _chk1().", nmrCheckRulesString);
    }

    [Test]
    public void ShouldGenerateCorrectRuleBasedOnNegationWithMoreParameters()
    {
        var program = _parser.ParseFromFile("../../../TestPrograms/NMRRuleWithNegationAnd2Parameters.apo");

        var callGraph = new CallGraphBuilder(new LiteralParamCountEqualizer()).BuildCallGraph(program);

        var olonSet = OlonDetector.DetectOlonIn(callGraph);

        var processedRules = new RuleMetadataSetter(callGraph, olonSet).SetMetadataOn(program.RuleTypesAsStatements.ToArray());

        NMRCheckGenerator nmrChecker = new NMRCheckGenerator();

        var nmrCheckRules = nmrChecker.GenerateNMRCheckRules(processedRules.Where(x => x.IsOlonRule).ToArray(), program);
        var nmrCheckRulesString = nmrCheckRules.Select(x => x.ToString()).ToArray();

        Assert.AreEqual(5, nmrCheckRules.Length);
        Assert.Contains("not _chk11(X, Y) :- not -a(X, Y).", nmrCheckRulesString);
        Assert.Contains("not _chk11(X, Y) :- -a(X, Y), not a(X, Y).", nmrCheckRulesString);
        Assert.Contains("not _chk11() :- forall(X, forall(Y, not _chk11(X, Y))).", nmrCheckRulesString);
        Assert.Contains("not _chk1() :- not _chk11().", nmrCheckRulesString);
        Assert.Contains("_nmr_check() :- not _chk1().", nmrCheckRulesString);
    }
}
