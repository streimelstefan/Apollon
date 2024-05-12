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

        var nmrCheckRules = nmrChecker.GenerateNMRCheckRules(processedRules.Where(x => x.IsOlonRule).ToArray());
        var nmrCheckRulesString = nmrCheckRules.Select(x => x.ToString()).ToArray();

        Assert.AreEqual(8, nmrCheckRules.Length);
        Assert.Contains("not chk22() :- not q().", nmrCheckRulesString);
        Assert.Contains("not chk22() :- q(), not d().", nmrCheckRulesString);
        Assert.Contains("not chk22() :- q(), d(), p().", nmrCheckRulesString);
        Assert.Contains("not chk2() :- not chk22().", nmrCheckRulesString);
        Assert.Contains("not chk11() :- p().", nmrCheckRulesString);
        Assert.Contains("not chk11() :- not p(), q().", nmrCheckRulesString);
        Assert.Contains("not chk1() :- not chk11().", nmrCheckRulesString);
        Assert.AreEqual("nmr_check() :- not chk1(), not chk2().", nmrCheckRulesString[7]);
    }

    [Test]
    public void ShouldGenerateForAll()
    {
        var program = _parser.ParseFromFile("../../../TestPrograms/NMRWithForall.apo");

        var callGraph = new CallGraphBuilder(new LiteralParamCountEqualizer()).BuildCallGraph(program);

        var olonSet = OlonDetector.DetectOlonIn(callGraph);

        var processedRules = new RuleMetadataSetter(callGraph, olonSet).SetMetadataOn(program.RuleTypesAsStatements.ToArray());

        NMRCheckGenerator nmrChecker = new NMRCheckGenerator();

        var nmrCheckRules = nmrChecker.GenerateNMRCheckRules(processedRules.Where(x => x.IsOlonRule).ToArray());
        var nmrCheckRulesString = nmrCheckRules.Select(x => x.ToString()).ToArray();

        Assert.AreEqual(10, nmrCheckRules.Length);
        Assert.Contains("not chk11(X, Z) :- not b(X, Z).", nmrCheckRulesString);
        Assert.Contains("not chk11(X, Z) :- b(X, Z), c(Z, X).", nmrCheckRulesString);
        Assert.Contains("not chk11(X, Z) :- b(X, Z), not c(Z, X), a(X).", nmrCheckRulesString);
        Assert.Contains("not chk11(X) :- forall(Z, not chk11(X, Z)).", nmrCheckRulesString);
        Assert.Contains("not chk1(X) :- not chk11(X).", nmrCheckRulesString);
        Assert.Contains("not chk22(X, V/0) :- V/0 != b().", nmrCheckRulesString);
        Assert.Contains("not chk22(X, V/0) :- V/0 = b(), not a(X).", nmrCheckRulesString);
        Assert.Contains("not chk22(X, V/0) :- V/0 = b(), a(X), c(X, b).", nmrCheckRulesString);
        Assert.Contains("not chk2(X, V/0) :- not chk22(X, V/0).", nmrCheckRulesString);
        Assert.AreEqual("nmr_check() :- forall(X, not chk1(X)), forall(X, forall(V/0, not chk2(X, V/0))).", nmrCheckRulesString[9]);
    }
}
