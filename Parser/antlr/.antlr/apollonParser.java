// Generated from c://Users//strei//Documents//dev//Apollon//Parser//antlr//apollon.g4 by ANTLR 4.13.1
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.*;
import org.antlr.v4.runtime.tree.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast", "CheckReturnValue"})
public class apollonParser extends Parser {
	static { RuntimeMetaData.checkVersion("4.13.1", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, NAF=8, NEGATION=9, 
		EQUALS=10, LARGER=11, SMALLER=12, LARGER_EQUALS=13, SMALLER_EQUALS=14, 
		PLUS=15, TIMES=16, DIVIDE=17, NOT_EQUALS=18, COMMENT=19, WS=20, CLASICAL_TERM=21, 
		VARIABLE_TERM=22, DOKU_SEPERATOR=23, NUMBER=24;
	public static final int
		RULE_program = 0, RULE_query = 1, RULE_statement = 2, RULE_fact = 3, RULE_rule = 4, 
		RULE_constraint = 5, RULE_head = 6, RULE_body = 7, RULE_body_part = 8, 
		RULE_literal = 9, RULE_variable_placeholder = 10, RULE_docu_string = 11, 
		RULE_docu_string_part = 12, RULE_docu_string_string_part = 13, RULE_docu = 14, 
		RULE_docu_head = 15, RULE_naf_literal = 16, RULE_atom = 17, RULE_atom_param_part = 18, 
		RULE_general_term = 19, RULE_inline_operation = 20, RULE_generating_operation = 21, 
		RULE_generating_operation_variable = 22, RULE_generating_operation_operant = 23, 
		RULE_inline_operators = 24, RULE_generating_operators = 25;
	private static String[] makeRuleNames() {
		return new String[] {
			"program", "query", "statement", "fact", "rule", "constraint", "head", 
			"body", "body_part", "literal", "variable_placeholder", "docu_string", 
			"docu_string_part", "docu_string_string_part", "docu", "docu_head", "naf_literal", 
			"atom", "atom_param_part", "general_term", "inline_operation", "generating_operation", 
			"generating_operation_variable", "generating_operation_operant", "inline_operators", 
			"generating_operators"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, "','", "'.'", "':-'", "'@('", "')'", "'is'", "'('", "'not'", "'-'", 
			"'='", "'>'", "'<'", "'>='", "'<='", "'+'", "'*'", "'/'", "'!='", null, 
			null, null, null, "'::'"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, null, null, null, null, null, null, null, "NAF", "NEGATION", "EQUALS", 
			"LARGER", "SMALLER", "LARGER_EQUALS", "SMALLER_EQUALS", "PLUS", "TIMES", 
			"DIVIDE", "NOT_EQUALS", "COMMENT", "WS", "CLASICAL_TERM", "VARIABLE_TERM", 
			"DOKU_SEPERATOR", "NUMBER"
		};
	}
	private static final String[] _SYMBOLIC_NAMES = makeSymbolicNames();
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}

	@Override
	public String getGrammarFileName() { return "apollon.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public ATN getATN() { return _ATN; }

	public apollonParser(TokenStream input) {
		super(input);
		_interp = new ParserATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ProgramContext extends ParserRuleContext {
		public TerminalNode EOF() { return getToken(apollonParser.EOF, 0); }
		public List<StatementContext> statement() {
			return getRuleContexts(StatementContext.class);
		}
		public StatementContext statement(int i) {
			return getRuleContext(StatementContext.class,i);
		}
		public ProgramContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_program; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterProgram(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitProgram(this);
		}
	}

	public final ProgramContext program() throws RecognitionException {
		ProgramContext _localctx = new ProgramContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_program);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(55);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & 2621960L) != 0)) {
				{
				{
				setState(52);
				statement();
				}
				}
				setState(57);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(58);
			match(EOF);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class QueryContext extends ParserRuleContext {
		public List<Body_partContext> body_part() {
			return getRuleContexts(Body_partContext.class);
		}
		public Body_partContext body_part(int i) {
			return getRuleContext(Body_partContext.class,i);
		}
		public TerminalNode EOF() { return getToken(apollonParser.EOF, 0); }
		public QueryContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_query; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterQuery(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitQuery(this);
		}
	}

	public final QueryContext query() throws RecognitionException {
		QueryContext _localctx = new QueryContext(_ctx, getState());
		enterRule(_localctx, 2, RULE_query);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(60);
			body_part();
			setState(65);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==T__0) {
				{
				{
				setState(61);
				match(T__0);
				setState(62);
				body_part();
				}
				}
				setState(67);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(68);
			match(T__1);
			setState(69);
			match(EOF);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class StatementContext extends ParserRuleContext {
		public FactContext fact() {
			return getRuleContext(FactContext.class,0);
		}
		public RuleContext rule_() {
			return getRuleContext(RuleContext.class,0);
		}
		public ConstraintContext constraint() {
			return getRuleContext(ConstraintContext.class,0);
		}
		public DocuContext docu() {
			return getRuleContext(DocuContext.class,0);
		}
		public TerminalNode COMMENT() { return getToken(apollonParser.COMMENT, 0); }
		public StatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_statement; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterStatement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitStatement(this);
		}
	}

	public final StatementContext statement() throws RecognitionException {
		StatementContext _localctx = new StatementContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_statement);
		try {
			setState(76);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,2,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(71);
				fact();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(72);
				rule_();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(73);
				constraint();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(74);
				docu();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(75);
				match(COMMENT);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class FactContext extends ParserRuleContext {
		public LiteralContext literal() {
			return getRuleContext(LiteralContext.class,0);
		}
		public FactContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_fact; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterFact(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitFact(this);
		}
	}

	public final FactContext fact() throws RecognitionException {
		FactContext _localctx = new FactContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_fact);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(78);
			literal();
			setState(79);
			match(T__1);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class RuleContext extends ParserRuleContext {
		public HeadContext head() {
			return getRuleContext(HeadContext.class,0);
		}
		public BodyContext body() {
			return getRuleContext(BodyContext.class,0);
		}
		public RuleContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_rule; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterRule(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitRule(this);
		}
	}

	public final RuleContext rule_() throws RecognitionException {
		RuleContext _localctx = new RuleContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_rule);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(81);
			head();
			setState(82);
			match(T__2);
			setState(83);
			body();
			setState(84);
			match(T__1);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ConstraintContext extends ParserRuleContext {
		public List<Naf_literalContext> naf_literal() {
			return getRuleContexts(Naf_literalContext.class);
		}
		public Naf_literalContext naf_literal(int i) {
			return getRuleContext(Naf_literalContext.class,i);
		}
		public ConstraintContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_constraint; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterConstraint(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitConstraint(this);
		}
	}

	public final ConstraintContext constraint() throws RecognitionException {
		ConstraintContext _localctx = new ConstraintContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_constraint);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(86);
			match(T__2);
			setState(87);
			naf_literal();
			setState(92);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==T__0) {
				{
				{
				setState(88);
				match(T__0);
				setState(89);
				naf_literal();
				}
				}
				setState(94);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(95);
			match(T__1);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class HeadContext extends ParserRuleContext {
		public LiteralContext literal() {
			return getRuleContext(LiteralContext.class,0);
		}
		public HeadContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_head; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterHead(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitHead(this);
		}
	}

	public final HeadContext head() throws RecognitionException {
		HeadContext _localctx = new HeadContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_head);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(97);
			literal();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class BodyContext extends ParserRuleContext {
		public List<Body_partContext> body_part() {
			return getRuleContexts(Body_partContext.class);
		}
		public Body_partContext body_part(int i) {
			return getRuleContext(Body_partContext.class,i);
		}
		public BodyContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_body; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterBody(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitBody(this);
		}
	}

	public final BodyContext body() throws RecognitionException {
		BodyContext _localctx = new BodyContext(_ctx, getState());
		enterRule(_localctx, 14, RULE_body);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(99);
			body_part();
			setState(104);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==T__0) {
				{
				{
				setState(100);
				match(T__0);
				setState(101);
				body_part();
				}
				}
				setState(106);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Body_partContext extends ParserRuleContext {
		public Inline_operationContext inline_operation() {
			return getRuleContext(Inline_operationContext.class,0);
		}
		public Naf_literalContext naf_literal() {
			return getRuleContext(Naf_literalContext.class,0);
		}
		public Generating_operationContext generating_operation() {
			return getRuleContext(Generating_operationContext.class,0);
		}
		public Body_partContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_body_part; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterBody_part(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitBody_part(this);
		}
	}

	public final Body_partContext body_part() throws RecognitionException {
		Body_partContext _localctx = new Body_partContext(_ctx, getState());
		enterRule(_localctx, 16, RULE_body_part);
		try {
			setState(110);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,5,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(107);
				inline_operation();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(108);
				naf_literal();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(109);
				generating_operation();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class LiteralContext extends ParserRuleContext {
		public AtomContext atom() {
			return getRuleContext(AtomContext.class,0);
		}
		public TerminalNode NEGATION() { return getToken(apollonParser.NEGATION, 0); }
		public LiteralContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_literal; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterLiteral(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitLiteral(this);
		}
	}

	public final LiteralContext literal() throws RecognitionException {
		LiteralContext _localctx = new LiteralContext(_ctx, getState());
		enterRule(_localctx, 18, RULE_literal);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(113);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NEGATION) {
				{
				setState(112);
				match(NEGATION);
				}
			}

			setState(115);
			atom();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Variable_placeholderContext extends ParserRuleContext {
		public TerminalNode VARIABLE_TERM() { return getToken(apollonParser.VARIABLE_TERM, 0); }
		public Variable_placeholderContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_variable_placeholder; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterVariable_placeholder(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitVariable_placeholder(this);
		}
	}

	public final Variable_placeholderContext variable_placeholder() throws RecognitionException {
		Variable_placeholderContext _localctx = new Variable_placeholderContext(_ctx, getState());
		enterRule(_localctx, 20, RULE_variable_placeholder);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(117);
			match(T__3);
			setState(118);
			match(VARIABLE_TERM);
			setState(119);
			match(T__4);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Docu_stringContext extends ParserRuleContext {
		public List<Docu_string_partContext> docu_string_part() {
			return getRuleContexts(Docu_string_partContext.class);
		}
		public Docu_string_partContext docu_string_part(int i) {
			return getRuleContext(Docu_string_partContext.class,i);
		}
		public Docu_stringContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_docu_string; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterDocu_string(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitDocu_string(this);
		}
	}

	public final Docu_stringContext docu_string() throws RecognitionException {
		Docu_stringContext _localctx = new Docu_stringContext(_ctx, getState());
		enterRule(_localctx, 22, RULE_docu_string);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(122); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(121);
				docu_string_part();
				}
				}
				setState(124); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & 6291536L) != 0) );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Docu_string_partContext extends ParserRuleContext {
		public Docu_string_string_partContext docu_string_string_part() {
			return getRuleContext(Docu_string_string_partContext.class,0);
		}
		public Variable_placeholderContext variable_placeholder() {
			return getRuleContext(Variable_placeholderContext.class,0);
		}
		public Docu_string_partContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_docu_string_part; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterDocu_string_part(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitDocu_string_part(this);
		}
	}

	public final Docu_string_partContext docu_string_part() throws RecognitionException {
		Docu_string_partContext _localctx = new Docu_string_partContext(_ctx, getState());
		enterRule(_localctx, 24, RULE_docu_string_part);
		try {
			setState(128);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case T__5:
			case CLASICAL_TERM:
			case VARIABLE_TERM:
				enterOuterAlt(_localctx, 1);
				{
				setState(126);
				docu_string_string_part();
				}
				break;
			case T__3:
				enterOuterAlt(_localctx, 2);
				{
				setState(127);
				variable_placeholder();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Docu_string_string_partContext extends ParserRuleContext {
		public General_termContext general_term() {
			return getRuleContext(General_termContext.class,0);
		}
		public Docu_string_string_partContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_docu_string_string_part; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterDocu_string_string_part(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitDocu_string_string_part(this);
		}
	}

	public final Docu_string_string_partContext docu_string_string_part() throws RecognitionException {
		Docu_string_string_partContext _localctx = new Docu_string_string_partContext(_ctx, getState());
		enterRule(_localctx, 26, RULE_docu_string_string_part);
		try {
			setState(132);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case CLASICAL_TERM:
			case VARIABLE_TERM:
				enterOuterAlt(_localctx, 1);
				{
				setState(130);
				general_term();
				}
				break;
			case T__5:
				enterOuterAlt(_localctx, 2);
				{
				setState(131);
				match(T__5);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class DocuContext extends ParserRuleContext {
		public Docu_headContext docu_head() {
			return getRuleContext(Docu_headContext.class,0);
		}
		public TerminalNode DOKU_SEPERATOR() { return getToken(apollonParser.DOKU_SEPERATOR, 0); }
		public Docu_stringContext docu_string() {
			return getRuleContext(Docu_stringContext.class,0);
		}
		public DocuContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_docu; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterDocu(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitDocu(this);
		}
	}

	public final DocuContext docu() throws RecognitionException {
		DocuContext _localctx = new DocuContext(_ctx, getState());
		enterRule(_localctx, 28, RULE_docu);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(134);
			docu_head();
			setState(137);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==DOKU_SEPERATOR) {
				{
				setState(135);
				match(DOKU_SEPERATOR);
				setState(136);
				docu_string();
				}
			}

			setState(139);
			match(T__1);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Docu_headContext extends ParserRuleContext {
		public TerminalNode CLASICAL_TERM() { return getToken(apollonParser.CLASICAL_TERM, 0); }
		public TerminalNode NEGATION() { return getToken(apollonParser.NEGATION, 0); }
		public List<TerminalNode> VARIABLE_TERM() { return getTokens(apollonParser.VARIABLE_TERM); }
		public TerminalNode VARIABLE_TERM(int i) {
			return getToken(apollonParser.VARIABLE_TERM, i);
		}
		public Docu_headContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_docu_head; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterDocu_head(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitDocu_head(this);
		}
	}

	public final Docu_headContext docu_head() throws RecognitionException {
		Docu_headContext _localctx = new Docu_headContext(_ctx, getState());
		enterRule(_localctx, 30, RULE_docu_head);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(142);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NEGATION) {
				{
				setState(141);
				match(NEGATION);
				}
			}

			setState(144);
			match(CLASICAL_TERM);
			setState(157);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==T__6) {
				{
				setState(145);
				match(T__6);
				setState(154);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==VARIABLE_TERM) {
					{
					setState(146);
					match(VARIABLE_TERM);
					setState(151);
					_errHandler.sync(this);
					_la = _input.LA(1);
					while (_la==T__0) {
						{
						{
						setState(147);
						match(T__0);
						setState(148);
						match(VARIABLE_TERM);
						}
						}
						setState(153);
						_errHandler.sync(this);
						_la = _input.LA(1);
					}
					}
				}

				setState(156);
				match(T__4);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Naf_literalContext extends ParserRuleContext {
		public LiteralContext literal() {
			return getRuleContext(LiteralContext.class,0);
		}
		public TerminalNode NAF() { return getToken(apollonParser.NAF, 0); }
		public Naf_literalContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_naf_literal; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterNaf_literal(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitNaf_literal(this);
		}
	}

	public final Naf_literalContext naf_literal() throws RecognitionException {
		Naf_literalContext _localctx = new Naf_literalContext(_ctx, getState());
		enterRule(_localctx, 32, RULE_naf_literal);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(160);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NAF) {
				{
				setState(159);
				match(NAF);
				}
			}

			setState(162);
			literal();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class AtomContext extends ParserRuleContext {
		public TerminalNode CLASICAL_TERM() { return getToken(apollonParser.CLASICAL_TERM, 0); }
		public List<Atom_param_partContext> atom_param_part() {
			return getRuleContexts(Atom_param_partContext.class);
		}
		public Atom_param_partContext atom_param_part(int i) {
			return getRuleContext(Atom_param_partContext.class,i);
		}
		public AtomContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_atom; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterAtom(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitAtom(this);
		}
	}

	public final AtomContext atom() throws RecognitionException {
		AtomContext _localctx = new AtomContext(_ctx, getState());
		enterRule(_localctx, 34, RULE_atom);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(164);
			match(CLASICAL_TERM);
			setState(177);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==T__6) {
				{
				setState(165);
				match(T__6);
				setState(174);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 23069184L) != 0)) {
					{
					setState(166);
					atom_param_part();
					setState(171);
					_errHandler.sync(this);
					_la = _input.LA(1);
					while (_la==T__0) {
						{
						{
						setState(167);
						match(T__0);
						setState(168);
						atom_param_part();
						}
						}
						setState(173);
						_errHandler.sync(this);
						_la = _input.LA(1);
					}
					}
				}

				setState(176);
				match(T__4);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Atom_param_partContext extends ParserRuleContext {
		public General_termContext general_term() {
			return getRuleContext(General_termContext.class,0);
		}
		public TerminalNode NUMBER() { return getToken(apollonParser.NUMBER, 0); }
		public LiteralContext literal() {
			return getRuleContext(LiteralContext.class,0);
		}
		public Atom_param_partContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_atom_param_part; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterAtom_param_part(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitAtom_param_part(this);
		}
	}

	public final Atom_param_partContext atom_param_part() throws RecognitionException {
		Atom_param_partContext _localctx = new Atom_param_partContext(_ctx, getState());
		enterRule(_localctx, 36, RULE_atom_param_part);
		try {
			setState(182);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,19,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(179);
				general_term();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(180);
				match(NUMBER);
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(181);
				literal();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class General_termContext extends ParserRuleContext {
		public TerminalNode VARIABLE_TERM() { return getToken(apollonParser.VARIABLE_TERM, 0); }
		public TerminalNode CLASICAL_TERM() { return getToken(apollonParser.CLASICAL_TERM, 0); }
		public General_termContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_general_term; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterGeneral_term(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitGeneral_term(this);
		}
	}

	public final General_termContext general_term() throws RecognitionException {
		General_termContext _localctx = new General_termContext(_ctx, getState());
		enterRule(_localctx, 38, RULE_general_term);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(184);
			_la = _input.LA(1);
			if ( !(_la==CLASICAL_TERM || _la==VARIABLE_TERM) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Inline_operationContext extends ParserRuleContext {
		public TerminalNode VARIABLE_TERM() { return getToken(apollonParser.VARIABLE_TERM, 0); }
		public Inline_operatorsContext inline_operators() {
			return getRuleContext(Inline_operatorsContext.class,0);
		}
		public Atom_param_partContext atom_param_part() {
			return getRuleContext(Atom_param_partContext.class,0);
		}
		public Inline_operationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_inline_operation; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterInline_operation(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitInline_operation(this);
		}
	}

	public final Inline_operationContext inline_operation() throws RecognitionException {
		Inline_operationContext _localctx = new Inline_operationContext(_ctx, getState());
		enterRule(_localctx, 40, RULE_inline_operation);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(186);
			match(VARIABLE_TERM);
			setState(187);
			inline_operators();
			setState(188);
			atom_param_part();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Generating_operationContext extends ParserRuleContext {
		public TerminalNode VARIABLE_TERM() { return getToken(apollonParser.VARIABLE_TERM, 0); }
		public Generating_operation_variableContext generating_operation_variable() {
			return getRuleContext(Generating_operation_variableContext.class,0);
		}
		public Generating_operatorsContext generating_operators() {
			return getRuleContext(Generating_operatorsContext.class,0);
		}
		public Generating_operation_operantContext generating_operation_operant() {
			return getRuleContext(Generating_operation_operantContext.class,0);
		}
		public Generating_operationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_generating_operation; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterGenerating_operation(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitGenerating_operation(this);
		}
	}

	public final Generating_operationContext generating_operation() throws RecognitionException {
		Generating_operationContext _localctx = new Generating_operationContext(_ctx, getState());
		enterRule(_localctx, 42, RULE_generating_operation);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(190);
			match(VARIABLE_TERM);
			setState(191);
			match(T__5);
			setState(192);
			generating_operation_variable();
			setState(193);
			generating_operators();
			setState(194);
			generating_operation_operant();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Generating_operation_variableContext extends ParserRuleContext {
		public TerminalNode VARIABLE_TERM() { return getToken(apollonParser.VARIABLE_TERM, 0); }
		public Generating_operation_variableContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_generating_operation_variable; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterGenerating_operation_variable(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitGenerating_operation_variable(this);
		}
	}

	public final Generating_operation_variableContext generating_operation_variable() throws RecognitionException {
		Generating_operation_variableContext _localctx = new Generating_operation_variableContext(_ctx, getState());
		enterRule(_localctx, 44, RULE_generating_operation_variable);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(196);
			match(VARIABLE_TERM);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Generating_operation_operantContext extends ParserRuleContext {
		public TerminalNode VARIABLE_TERM() { return getToken(apollonParser.VARIABLE_TERM, 0); }
		public TerminalNode NUMBER() { return getToken(apollonParser.NUMBER, 0); }
		public Generating_operation_operantContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_generating_operation_operant; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterGenerating_operation_operant(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitGenerating_operation_operant(this);
		}
	}

	public final Generating_operation_operantContext generating_operation_operant() throws RecognitionException {
		Generating_operation_operantContext _localctx = new Generating_operation_operantContext(_ctx, getState());
		enterRule(_localctx, 46, RULE_generating_operation_operant);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(198);
			_la = _input.LA(1);
			if ( !(_la==VARIABLE_TERM || _la==NUMBER) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Inline_operatorsContext extends ParserRuleContext {
		public TerminalNode EQUALS() { return getToken(apollonParser.EQUALS, 0); }
		public TerminalNode NOT_EQUALS() { return getToken(apollonParser.NOT_EQUALS, 0); }
		public TerminalNode LARGER() { return getToken(apollonParser.LARGER, 0); }
		public TerminalNode SMALLER() { return getToken(apollonParser.SMALLER, 0); }
		public TerminalNode LARGER_EQUALS() { return getToken(apollonParser.LARGER_EQUALS, 0); }
		public TerminalNode SMALLER_EQUALS() { return getToken(apollonParser.SMALLER_EQUALS, 0); }
		public Inline_operatorsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_inline_operators; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterInline_operators(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitInline_operators(this);
		}
	}

	public final Inline_operatorsContext inline_operators() throws RecognitionException {
		Inline_operatorsContext _localctx = new Inline_operatorsContext(_ctx, getState());
		enterRule(_localctx, 48, RULE_inline_operators);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(200);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 293888L) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Generating_operatorsContext extends ParserRuleContext {
		public TerminalNode PLUS() { return getToken(apollonParser.PLUS, 0); }
		public TerminalNode NEGATION() { return getToken(apollonParser.NEGATION, 0); }
		public TerminalNode TIMES() { return getToken(apollonParser.TIMES, 0); }
		public TerminalNode DIVIDE() { return getToken(apollonParser.DIVIDE, 0); }
		public Generating_operatorsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_generating_operators; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).enterGenerating_operators(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof apollonListener ) ((apollonListener)listener).exitGenerating_operators(this);
		}
	}

	public final Generating_operatorsContext generating_operators() throws RecognitionException {
		Generating_operatorsContext _localctx = new Generating_operatorsContext(_ctx, getState());
		enterRule(_localctx, 50, RULE_generating_operators);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(202);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 229888L) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static final String _serializedATN =
		"\u0004\u0001\u0018\u00cd\u0002\u0000\u0007\u0000\u0002\u0001\u0007\u0001"+
		"\u0002\u0002\u0007\u0002\u0002\u0003\u0007\u0003\u0002\u0004\u0007\u0004"+
		"\u0002\u0005\u0007\u0005\u0002\u0006\u0007\u0006\u0002\u0007\u0007\u0007"+
		"\u0002\b\u0007\b\u0002\t\u0007\t\u0002\n\u0007\n\u0002\u000b\u0007\u000b"+
		"\u0002\f\u0007\f\u0002\r\u0007\r\u0002\u000e\u0007\u000e\u0002\u000f\u0007"+
		"\u000f\u0002\u0010\u0007\u0010\u0002\u0011\u0007\u0011\u0002\u0012\u0007"+
		"\u0012\u0002\u0013\u0007\u0013\u0002\u0014\u0007\u0014\u0002\u0015\u0007"+
		"\u0015\u0002\u0016\u0007\u0016\u0002\u0017\u0007\u0017\u0002\u0018\u0007"+
		"\u0018\u0002\u0019\u0007\u0019\u0001\u0000\u0005\u00006\b\u0000\n\u0000"+
		"\f\u00009\t\u0000\u0001\u0000\u0001\u0000\u0001\u0001\u0001\u0001\u0001"+
		"\u0001\u0005\u0001@\b\u0001\n\u0001\f\u0001C\t\u0001\u0001\u0001\u0001"+
		"\u0001\u0001\u0001\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0001"+
		"\u0002\u0003\u0002M\b\u0002\u0001\u0003\u0001\u0003\u0001\u0003\u0001"+
		"\u0004\u0001\u0004\u0001\u0004\u0001\u0004\u0001\u0004\u0001\u0005\u0001"+
		"\u0005\u0001\u0005\u0001\u0005\u0005\u0005[\b\u0005\n\u0005\f\u0005^\t"+
		"\u0005\u0001\u0005\u0001\u0005\u0001\u0006\u0001\u0006\u0001\u0007\u0001"+
		"\u0007\u0001\u0007\u0005\u0007g\b\u0007\n\u0007\f\u0007j\t\u0007\u0001"+
		"\b\u0001\b\u0001\b\u0003\bo\b\b\u0001\t\u0003\tr\b\t\u0001\t\u0001\t\u0001"+
		"\n\u0001\n\u0001\n\u0001\n\u0001\u000b\u0004\u000b{\b\u000b\u000b\u000b"+
		"\f\u000b|\u0001\f\u0001\f\u0003\f\u0081\b\f\u0001\r\u0001\r\u0003\r\u0085"+
		"\b\r\u0001\u000e\u0001\u000e\u0001\u000e\u0003\u000e\u008a\b\u000e\u0001"+
		"\u000e\u0001\u000e\u0001\u000f\u0003\u000f\u008f\b\u000f\u0001\u000f\u0001"+
		"\u000f\u0001\u000f\u0001\u000f\u0001\u000f\u0005\u000f\u0096\b\u000f\n"+
		"\u000f\f\u000f\u0099\t\u000f\u0003\u000f\u009b\b\u000f\u0001\u000f\u0003"+
		"\u000f\u009e\b\u000f\u0001\u0010\u0003\u0010\u00a1\b\u0010\u0001\u0010"+
		"\u0001\u0010\u0001\u0011\u0001\u0011\u0001\u0011\u0001\u0011\u0001\u0011"+
		"\u0005\u0011\u00aa\b\u0011\n\u0011\f\u0011\u00ad\t\u0011\u0003\u0011\u00af"+
		"\b\u0011\u0001\u0011\u0003\u0011\u00b2\b\u0011\u0001\u0012\u0001\u0012"+
		"\u0001\u0012\u0003\u0012\u00b7\b\u0012\u0001\u0013\u0001\u0013\u0001\u0014"+
		"\u0001\u0014\u0001\u0014\u0001\u0014\u0001\u0015\u0001\u0015\u0001\u0015"+
		"\u0001\u0015\u0001\u0015\u0001\u0015\u0001\u0016\u0001\u0016\u0001\u0017"+
		"\u0001\u0017\u0001\u0018\u0001\u0018\u0001\u0019\u0001\u0019\u0001\u0019"+
		"\u0000\u0000\u001a\u0000\u0002\u0004\u0006\b\n\f\u000e\u0010\u0012\u0014"+
		"\u0016\u0018\u001a\u001c\u001e \"$&(*,.02\u0000\u0004\u0001\u0000\u0015"+
		"\u0016\u0002\u0000\u0016\u0016\u0018\u0018\u0002\u0000\n\u000e\u0012\u0012"+
		"\u0002\u0000\t\t\u000f\u0011\u00cb\u00007\u0001\u0000\u0000\u0000\u0002"+
		"<\u0001\u0000\u0000\u0000\u0004L\u0001\u0000\u0000\u0000\u0006N\u0001"+
		"\u0000\u0000\u0000\bQ\u0001\u0000\u0000\u0000\nV\u0001\u0000\u0000\u0000"+
		"\fa\u0001\u0000\u0000\u0000\u000ec\u0001\u0000\u0000\u0000\u0010n\u0001"+
		"\u0000\u0000\u0000\u0012q\u0001\u0000\u0000\u0000\u0014u\u0001\u0000\u0000"+
		"\u0000\u0016z\u0001\u0000\u0000\u0000\u0018\u0080\u0001\u0000\u0000\u0000"+
		"\u001a\u0084\u0001\u0000\u0000\u0000\u001c\u0086\u0001\u0000\u0000\u0000"+
		"\u001e\u008e\u0001\u0000\u0000\u0000 \u00a0\u0001\u0000\u0000\u0000\""+
		"\u00a4\u0001\u0000\u0000\u0000$\u00b6\u0001\u0000\u0000\u0000&\u00b8\u0001"+
		"\u0000\u0000\u0000(\u00ba\u0001\u0000\u0000\u0000*\u00be\u0001\u0000\u0000"+
		"\u0000,\u00c4\u0001\u0000\u0000\u0000.\u00c6\u0001\u0000\u0000\u00000"+
		"\u00c8\u0001\u0000\u0000\u00002\u00ca\u0001\u0000\u0000\u000046\u0003"+
		"\u0004\u0002\u000054\u0001\u0000\u0000\u000069\u0001\u0000\u0000\u0000"+
		"75\u0001\u0000\u0000\u000078\u0001\u0000\u0000\u00008:\u0001\u0000\u0000"+
		"\u000097\u0001\u0000\u0000\u0000:;\u0005\u0000\u0000\u0001;\u0001\u0001"+
		"\u0000\u0000\u0000<A\u0003\u0010\b\u0000=>\u0005\u0001\u0000\u0000>@\u0003"+
		"\u0010\b\u0000?=\u0001\u0000\u0000\u0000@C\u0001\u0000\u0000\u0000A?\u0001"+
		"\u0000\u0000\u0000AB\u0001\u0000\u0000\u0000BD\u0001\u0000\u0000\u0000"+
		"CA\u0001\u0000\u0000\u0000DE\u0005\u0002\u0000\u0000EF\u0005\u0000\u0000"+
		"\u0001F\u0003\u0001\u0000\u0000\u0000GM\u0003\u0006\u0003\u0000HM\u0003"+
		"\b\u0004\u0000IM\u0003\n\u0005\u0000JM\u0003\u001c\u000e\u0000KM\u0005"+
		"\u0013\u0000\u0000LG\u0001\u0000\u0000\u0000LH\u0001\u0000\u0000\u0000"+
		"LI\u0001\u0000\u0000\u0000LJ\u0001\u0000\u0000\u0000LK\u0001\u0000\u0000"+
		"\u0000M\u0005\u0001\u0000\u0000\u0000NO\u0003\u0012\t\u0000OP\u0005\u0002"+
		"\u0000\u0000P\u0007\u0001\u0000\u0000\u0000QR\u0003\f\u0006\u0000RS\u0005"+
		"\u0003\u0000\u0000ST\u0003\u000e\u0007\u0000TU\u0005\u0002\u0000\u0000"+
		"U\t\u0001\u0000\u0000\u0000VW\u0005\u0003\u0000\u0000W\\\u0003 \u0010"+
		"\u0000XY\u0005\u0001\u0000\u0000Y[\u0003 \u0010\u0000ZX\u0001\u0000\u0000"+
		"\u0000[^\u0001\u0000\u0000\u0000\\Z\u0001\u0000\u0000\u0000\\]\u0001\u0000"+
		"\u0000\u0000]_\u0001\u0000\u0000\u0000^\\\u0001\u0000\u0000\u0000_`\u0005"+
		"\u0002\u0000\u0000`\u000b\u0001\u0000\u0000\u0000ab\u0003\u0012\t\u0000"+
		"b\r\u0001\u0000\u0000\u0000ch\u0003\u0010\b\u0000de\u0005\u0001\u0000"+
		"\u0000eg\u0003\u0010\b\u0000fd\u0001\u0000\u0000\u0000gj\u0001\u0000\u0000"+
		"\u0000hf\u0001\u0000\u0000\u0000hi\u0001\u0000\u0000\u0000i\u000f\u0001"+
		"\u0000\u0000\u0000jh\u0001\u0000\u0000\u0000ko\u0003(\u0014\u0000lo\u0003"+
		" \u0010\u0000mo\u0003*\u0015\u0000nk\u0001\u0000\u0000\u0000nl\u0001\u0000"+
		"\u0000\u0000nm\u0001\u0000\u0000\u0000o\u0011\u0001\u0000\u0000\u0000"+
		"pr\u0005\t\u0000\u0000qp\u0001\u0000\u0000\u0000qr\u0001\u0000\u0000\u0000"+
		"rs\u0001\u0000\u0000\u0000st\u0003\"\u0011\u0000t\u0013\u0001\u0000\u0000"+
		"\u0000uv\u0005\u0004\u0000\u0000vw\u0005\u0016\u0000\u0000wx\u0005\u0005"+
		"\u0000\u0000x\u0015\u0001\u0000\u0000\u0000y{\u0003\u0018\f\u0000zy\u0001"+
		"\u0000\u0000\u0000{|\u0001\u0000\u0000\u0000|z\u0001\u0000\u0000\u0000"+
		"|}\u0001\u0000\u0000\u0000}\u0017\u0001\u0000\u0000\u0000~\u0081\u0003"+
		"\u001a\r\u0000\u007f\u0081\u0003\u0014\n\u0000\u0080~\u0001\u0000\u0000"+
		"\u0000\u0080\u007f\u0001\u0000\u0000\u0000\u0081\u0019\u0001\u0000\u0000"+
		"\u0000\u0082\u0085\u0003&\u0013\u0000\u0083\u0085\u0005\u0006\u0000\u0000"+
		"\u0084\u0082\u0001\u0000\u0000\u0000\u0084\u0083\u0001\u0000\u0000\u0000"+
		"\u0085\u001b\u0001\u0000\u0000\u0000\u0086\u0089\u0003\u001e\u000f\u0000"+
		"\u0087\u0088\u0005\u0017\u0000\u0000\u0088\u008a\u0003\u0016\u000b\u0000"+
		"\u0089\u0087\u0001\u0000\u0000\u0000\u0089\u008a\u0001\u0000\u0000\u0000"+
		"\u008a\u008b\u0001\u0000\u0000\u0000\u008b\u008c\u0005\u0002\u0000\u0000"+
		"\u008c\u001d\u0001\u0000\u0000\u0000\u008d\u008f\u0005\t\u0000\u0000\u008e"+
		"\u008d\u0001\u0000\u0000\u0000\u008e\u008f\u0001\u0000\u0000\u0000\u008f"+
		"\u0090\u0001\u0000\u0000\u0000\u0090\u009d\u0005\u0015\u0000\u0000\u0091"+
		"\u009a\u0005\u0007\u0000\u0000\u0092\u0097\u0005\u0016\u0000\u0000\u0093"+
		"\u0094\u0005\u0001\u0000\u0000\u0094\u0096\u0005\u0016\u0000\u0000\u0095"+
		"\u0093\u0001\u0000\u0000\u0000\u0096\u0099\u0001\u0000\u0000\u0000\u0097"+
		"\u0095\u0001\u0000\u0000\u0000\u0097\u0098\u0001\u0000\u0000\u0000\u0098"+
		"\u009b\u0001\u0000\u0000\u0000\u0099\u0097\u0001\u0000\u0000\u0000\u009a"+
		"\u0092\u0001\u0000\u0000\u0000\u009a\u009b\u0001\u0000\u0000\u0000\u009b"+
		"\u009c\u0001\u0000\u0000\u0000\u009c\u009e\u0005\u0005\u0000\u0000\u009d"+
		"\u0091\u0001\u0000\u0000\u0000\u009d\u009e\u0001\u0000\u0000\u0000\u009e"+
		"\u001f\u0001\u0000\u0000\u0000\u009f\u00a1\u0005\b\u0000\u0000\u00a0\u009f"+
		"\u0001\u0000\u0000\u0000\u00a0\u00a1\u0001\u0000\u0000\u0000\u00a1\u00a2"+
		"\u0001\u0000\u0000\u0000\u00a2\u00a3\u0003\u0012\t\u0000\u00a3!\u0001"+
		"\u0000\u0000\u0000\u00a4\u00b1\u0005\u0015\u0000\u0000\u00a5\u00ae\u0005"+
		"\u0007\u0000\u0000\u00a6\u00ab\u0003$\u0012\u0000\u00a7\u00a8\u0005\u0001"+
		"\u0000\u0000\u00a8\u00aa\u0003$\u0012\u0000\u00a9\u00a7\u0001\u0000\u0000"+
		"\u0000\u00aa\u00ad\u0001\u0000\u0000\u0000\u00ab\u00a9\u0001\u0000\u0000"+
		"\u0000\u00ab\u00ac\u0001\u0000\u0000\u0000\u00ac\u00af\u0001\u0000\u0000"+
		"\u0000\u00ad\u00ab\u0001\u0000\u0000\u0000\u00ae\u00a6\u0001\u0000\u0000"+
		"\u0000\u00ae\u00af\u0001\u0000\u0000\u0000\u00af\u00b0\u0001\u0000\u0000"+
		"\u0000\u00b0\u00b2\u0005\u0005\u0000\u0000\u00b1\u00a5\u0001\u0000\u0000"+
		"\u0000\u00b1\u00b2\u0001\u0000\u0000\u0000\u00b2#\u0001\u0000\u0000\u0000"+
		"\u00b3\u00b7\u0003&\u0013\u0000\u00b4\u00b7\u0005\u0018\u0000\u0000\u00b5"+
		"\u00b7\u0003\u0012\t\u0000\u00b6\u00b3\u0001\u0000\u0000\u0000\u00b6\u00b4"+
		"\u0001\u0000\u0000\u0000\u00b6\u00b5\u0001\u0000\u0000\u0000\u00b7%\u0001"+
		"\u0000\u0000\u0000\u00b8\u00b9\u0007\u0000\u0000\u0000\u00b9\'\u0001\u0000"+
		"\u0000\u0000\u00ba\u00bb\u0005\u0016\u0000\u0000\u00bb\u00bc\u00030\u0018"+
		"\u0000\u00bc\u00bd\u0003$\u0012\u0000\u00bd)\u0001\u0000\u0000\u0000\u00be"+
		"\u00bf\u0005\u0016\u0000\u0000\u00bf\u00c0\u0005\u0006\u0000\u0000\u00c0"+
		"\u00c1\u0003,\u0016\u0000\u00c1\u00c2\u00032\u0019\u0000\u00c2\u00c3\u0003"+
		".\u0017\u0000\u00c3+\u0001\u0000\u0000\u0000\u00c4\u00c5\u0005\u0016\u0000"+
		"\u0000\u00c5-\u0001\u0000\u0000\u0000\u00c6\u00c7\u0007\u0001\u0000\u0000"+
		"\u00c7/\u0001\u0000\u0000\u0000\u00c8\u00c9\u0007\u0002\u0000\u0000\u00c9"+
		"1\u0001\u0000\u0000\u0000\u00ca\u00cb\u0007\u0003\u0000\u0000\u00cb3\u0001"+
		"\u0000\u0000\u0000\u00147AL\\hnq|\u0080\u0084\u0089\u008e\u0097\u009a"+
		"\u009d\u00a0\u00ab\u00ae\u00b1\u00b6";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}