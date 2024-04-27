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

using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public partial class apollonParser : Parser {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, NAF=6, NEGATION=7, EQUALS=8, NOT_EQUALS=9, 
		COMMENT=10, WS=11, CLASICAL_TERM=12, VARIABLE_TERM=13, NUMBER=14;
	public const int
		RULE_program = 0, RULE_statement = 1, RULE_fact = 2, RULE_rule = 3, RULE_constraint = 4, 
		RULE_head = 5, RULE_body = 6, RULE_body_part = 7, RULE_literal = 8, RULE_naf_literal = 9, 
		RULE_atom = 10, RULE_atom_param_part = 11, RULE_general_term = 12, RULE_operation = 13, 
		RULE_operator = 14;
	public static readonly string[] ruleNames = {
		"program", "statement", "fact", "rule", "constraint", "head", "body", 
		"body_part", "literal", "naf_literal", "atom", "atom_param_part", "general_term", 
		"operation", "operator"
	};

	private static readonly string[] _LiteralNames = {
		null, "'.'", "':-'", "','", "'('", "')'", "'not'", "'-'", "'='", "'!='"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, "NAF", "NEGATION", "EQUALS", "NOT_EQUALS", 
		"COMMENT", "WS", "CLASICAL_TERM", "VARIABLE_TERM", "NUMBER"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "apollon.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static apollonParser() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}

		public apollonParser(ITokenStream input) : this(input, Console.Out, Console.Error) { }

		public apollonParser(ITokenStream input, TextWriter output, TextWriter errorOutput)
		: base(input, output, errorOutput)
	{
		Interpreter = new ParserATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	public partial class ProgramContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode Eof() { return GetToken(apollonParser.Eof, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public StatementContext[] statement() {
			return GetRuleContexts<StatementContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public StatementContext statement(int i) {
			return GetRuleContext<StatementContext>(i);
		}
		public ProgramContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_program; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.EnterProgram(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.ExitProgram(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IapollonVisitor<TResult> typedVisitor = visitor as IapollonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitProgram(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ProgramContext program() {
		ProgramContext _localctx = new ProgramContext(Context, State);
		EnterRule(_localctx, 0, RULE_program);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 33;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & 5252L) != 0)) {
				{
				{
				State = 30;
				statement();
				}
				}
				State = 35;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 36;
			Match(Eof);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class StatementContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public FactContext fact() {
			return GetRuleContext<FactContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public RuleContext rule() {
			return GetRuleContext<RuleContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ConstraintContext constraint() {
			return GetRuleContext<ConstraintContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode COMMENT() { return GetToken(apollonParser.COMMENT, 0); }
		public StatementContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_statement; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.EnterStatement(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.ExitStatement(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IapollonVisitor<TResult> typedVisitor = visitor as IapollonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitStatement(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public StatementContext statement() {
		StatementContext _localctx = new StatementContext(Context, State);
		EnterRule(_localctx, 2, RULE_statement);
		try {
			State = 42;
			ErrorHandler.Sync(this);
			switch ( Interpreter.AdaptivePredict(TokenStream,1,Context) ) {
			case 1:
				EnterOuterAlt(_localctx, 1);
				{
				State = 38;
				fact();
				}
				break;
			case 2:
				EnterOuterAlt(_localctx, 2);
				{
				State = 39;
				rule();
				}
				break;
			case 3:
				EnterOuterAlt(_localctx, 3);
				{
				State = 40;
				constraint();
				}
				break;
			case 4:
				EnterOuterAlt(_localctx, 4);
				{
				State = 41;
				Match(COMMENT);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class FactContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public LiteralContext literal() {
			return GetRuleContext<LiteralContext>(0);
		}
		public FactContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_fact; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.EnterFact(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.ExitFact(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IapollonVisitor<TResult> typedVisitor = visitor as IapollonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitFact(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public FactContext fact() {
		FactContext _localctx = new FactContext(Context, State);
		EnterRule(_localctx, 4, RULE_fact);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 44;
			literal();
			State = 45;
			Match(T__0);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class RuleContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public HeadContext head() {
			return GetRuleContext<HeadContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public BodyContext body() {
			return GetRuleContext<BodyContext>(0);
		}
		public RuleContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_rule; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.EnterRule(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.ExitRule(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IapollonVisitor<TResult> typedVisitor = visitor as IapollonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitRule(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public RuleContext rule() {
		RuleContext _localctx = new RuleContext(Context, State);
		EnterRule(_localctx, 6, RULE_rule);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 47;
			head();
			State = 48;
			Match(T__1);
			State = 49;
			body();
			State = 50;
			Match(T__0);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ConstraintContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public Naf_literalContext[] naf_literal() {
			return GetRuleContexts<Naf_literalContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public Naf_literalContext naf_literal(int i) {
			return GetRuleContext<Naf_literalContext>(i);
		}
		public ConstraintContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_constraint; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.EnterConstraint(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.ExitConstraint(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IapollonVisitor<TResult> typedVisitor = visitor as IapollonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitConstraint(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ConstraintContext constraint() {
		ConstraintContext _localctx = new ConstraintContext(Context, State);
		EnterRule(_localctx, 8, RULE_constraint);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 52;
			Match(T__1);
			State = 53;
			naf_literal();
			State = 58;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==T__2) {
				{
				{
				State = 54;
				Match(T__2);
				State = 55;
				naf_literal();
				}
				}
				State = 60;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 61;
			Match(T__0);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class HeadContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public LiteralContext literal() {
			return GetRuleContext<LiteralContext>(0);
		}
		public HeadContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_head; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.EnterHead(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.ExitHead(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IapollonVisitor<TResult> typedVisitor = visitor as IapollonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitHead(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public HeadContext head() {
		HeadContext _localctx = new HeadContext(Context, State);
		EnterRule(_localctx, 10, RULE_head);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 63;
			literal();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class BodyContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public Body_partContext[] body_part() {
			return GetRuleContexts<Body_partContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public Body_partContext body_part(int i) {
			return GetRuleContext<Body_partContext>(i);
		}
		public BodyContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_body; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.EnterBody(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.ExitBody(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IapollonVisitor<TResult> typedVisitor = visitor as IapollonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitBody(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public BodyContext body() {
		BodyContext _localctx = new BodyContext(Context, State);
		EnterRule(_localctx, 12, RULE_body);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 65;
			body_part();
			State = 70;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==T__2) {
				{
				{
				State = 66;
				Match(T__2);
				State = 67;
				body_part();
				}
				}
				State = 72;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class Body_partContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public OperationContext operation() {
			return GetRuleContext<OperationContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public Naf_literalContext naf_literal() {
			return GetRuleContext<Naf_literalContext>(0);
		}
		public Body_partContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_body_part; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.EnterBody_part(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.ExitBody_part(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IapollonVisitor<TResult> typedVisitor = visitor as IapollonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitBody_part(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public Body_partContext body_part() {
		Body_partContext _localctx = new Body_partContext(Context, State);
		EnterRule(_localctx, 14, RULE_body_part);
		try {
			State = 75;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case VARIABLE_TERM:
				EnterOuterAlt(_localctx, 1);
				{
				State = 73;
				operation();
				}
				break;
			case NAF:
			case NEGATION:
			case CLASICAL_TERM:
				EnterOuterAlt(_localctx, 2);
				{
				State = 74;
				naf_literal();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class LiteralContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public AtomContext atom() {
			return GetRuleContext<AtomContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode NEGATION() { return GetToken(apollonParser.NEGATION, 0); }
		public LiteralContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_literal; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.EnterLiteral(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.ExitLiteral(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IapollonVisitor<TResult> typedVisitor = visitor as IapollonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitLiteral(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public LiteralContext literal() {
		LiteralContext _localctx = new LiteralContext(Context, State);
		EnterRule(_localctx, 16, RULE_literal);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 78;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==NEGATION) {
				{
				State = 77;
				Match(NEGATION);
				}
			}

			State = 80;
			atom();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class Naf_literalContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public LiteralContext literal() {
			return GetRuleContext<LiteralContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode NAF() { return GetToken(apollonParser.NAF, 0); }
		public Naf_literalContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_naf_literal; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.EnterNaf_literal(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.ExitNaf_literal(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IapollonVisitor<TResult> typedVisitor = visitor as IapollonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitNaf_literal(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public Naf_literalContext naf_literal() {
		Naf_literalContext _localctx = new Naf_literalContext(Context, State);
		EnterRule(_localctx, 18, RULE_naf_literal);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 83;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==NAF) {
				{
				State = 82;
				Match(NAF);
				}
			}

			State = 85;
			literal();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class AtomContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode CLASICAL_TERM() { return GetToken(apollonParser.CLASICAL_TERM, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public Atom_param_partContext[] atom_param_part() {
			return GetRuleContexts<Atom_param_partContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public Atom_param_partContext atom_param_part(int i) {
			return GetRuleContext<Atom_param_partContext>(i);
		}
		public AtomContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_atom; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.EnterAtom(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.ExitAtom(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IapollonVisitor<TResult> typedVisitor = visitor as IapollonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitAtom(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public AtomContext atom() {
		AtomContext _localctx = new AtomContext(Context, State);
		EnterRule(_localctx, 20, RULE_atom);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 87;
			Match(CLASICAL_TERM);
			State = 100;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==T__3) {
				{
				State = 88;
				Match(T__3);
				State = 97;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 28672L) != 0)) {
					{
					State = 89;
					atom_param_part();
					State = 94;
					ErrorHandler.Sync(this);
					_la = TokenStream.LA(1);
					while (_la==T__2) {
						{
						{
						State = 90;
						Match(T__2);
						State = 91;
						atom_param_part();
						}
						}
						State = 96;
						ErrorHandler.Sync(this);
						_la = TokenStream.LA(1);
					}
					}
				}

				State = 99;
				Match(T__4);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class Atom_param_partContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public General_termContext general_term() {
			return GetRuleContext<General_termContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode NUMBER() { return GetToken(apollonParser.NUMBER, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public AtomContext atom() {
			return GetRuleContext<AtomContext>(0);
		}
		public Atom_param_partContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_atom_param_part; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.EnterAtom_param_part(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.ExitAtom_param_part(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IapollonVisitor<TResult> typedVisitor = visitor as IapollonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitAtom_param_part(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public Atom_param_partContext atom_param_part() {
		Atom_param_partContext _localctx = new Atom_param_partContext(Context, State);
		EnterRule(_localctx, 22, RULE_atom_param_part);
		try {
			State = 105;
			ErrorHandler.Sync(this);
			switch ( Interpreter.AdaptivePredict(TokenStream,10,Context) ) {
			case 1:
				EnterOuterAlt(_localctx, 1);
				{
				State = 102;
				general_term();
				}
				break;
			case 2:
				EnterOuterAlt(_localctx, 2);
				{
				State = 103;
				Match(NUMBER);
				}
				break;
			case 3:
				EnterOuterAlt(_localctx, 3);
				{
				State = 104;
				atom();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class General_termContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode VARIABLE_TERM() { return GetToken(apollonParser.VARIABLE_TERM, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode CLASICAL_TERM() { return GetToken(apollonParser.CLASICAL_TERM, 0); }
		public General_termContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_general_term; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.EnterGeneral_term(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.ExitGeneral_term(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IapollonVisitor<TResult> typedVisitor = visitor as IapollonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitGeneral_term(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public General_termContext general_term() {
		General_termContext _localctx = new General_termContext(Context, State);
		EnterRule(_localctx, 24, RULE_general_term);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 107;
			_la = TokenStream.LA(1);
			if ( !(_la==CLASICAL_TERM || _la==VARIABLE_TERM) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class OperationContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode VARIABLE_TERM() { return GetToken(apollonParser.VARIABLE_TERM, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public OperatorContext @operator() {
			return GetRuleContext<OperatorContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public AtomContext atom() {
			return GetRuleContext<AtomContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode NUMBER() { return GetToken(apollonParser.NUMBER, 0); }
		public OperationContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_operation; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.EnterOperation(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.ExitOperation(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IapollonVisitor<TResult> typedVisitor = visitor as IapollonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitOperation(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public OperationContext operation() {
		OperationContext _localctx = new OperationContext(Context, State);
		EnterRule(_localctx, 26, RULE_operation);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 109;
			Match(VARIABLE_TERM);
			State = 110;
			@operator();
			State = 113;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case CLASICAL_TERM:
				{
				State = 111;
				atom();
				}
				break;
			case NUMBER:
				{
				State = 112;
				Match(NUMBER);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class OperatorContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode EQUALS() { return GetToken(apollonParser.EQUALS, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode NOT_EQUALS() { return GetToken(apollonParser.NOT_EQUALS, 0); }
		public OperatorContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_operator; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.EnterOperator(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IapollonListener typedListener = listener as IapollonListener;
			if (typedListener != null) typedListener.ExitOperator(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IapollonVisitor<TResult> typedVisitor = visitor as IapollonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitOperator(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public OperatorContext @operator() {
		OperatorContext _localctx = new OperatorContext(Context, State);
		EnterRule(_localctx, 28, RULE_operator);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 115;
			_la = TokenStream.LA(1);
			if ( !(_la==EQUALS || _la==NOT_EQUALS) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	private static int[] _serializedATN = {
		4,1,14,118,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,6,2,7,
		7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,7,14,
		1,0,5,0,32,8,0,10,0,12,0,35,9,0,1,0,1,0,1,1,1,1,1,1,1,1,3,1,43,8,1,1,2,
		1,2,1,2,1,3,1,3,1,3,1,3,1,3,1,4,1,4,1,4,1,4,5,4,57,8,4,10,4,12,4,60,9,
		4,1,4,1,4,1,5,1,5,1,6,1,6,1,6,5,6,69,8,6,10,6,12,6,72,9,6,1,7,1,7,3,7,
		76,8,7,1,8,3,8,79,8,8,1,8,1,8,1,9,3,9,84,8,9,1,9,1,9,1,10,1,10,1,10,1,
		10,1,10,5,10,93,8,10,10,10,12,10,96,9,10,3,10,98,8,10,1,10,3,10,101,8,
		10,1,11,1,11,1,11,3,11,106,8,11,1,12,1,12,1,13,1,13,1,13,1,13,3,13,114,
		8,13,1,14,1,14,1,14,0,0,15,0,2,4,6,8,10,12,14,16,18,20,22,24,26,28,0,2,
		1,0,12,13,1,0,8,9,117,0,33,1,0,0,0,2,42,1,0,0,0,4,44,1,0,0,0,6,47,1,0,
		0,0,8,52,1,0,0,0,10,63,1,0,0,0,12,65,1,0,0,0,14,75,1,0,0,0,16,78,1,0,0,
		0,18,83,1,0,0,0,20,87,1,0,0,0,22,105,1,0,0,0,24,107,1,0,0,0,26,109,1,0,
		0,0,28,115,1,0,0,0,30,32,3,2,1,0,31,30,1,0,0,0,32,35,1,0,0,0,33,31,1,0,
		0,0,33,34,1,0,0,0,34,36,1,0,0,0,35,33,1,0,0,0,36,37,5,0,0,1,37,1,1,0,0,
		0,38,43,3,4,2,0,39,43,3,6,3,0,40,43,3,8,4,0,41,43,5,10,0,0,42,38,1,0,0,
		0,42,39,1,0,0,0,42,40,1,0,0,0,42,41,1,0,0,0,43,3,1,0,0,0,44,45,3,16,8,
		0,45,46,5,1,0,0,46,5,1,0,0,0,47,48,3,10,5,0,48,49,5,2,0,0,49,50,3,12,6,
		0,50,51,5,1,0,0,51,7,1,0,0,0,52,53,5,2,0,0,53,58,3,18,9,0,54,55,5,3,0,
		0,55,57,3,18,9,0,56,54,1,0,0,0,57,60,1,0,0,0,58,56,1,0,0,0,58,59,1,0,0,
		0,59,61,1,0,0,0,60,58,1,0,0,0,61,62,5,1,0,0,62,9,1,0,0,0,63,64,3,16,8,
		0,64,11,1,0,0,0,65,70,3,14,7,0,66,67,5,3,0,0,67,69,3,14,7,0,68,66,1,0,
		0,0,69,72,1,0,0,0,70,68,1,0,0,0,70,71,1,0,0,0,71,13,1,0,0,0,72,70,1,0,
		0,0,73,76,3,26,13,0,74,76,3,18,9,0,75,73,1,0,0,0,75,74,1,0,0,0,76,15,1,
		0,0,0,77,79,5,7,0,0,78,77,1,0,0,0,78,79,1,0,0,0,79,80,1,0,0,0,80,81,3,
		20,10,0,81,17,1,0,0,0,82,84,5,6,0,0,83,82,1,0,0,0,83,84,1,0,0,0,84,85,
		1,0,0,0,85,86,3,16,8,0,86,19,1,0,0,0,87,100,5,12,0,0,88,97,5,4,0,0,89,
		94,3,22,11,0,90,91,5,3,0,0,91,93,3,22,11,0,92,90,1,0,0,0,93,96,1,0,0,0,
		94,92,1,0,0,0,94,95,1,0,0,0,95,98,1,0,0,0,96,94,1,0,0,0,97,89,1,0,0,0,
		97,98,1,0,0,0,98,99,1,0,0,0,99,101,5,5,0,0,100,88,1,0,0,0,100,101,1,0,
		0,0,101,21,1,0,0,0,102,106,3,24,12,0,103,106,5,14,0,0,104,106,3,20,10,
		0,105,102,1,0,0,0,105,103,1,0,0,0,105,104,1,0,0,0,106,23,1,0,0,0,107,108,
		7,0,0,0,108,25,1,0,0,0,109,110,5,13,0,0,110,113,3,28,14,0,111,114,3,20,
		10,0,112,114,5,14,0,0,113,111,1,0,0,0,113,112,1,0,0,0,114,27,1,0,0,0,115,
		116,7,1,0,0,116,29,1,0,0,0,12,33,42,58,70,75,78,83,94,97,100,105,113
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
