﻿using Apollon.Lib.Atoms;
using Apollon.Lib.Extensions;
using Apollon.Lib.Resolution.CoSLD.States;
using Apollon.Lib.Rules.Operations;
using Apollon.Lib.Unification;
using Apollon.Lib.Unification.Substitutioners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution.CoSLD
{
    public class OperationResolver
    {

        private Dictionary<Operator, Func<Operation, ResolutionBaseState, CoResolutionResult>> resolvers;

        private IUnifier constructiveUnifier = new ConstructiveUnifier();

        public OperationResolver()
        {
            this.resolvers = new Dictionary<Operator, Func<Operation, ResolutionBaseState, CoResolutionResult>>();
            this.PopulateResolvers();
        }

        public CoResolutionResult ResolveOperation(Operation operation, ResolutionBaseState state) 
        {
            if (operation.IsNAF)
            {
                throw new NotSupportedException("The resolution of naf negated operations is not supported yet.");
            }

            var resolver = this.resolvers[operation.Operator];

            try
            {
                var res = resolver(operation, state);

                state.Logger.Silly($"Operation {operation} {(res.Success ? "succeeded" : "failed")}");
                res.State.LogState();

                return res;
            } catch (Exception e)
            {
                state.Logger.Warn($"Encountered error while resolving operation {operation}. Failing resolution. Error: {e.Message}");
                return new CoResolutionResult();
            }
        }

        private void PopulateResolvers()
        {
            this.resolvers.Add(Operator.Equals, this.ResolveEquals);
            this.resolvers.Add(Operator.NotEquals, this.ResolveNotEquals);
            this.resolvers.Add(Operator.LessThan, this.ResolveLessThan);
            this.resolvers.Add(Operator.GreaterThan, this.ResolveGreaterThan);
            this.resolvers.Add(Operator.LessThanOrEqual, this.ResolveLessThanOrEqual);
            this.resolvers.Add(Operator.GreaterThanOrEqual, this.ResolveGreaterThanOrEqual);

            this.resolvers.Add(Operator.Plus, this.ResolvePlus);
            this.resolvers.Add(Operator.Minus, this.ResolveMinus);
            this.resolvers.Add(Operator.Times, this.ResolveTimes);
            this.resolvers.Add(Operator.Divide, this.ResolveDivide);
        }

        private CoResolutionResult ResolveEquals(Operation operation, ResolutionBaseState state)
        {
            this.ThrowIfGeneratingOperation(operation);
            ArgumentNullException.ThrowIfNull(operation.Condition.Literal);
            ArgumentNullException.ThrowIfNull(operation.Variable.Literal);

            var op = state.Substitution.Apply(operation);
            var condition = this.ExtractAsLiteral(op.Condition, state);
            var variable = this.ExtractAsLiteral(op.Variable, state);

            var unificationRes = this.constructiveUnifier.Unify(condition, variable);
            return new CoResolutionResult(unificationRes.IsSuccess, unificationRes.Value ?? new Substitution(), state);
        }

        private CoResolutionResult ResolveNotEquals(Operation operation, ResolutionBaseState state)
        {
            this.ThrowIfGeneratingOperation(operation);
            if (operation.Variable.Term == null || !operation.Variable.Term.IsVariable)
            {
                throw new InvalidOperationException("operation variable is not variable...");
            }
            operation.Variable.Term.ProhibitedValues.AddValue(operation.Condition);

            var op = state.Substitution.Apply(operation);
            var condition = this.ExtractAsLiteral(op.Condition, state);
            var variable = this.ExtractAsLiteral(op.Variable, state);

            var areEqual = condition.Equals(variable);
            return new CoResolutionResult(!areEqual, state.Substitution, state);
        }

        private CoResolutionResult ResolveLessThan(Operation operation, ResolutionBaseState state)
        {
            return this.ConditionalOperationBase(operation, state, (var, con) => var < con);
        }

        private CoResolutionResult ResolveGreaterThan(Operation operation, ResolutionBaseState state)
        {
            return this.ConditionalOperationBase(operation, state, (var, con) => var > con);
        }

        private CoResolutionResult ResolveGreaterThanOrEqual(Operation operation, ResolutionBaseState state)
        {
            return this.ConditionalOperationBase(operation, state, (var, con) => var >= con);
        }

        private CoResolutionResult ResolveLessThanOrEqual(Operation operation, ResolutionBaseState state)
        {
            return this.ConditionalOperationBase(operation, state, (var, con) => var <= con);
        }

        private CoResolutionResult ConditionalOperationBase(Operation operation, ResolutionBaseState state, Func<int, int, bool> executor)
        {
            this.ThrowIfGeneratingOperation(operation);
            var sub = state.Substitution.Apply(operation);

            var variable = this.ExtractAsNumberOrThrow(sub.Variable, state);
            var condition = this.ExtractAsNumberOrThrow(sub.Condition, state);

            if (executor(variable, condition))
            {
                return new CoResolutionResult(true, state.Substitution, state);
            }

            return new CoResolutionResult();
        }

        private CoResolutionResult ResolvePlus(Operation operation, ResolutionBaseState state)
        {
            return this.GeneratingOperationBase(operation, state, (variable, condition) => variable + condition);
        }

        private CoResolutionResult ResolveMinus(Operation operation, ResolutionBaseState state)
        {
            return this.GeneratingOperationBase(operation, state, (variable, condition) => variable - condition);
        }

        private CoResolutionResult ResolveTimes(Operation operation, ResolutionBaseState state)
        {
            return this.GeneratingOperationBase(operation, state, (variable, condition) => variable * condition);
        }


        private CoResolutionResult ResolveDivide(Operation operation, ResolutionBaseState state)
        {
            return this.GeneratingOperationBase(operation, state, (variable, condition) => variable / condition);
        }

        private CoResolutionResult GeneratingOperationBase(Operation operation, ResolutionBaseState state, Func<int, int, int> executor)
        {
            this.ThrowIfInlineOperation(operation);
            var sub = state.Substitution.Apply(operation);

            var variable = this.ExtractAsNumberOrThrow(sub.Variable, state);
            var condition = this.ExtractAsNumberOrThrow(sub.Condition, state);
            var output = sub.OutputtingVariable;

            int res = executor(variable, condition);

            state.Logger.Silly($"Operation {sub} resulted in {res}");

            var stateCopy = (ResolutionBaseState)state.Clone();
            stateCopy.Substitution.Add(output, new Term(res.ToString()));

            return new CoResolutionResult(true, stateCopy.Substitution, stateCopy);
        }

        private void ThrowIfGeneratingOperation(Operation operation)
        {
            if (operation.OutputtingVariable != null)
            {
                throw new InvalidOperationException("Function cannot process generating operations.");
            }
        }

        private void ThrowIfInlineOperation(Operation operation)
        {
            if (operation.OutputtingVariable == null)
            {
                throw new InvalidOperationException("Function cannot process comperative operations.");
            }
        }

        private Literal ExtractAsLiteral(AtomParam param, ResolutionBaseState state)
        {
            if (param.Term != null)
            {
                var newLiteral = new Literal(new Atom(param.Term.Value), false, false);
                state.Logger.Silly($"Converted term {param} to literal {newLiteral}.");
                return newLiteral;
            }
            if (param.Literal != null)
            {
                return param.Literal;
            }
            throw new NotImplementedException($"Unahndled case for extraction as literal of {param}");
        }

        private int ExtractAsNumberOrThrow(AtomParam param, ResolutionBaseState state)
        {
            var term = this.ExtractAsTermOrThrow(param, state);

            if (!term.IsNumber())
            {
                throw new InvalidOperationException($"Unable to convert term {term} to a number");
            }

            return int.Parse(term.Value);
        }

        private Term ExtractAsTermOrThrow(AtomParam param, ResolutionBaseState state)
        {
            if (param.Term != null)
            {
                return param.Term;
            } 
            if (param.Literal != null)
            {
                if (param.Literal.Atom.ParamList.Length > 0 || param.Literal.IsNAF || param.Literal.IsNegative)
                {
                    throw new InvalidOperationException($"Unable to convert literal {param} to term.");
                }

                var newTerm = new Term(param.Literal.Atom.Name);

                state.Logger.Silly($"Converted literal {param.Literal} to term {newTerm}");
                return newTerm;
            }

            throw new NotImplementedException($"Unhandled case for extraction as term of {param}");
        }
    }
}