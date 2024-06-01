//-----------------------------------------------------------------------
// <copyright file="DualRuleGenerator.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.DualRules
{
    using Apollon.Lib.Atoms;
    using Apollon.Lib.Docu;
    using Apollon.Lib.Graph;
    using Apollon.Lib.Resolution;
    using Apollon.Lib.Rules;
    using Apollon.Lib.Rules.Operations;
    using Apollon.Lib.Unification;

    /// <summary>
    /// Generates dual rules from a given set of statements.
    /// </summary>
    public class DualRuleGenerator : IDualRuleGenerator
    {
        private readonly OperationNAFSwitcher operationNAFSwitcher = new();

        /// <summary>
        /// Generates dual rules for all the statements that are contained in the statement given.
        /// </summary>
        /// <example>
        /// Generally dual rules are just the same rules with naf negation flipped for each literal once.
        /// Dual Rules are the only statments that allow NAF in the head literal.
        /// If the statement head contains a term (lower case string or number) the term
        /// gets disunified.
        /// a(0). -> not a(X) :- X != 0.
        /// a(b). -> not a(X) :- X != b.
        ///
        /// Statements with mutliple body parts get split up by into as many rules as there are
        /// body with the same name and parameter count.
        /// p(X) :- q(X), -s(X). ->
        /// not p(X) :- not q(X).
        /// not p(X) :- q(X). not -s(X). // notice that the first body part persists without the NAF turnaround. And that clasical negation persists as well.
        ///
        /// if there are multiple statements with the same head structure, their dual rules get created
        /// normally and an overarching rule gets created to combine all of those.
        /// p(1).
        /// p(2).
        /// ->
        /// not p(X) :- not p1(X), not p2(X). // This is the overarching rule anding all the other rules.
        /// not p1(X) :- X != 1. // these rules get numbered through. Here we need to look out for naming conflicts.
        /// not p2(X) :- X != 2.
        ///
        /// if there are statements that contain variables and non variables in the statement head the rules 1 and two written here are applied sequentially.
        /// p(X, b) :- q(X).
        /// ->
        /// not p(X, V0) :- not q(X). // notice that the second parameter was switched to a variable
        /// not p(X, V0) :- q(X), V0 != b.
        ///
        /// Statments containing linking variables insider their bodies only (Variables that contain two parts of the body) are connected using the forall
        /// function.
        /// TODO: Add forall support to the satement class
        /// a(X) :- q(Y), s(X, Y). // Here Y is the linking Variable. It is also only in the body so the forall syntax needs to be used.
        /// ->
        /// not a(X) :- forall(Y, a(X, Y)). // forall has been added for the internal variable Y.
        /// not a(X, Y) :- not q(Y). // notice that the Y variable from the body gets added to the head as well.
        /// not a(X, Y) :- q(Y), not s(X, Y).
        ///
        /// If there are parts of the rule that are connected by a linking variable but also contain other parameters. The other parameters get treated as always while
        /// the linking variable gets the same forall treatment as seen in the example below:
        /// a(X) :- q(Y), s(X, Y, b). // Here again Y is the linking variable. But now s contains an atom b aswell.
        /// ->
        /// not a(X) :- forall(Y, a(X, y).
        /// not a(X, Y) :- not q(Y).
        /// not a(X, Y) :- q(Y), not s(X, Y, b).
        /// </example>
        /// <param name="statements">The statements for which dual rules should be created for.</param>
        /// <returns>The dual rule that have been created.</returns>
        public DualRule[] GenerateDualRules(Statement[] statements)
        {
            // group statements based on their heads and the param count in their heads
            List<StatementGroup> groups = this.GroupStatmentsBasedOnHeadParamCount(statements);
            List<DualRule> dualRules = new();

            foreach (StatementGroup group in groups)
            {
                dualRules.AddRange(this.GenerateDualRulesForGroup(group));
            }

            var allStatements = statements
                    .Union(dualRules).ToArray();

            var missingLiterals = this.AddAllMissingNAFLiterals(allStatements);
            dualRules.AddRange(missingLiterals);

            return dualRules.ToArray();
        }

        /// <summary>
        /// Builds the body of an forall dual rule.
        /// </summary>
        /// <param name="currentHead">The head of the dual rule.</param>
        /// <param name="linkingVariabels">The variables still to build foralls for.</param>
        /// <param name="currentIndex">The current index of the linking variables. Should always be zero when first executing the function.</param>
        /// <returns>The body of the forall dual rule.</returns>
        public BodyPart BuildForAllBody(Literal currentHead, List<Term> linkingVariabels, int currentIndex = 0)
        {
            // if we are at the last index
            return currentIndex == linkingVariabels.Count() - 1
                ? new BodyPart(linkingVariabels.ElementAt(currentIndex), currentHead)
                : new BodyPart(linkingVariabels.ElementAt(currentIndex), this.BuildForAllBody(currentHead, linkingVariabels, ++currentIndex));
        }

        /// <summary>
        /// Moves the atoms from the head to the body of the statement.
        /// </summary>
        /// <param name="statement">The statment for which to move the variables to the body.</param>
        /// <param name="linkingVariables">ALl the variables that need to be added to the head.</param>
        /// <returns>The statement where the atoms where added from the head to the body.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the head is null.</exception>
        /// <exception cref="InvalidOperationException">Is thrown if param in the head is neither a literal nor an term.</exception>
        public Statement MoveAtomsFromHeadToBody(Statement statement, IEnumerable<Term> linkingVariables)
        {
            if (statement.Head == null)
            {
                throw new ArgumentNullException(nameof(statement), "Head of statement is not allowed to be null.");
            }

            List<BodyPart> body = new();
            List<AtomParam> head = new();

            int newVariableIndex = 0;
            foreach (AtomParam param in statement.Head.Atom.ParamList)
            {
                if (param.Term == null || (param.Term != null && !param.Term.IsVariable))
                {
                    // move value to body and replace it with a Variable.
                    Term variable = new($"V/{newVariableIndex}");
                    newVariableIndex++;

                    Literal condtion = param.Term != null
                        ? new Literal(new Atom(param.Term.Value), false, false)
                        : param.Literal != null ? (Literal)param.Literal.Clone() : throw new InvalidOperationException();
                    Operation operation = new(new AtomParam(variable), Operator.Equals, new AtomParam(condtion));
                    head.Add(new AtomParam(null, variable));
                    body.Add(new BodyPart(null, operation));
                }
                else
                {
                    head.Add(param);
                }
            }

            body.AddRange(statement.Body);
            head.AddRange(linkingVariables.Select(t => new AtomParam(t)));

            return new Statement(new Literal(new Atom(statement.Head.Atom.Name, head.ToArray()), statement.Head.IsNAF, statement.Head.IsNegative), body.ToArray());
        }

        /// <summary>
        /// Switches the NAF negation of a body part.
        /// </summary>
        /// <param name="bodyPart">The body part to switch the NAF for.</param>
        public void SwitchNegation(BodyPart bodyPart)
        {
            // Maybe make this function into a whole class.
            if (bodyPart.Operation != null)
            {
                this.operationNAFSwitcher.SwitchNaf(bodyPart.Operation);
            }
            else if (bodyPart.Literal != null)
            {
                bodyPart.Literal.IsNAF = !bodyPart.Literal.IsNAF;
            }
        }

        /// <summary>
        /// Returns all the variables that are in the body but are not used in the head, in order.
        /// Variables are only returned if they exsist in more then one body part.
        /// </summary>
        /// <param name="statement">The statement for which the variables that are not in the head should be extracted.</param>
        /// <returns>All the variables that are not in the head.</returns>
        public List<Term> GetAllVariablesNotInHead(Statement statement)
        {
            VariableExtractor variableExtractor = new();

            HashSet<string> statementVariabels = variableExtractor.ExtractVariablesFrom(statement).Select(t => t.Value).ToHashSet();
            HashSet<string> headVariables = variableExtractor.ExtractVariablesFrom(statement.Head!).Select(t => t.Value).ToHashSet();

            List<Term> except = new();

            foreach (string? term in statementVariabels)
            {
                if (!headVariables.Contains(term))
                {
                    except.Add(new Term(term));
                }
            }

            return except;
        }

        private List<StatementGroup> GroupStatmentsBasedOnHeadParamCount(IEnumerable<Statement> statements)
        {
            // equilizes literals based on their name and param count
            LiteralParamCountEqualizer equilizer = new();
            List<StatementGroup> groups = new();

            foreach (Statement statement in statements)
            {
                // ignore statements without heads (constraints) as they are not taken into consideration
                // while generating dual rules.
                if (statement.Head == null)
                {
                    continue;
                }

                StatementGroup? matchingGroup = groups.Find(g => equilizer.AreEqual(statement.Head, g.ReferenceLiteral));

                // if a matching group was found add statment to group.
                if (matchingGroup != null)
                {
                    matchingGroup.Statements.Add(statement);
                }
                else
                {
                    // if no matching group was found create a new one with this statement.
                    groups.Add(new StatementGroup(statement));
                }
            }

            return groups;
        }

        private List<DualRule> AddAllMissingNAFLiterals(Statement[] allStatements)
        {
            var allLiterals = allStatements
                    .SelectMany(s => s.Body.Where(b => b.Literal != null).Select(b => b.Literal))
                    .Union(allStatements.Where(s => s.Head != null).Select(s => s.Head)).Cast<Literal>().ToArray();

            var statementHeads = allStatements.Select(s => s.Head).Where(l => l != null).ToArray();

            var eqaulizer = new LiteralParamCountEqualizer();
            var newRules = new List<DualRule>();
            foreach (Literal literal in allLiterals)
            {
                if (literal == null || literal.IsNAF)
                {
                    continue;
                }

                var litCopy = (Literal)literal.Clone();
                litCopy.IsNAF = true;

                // if there is a naf version of this literal continue.
                if (statementHeads.Where(lit => eqaulizer.AreEqual(lit!, litCopy)).Any())
                {
                    continue;
                }

                // if the rule already exists
                if (newRules.Where(rule => eqaulizer.AreEqual(rule.Head!, litCopy)).Any())
                {
                    continue;
                }

                var paramCount = 0;
                foreach (var param in litCopy.Atom.ParamList)
                {
                    param.Term = new Term($"DV/{paramCount}");
                    param.Literal = null;
                    paramCount++;
                }

                newRules.Add(new DualRule(litCopy));
            }

            return newRules;
        }

        private List<DualRule> GenerateDualRulesForGroup(StatementGroup group)
        {
            List<DualRule> dualRules = new();
            string? statementName = group.Statements[0].Head?.Atom.Name;

            int index = 0;
            foreach (Statement statement in group.Statements)
            {
                // create dual rules for each statment
                // the names of the rules just get appended by their index.
                string rulename = $"_{statementName}{index}";

                // check if we need to build a forall clause or not
                List<DualRule> rules = this.GenerateDualRulesFor(rulename, statement);
                dualRules.AddRange(rules);

                index++;
            }

            // if no dual rules where created return empty array.
            if (dualRules.Count == 0)
            {
                return dualRules;
            }

            // build overarching rule for all created dual rules.
            DualRule overarchingRule = this.GenerateOverarchingRule(group.Statements[0], dualRules);
            dualRules.Add(overarchingRule);

            return dualRules;
        }

        private DualRule GenerateOverarchingRule(Statement exampleStatement, IEnumerable<DualRule> dualRules)
        {
            if (exampleStatement.Head == null)
            {
                throw new ArgumentNullException(nameof(exampleStatement), "Head of example is not alled to be null");
            }

            AtomParam[] paramList = exampleStatement.Head.Atom.ParamList.Select((p, index) => new AtomParam(null, new Term($"V/{index}"))).ToArray();

            List<BodyPart> body = new();
            foreach (DualRule rule in dualRules)
            {
                if (rule.Head == null)
                {
                    throw new NullReferenceException("Head of rule is not allowed to be null");
                }

                if (rule.Head.Atom.ParamList.Length != exampleStatement.Head.Atom.ParamList.Length)
                {
                    // this rule is part of an forall rule and has gotten its own overarching rule.
                    continue;
                }

                // if this rule head is already in the body skip it.
                if (body.Where(bp => bp.Literal?.Atom.Name == rule.Head.Atom.Name).Any())
                {
                    continue;
                }

                body.Add(
                    new BodyPart(
                        new Literal(
                            new Atom(
                                rule.Head.Atom.Name,
                                paramList),
                            true,
                            rule.Head.IsNegative),
                        null));
            }

            return new DualRule(
                new Literal(
                    new Atom(exampleStatement.Head.Atom.Name, paramList),
                    true,
                    exampleStatement.Head.IsNegative),
                body.ToArray());
        }

        private List<DualRule> GenerateDualRulesFor(string ruleName, Statement statement)
        {
            if (statement.Head == null)
            {
                throw new ArgumentNullException(nameof(statement), "Head of statement is not allowed to be null in Dual Rule generation");
            }

            List<DualRule> rules = new();
            List<Term> linkingVariables = this.GetAllVariablesNotInHead(statement);
            Statement processedStatement = this.MoveAtomsFromHeadToBody(statement, linkingVariables);
            if (processedStatement.Head == null)
            {
                throw new ArgumentNullException(nameof(processedStatement), "Head of statement is not allowed to be null in Dual Rule generation");
            }

            processedStatement.Head.IsNAF = true;
            processedStatement.Head.Atom.Name = ruleName;

            for (int i = 0; i < processedStatement.Body.Length; i++)
            {
                // Generate the body of the rules.
                BodyPart[] ruleBody = new BodyPart[i + 1];
                for (int x = 0; x <= i; x++)
                {
                    ruleBody[x] = (BodyPart)processedStatement.Body[x].Clone();

                    // if we are in the last body part we need to generate switch the NAF
                    if (x == i)
                    {
                        this.SwitchNegation(ruleBody[x]);
                    }
                }

                rules.Add(new DualRule(processedStatement.Head, ruleBody.ToArray()));
            }

            if (linkingVariables.Count() > 0)
            {
                // generate forall rule
                BodyPart body = this.BuildForAllBody(processedStatement.Head, linkingVariables);
                Literal newHead = (Literal)statement.Head.Clone();
                newHead.IsNAF = true;
                newHead.Atom.Name = ruleName;
                rules.Add(new DualRule(newHead, new BodyPart[] { body }));
            }

            return rules;
        }
    }
}
