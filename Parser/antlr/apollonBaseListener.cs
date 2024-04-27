//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from c://Users//strei//Documents//dev//Apollon//Parser//antlr//apollon.g4 by ANTLR 4.13.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419


using Antlr4.Runtime.Misc;
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IapollonListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.Diagnostics.DebuggerNonUserCode]
[System.CLSCompliant(false)]
public partial class apollonBaseListener : IapollonListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="apollonParser.program"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterProgram([NotNull] apollonParser.ProgramContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="apollonParser.program"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitProgram([NotNull] apollonParser.ProgramContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="apollonParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterStatement([NotNull] apollonParser.StatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="apollonParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitStatement([NotNull] apollonParser.StatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="apollonParser.fact"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFact([NotNull] apollonParser.FactContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="apollonParser.fact"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFact([NotNull] apollonParser.FactContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="apollonParser.rule"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterRule([NotNull] apollonParser.RuleContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="apollonParser.rule"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitRule([NotNull] apollonParser.RuleContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="apollonParser.constraint"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterConstraint([NotNull] apollonParser.ConstraintContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="apollonParser.constraint"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitConstraint([NotNull] apollonParser.ConstraintContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="apollonParser.head"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterHead([NotNull] apollonParser.HeadContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="apollonParser.head"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitHead([NotNull] apollonParser.HeadContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="apollonParser.body"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBody([NotNull] apollonParser.BodyContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="apollonParser.body"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBody([NotNull] apollonParser.BodyContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="apollonParser.body_part"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBody_part([NotNull] apollonParser.Body_partContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="apollonParser.body_part"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBody_part([NotNull] apollonParser.Body_partContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="apollonParser.literal"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLiteral([NotNull] apollonParser.LiteralContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="apollonParser.literal"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLiteral([NotNull] apollonParser.LiteralContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="apollonParser.naf_literal"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNaf_literal([NotNull] apollonParser.Naf_literalContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="apollonParser.naf_literal"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNaf_literal([NotNull] apollonParser.Naf_literalContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="apollonParser.atom"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAtom([NotNull] apollonParser.AtomContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="apollonParser.atom"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAtom([NotNull] apollonParser.AtomContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="apollonParser.atom_param_part"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAtom_param_part([NotNull] apollonParser.Atom_param_partContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="apollonParser.atom_param_part"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAtom_param_part([NotNull] apollonParser.Atom_param_partContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="apollonParser.general_term"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterGeneral_term([NotNull] apollonParser.General_termContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="apollonParser.general_term"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitGeneral_term([NotNull] apollonParser.General_termContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="apollonParser.operation"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterOperation([NotNull] apollonParser.OperationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="apollonParser.operation"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitOperation([NotNull] apollonParser.OperationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="apollonParser.operator"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterOperator([NotNull] apollonParser.OperatorContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="apollonParser.operator"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitOperator([NotNull] apollonParser.OperatorContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
