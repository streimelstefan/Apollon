// Generated from c://Users//strei//Documents//dev//Apollon//Parser//antlr//apollon.g4 by ANTLR 4.13.1
import org.antlr.v4.runtime.Lexer;
import org.antlr.v4.runtime.CharStream;
import org.antlr.v4.runtime.Token;
import org.antlr.v4.runtime.TokenStream;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.misc.*;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast", "CheckReturnValue", "this-escape"})
public class apollonLexer extends Lexer {
	static { RuntimeMetaData.checkVersion("4.13.1", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, NAF=8, NEGATION=9, 
		EQUALS=10, LARGER=11, SMALLER=12, LARGER_EQUALS=13, SMALLER_EQUALS=14, 
		PLUS=15, TIMES=16, DIVIDE=17, NOT_EQUALS=18, COMMENT=19, WS=20, CLASICAL_TERM=21, 
		VARIABLE_TERM=22, DOKU_SEPERATOR=23, NUMBER=24;
	public static String[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static String[] modeNames = {
		"DEFAULT_MODE"
	};

	private static String[] makeRuleNames() {
		return new String[] {
			"T__0", "T__1", "T__2", "T__3", "T__4", "T__5", "T__6", "NAF", "NEGATION", 
			"EQUALS", "LARGER", "SMALLER", "LARGER_EQUALS", "SMALLER_EQUALS", "PLUS", 
			"TIMES", "DIVIDE", "NOT_EQUALS", "COMMENT", "WS", "CLASICAL_TERM", "VARIABLE_TERM", 
			"DOKU_SEPERATOR", "NUMBER"
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


	public apollonLexer(CharStream input) {
		super(input);
		_interp = new LexerATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@Override
	public String getGrammarFileName() { return "apollon.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public String[] getChannelNames() { return channelNames; }

	@Override
	public String[] getModeNames() { return modeNames; }

	@Override
	public ATN getATN() { return _ATN; }

	public static final String _serializedATN =
		"\u0004\u0000\u0018\u0083\u0006\uffff\uffff\u0002\u0000\u0007\u0000\u0002"+
		"\u0001\u0007\u0001\u0002\u0002\u0007\u0002\u0002\u0003\u0007\u0003\u0002"+
		"\u0004\u0007\u0004\u0002\u0005\u0007\u0005\u0002\u0006\u0007\u0006\u0002"+
		"\u0007\u0007\u0007\u0002\b\u0007\b\u0002\t\u0007\t\u0002\n\u0007\n\u0002"+
		"\u000b\u0007\u000b\u0002\f\u0007\f\u0002\r\u0007\r\u0002\u000e\u0007\u000e"+
		"\u0002\u000f\u0007\u000f\u0002\u0010\u0007\u0010\u0002\u0011\u0007\u0011"+
		"\u0002\u0012\u0007\u0012\u0002\u0013\u0007\u0013\u0002\u0014\u0007\u0014"+
		"\u0002\u0015\u0007\u0015\u0002\u0016\u0007\u0016\u0002\u0017\u0007\u0017"+
		"\u0001\u0000\u0001\u0000\u0001\u0001\u0001\u0001\u0001\u0002\u0001\u0002"+
		"\u0001\u0002\u0001\u0003\u0001\u0003\u0001\u0003\u0001\u0004\u0001\u0004"+
		"\u0001\u0005\u0001\u0005\u0001\u0005\u0001\u0006\u0001\u0006\u0001\u0007"+
		"\u0001\u0007\u0001\u0007\u0001\u0007\u0001\b\u0001\b\u0001\t\u0001\t\u0001"+
		"\n\u0001\n\u0001\u000b\u0001\u000b\u0001\f\u0001\f\u0001\f\u0001\r\u0001"+
		"\r\u0001\r\u0001\u000e\u0001\u000e\u0001\u000f\u0001\u000f\u0001\u0010"+
		"\u0001\u0010\u0001\u0011\u0001\u0011\u0001\u0011\u0001\u0012\u0001\u0012"+
		"\u0005\u0012`\b\u0012\n\u0012\f\u0012c\t\u0012\u0001\u0012\u0001\u0012"+
		"\u0001\u0013\u0004\u0013h\b\u0013\u000b\u0013\f\u0013i\u0001\u0013\u0001"+
		"\u0013\u0001\u0014\u0001\u0014\u0005\u0014p\b\u0014\n\u0014\f\u0014s\t"+
		"\u0014\u0001\u0015\u0001\u0015\u0005\u0015w\b\u0015\n\u0015\f\u0015z\t"+
		"\u0015\u0001\u0016\u0001\u0016\u0001\u0016\u0001\u0017\u0004\u0017\u0080"+
		"\b\u0017\u000b\u0017\f\u0017\u0081\u0000\u0000\u0018\u0001\u0001\u0003"+
		"\u0002\u0005\u0003\u0007\u0004\t\u0005\u000b\u0006\r\u0007\u000f\b\u0011"+
		"\t\u0013\n\u0015\u000b\u0017\f\u0019\r\u001b\u000e\u001d\u000f\u001f\u0010"+
		"!\u0011#\u0012%\u0013\'\u0014)\u0015+\u0016-\u0017/\u0018\u0001\u0000"+
		"\u0006\u0002\u0000\n\n\r\r\u0003\u0000\t\n\r\r  \u0001\u0000az\u0004\u0000"+
		"09AZ__az\u0001\u0000AZ\u0001\u000009\u0087\u0000\u0001\u0001\u0000\u0000"+
		"\u0000\u0000\u0003\u0001\u0000\u0000\u0000\u0000\u0005\u0001\u0000\u0000"+
		"\u0000\u0000\u0007\u0001\u0000\u0000\u0000\u0000\t\u0001\u0000\u0000\u0000"+
		"\u0000\u000b\u0001\u0000\u0000\u0000\u0000\r\u0001\u0000\u0000\u0000\u0000"+
		"\u000f\u0001\u0000\u0000\u0000\u0000\u0011\u0001\u0000\u0000\u0000\u0000"+
		"\u0013\u0001\u0000\u0000\u0000\u0000\u0015\u0001\u0000\u0000\u0000\u0000"+
		"\u0017\u0001\u0000\u0000\u0000\u0000\u0019\u0001\u0000\u0000\u0000\u0000"+
		"\u001b\u0001\u0000\u0000\u0000\u0000\u001d\u0001\u0000\u0000\u0000\u0000"+
		"\u001f\u0001\u0000\u0000\u0000\u0000!\u0001\u0000\u0000\u0000\u0000#\u0001"+
		"\u0000\u0000\u0000\u0000%\u0001\u0000\u0000\u0000\u0000\'\u0001\u0000"+
		"\u0000\u0000\u0000)\u0001\u0000\u0000\u0000\u0000+\u0001\u0000\u0000\u0000"+
		"\u0000-\u0001\u0000\u0000\u0000\u0000/\u0001\u0000\u0000\u0000\u00011"+
		"\u0001\u0000\u0000\u0000\u00033\u0001\u0000\u0000\u0000\u00055\u0001\u0000"+
		"\u0000\u0000\u00078\u0001\u0000\u0000\u0000\t;\u0001\u0000\u0000\u0000"+
		"\u000b=\u0001\u0000\u0000\u0000\r@\u0001\u0000\u0000\u0000\u000fB\u0001"+
		"\u0000\u0000\u0000\u0011F\u0001\u0000\u0000\u0000\u0013H\u0001\u0000\u0000"+
		"\u0000\u0015J\u0001\u0000\u0000\u0000\u0017L\u0001\u0000\u0000\u0000\u0019"+
		"N\u0001\u0000\u0000\u0000\u001bQ\u0001\u0000\u0000\u0000\u001dT\u0001"+
		"\u0000\u0000\u0000\u001fV\u0001\u0000\u0000\u0000!X\u0001\u0000\u0000"+
		"\u0000#Z\u0001\u0000\u0000\u0000%]\u0001\u0000\u0000\u0000\'g\u0001\u0000"+
		"\u0000\u0000)m\u0001\u0000\u0000\u0000+t\u0001\u0000\u0000\u0000-{\u0001"+
		"\u0000\u0000\u0000/\u007f\u0001\u0000\u0000\u000012\u0005,\u0000\u0000"+
		"2\u0002\u0001\u0000\u0000\u000034\u0005.\u0000\u00004\u0004\u0001\u0000"+
		"\u0000\u000056\u0005:\u0000\u000067\u0005-\u0000\u00007\u0006\u0001\u0000"+
		"\u0000\u000089\u0005@\u0000\u00009:\u0005(\u0000\u0000:\b\u0001\u0000"+
		"\u0000\u0000;<\u0005)\u0000\u0000<\n\u0001\u0000\u0000\u0000=>\u0005i"+
		"\u0000\u0000>?\u0005s\u0000\u0000?\f\u0001\u0000\u0000\u0000@A\u0005("+
		"\u0000\u0000A\u000e\u0001\u0000\u0000\u0000BC\u0005n\u0000\u0000CD\u0005"+
		"o\u0000\u0000DE\u0005t\u0000\u0000E\u0010\u0001\u0000\u0000\u0000FG\u0005"+
		"-\u0000\u0000G\u0012\u0001\u0000\u0000\u0000HI\u0005=\u0000\u0000I\u0014"+
		"\u0001\u0000\u0000\u0000JK\u0005>\u0000\u0000K\u0016\u0001\u0000\u0000"+
		"\u0000LM\u0005<\u0000\u0000M\u0018\u0001\u0000\u0000\u0000NO\u0005>\u0000"+
		"\u0000OP\u0005=\u0000\u0000P\u001a\u0001\u0000\u0000\u0000QR\u0005<\u0000"+
		"\u0000RS\u0005=\u0000\u0000S\u001c\u0001\u0000\u0000\u0000TU\u0005+\u0000"+
		"\u0000U\u001e\u0001\u0000\u0000\u0000VW\u0005*\u0000\u0000W \u0001\u0000"+
		"\u0000\u0000XY\u0005/\u0000\u0000Y\"\u0001\u0000\u0000\u0000Z[\u0005!"+
		"\u0000\u0000[\\\u0005=\u0000\u0000\\$\u0001\u0000\u0000\u0000]a\u0005"+
		"%\u0000\u0000^`\b\u0000\u0000\u0000_^\u0001\u0000\u0000\u0000`c\u0001"+
		"\u0000\u0000\u0000a_\u0001\u0000\u0000\u0000ab\u0001\u0000\u0000\u0000"+
		"bd\u0001\u0000\u0000\u0000ca\u0001\u0000\u0000\u0000de\u0006\u0012\u0000"+
		"\u0000e&\u0001\u0000\u0000\u0000fh\u0007\u0001\u0000\u0000gf\u0001\u0000"+
		"\u0000\u0000hi\u0001\u0000\u0000\u0000ig\u0001\u0000\u0000\u0000ij\u0001"+
		"\u0000\u0000\u0000jk\u0001\u0000\u0000\u0000kl\u0006\u0013\u0000\u0000"+
		"l(\u0001\u0000\u0000\u0000mq\u0007\u0002\u0000\u0000np\u0007\u0003\u0000"+
		"\u0000on\u0001\u0000\u0000\u0000ps\u0001\u0000\u0000\u0000qo\u0001\u0000"+
		"\u0000\u0000qr\u0001\u0000\u0000\u0000r*\u0001\u0000\u0000\u0000sq\u0001"+
		"\u0000\u0000\u0000tx\u0007\u0004\u0000\u0000uw\u0007\u0003\u0000\u0000"+
		"vu\u0001\u0000\u0000\u0000wz\u0001\u0000\u0000\u0000xv\u0001\u0000\u0000"+
		"\u0000xy\u0001\u0000\u0000\u0000y,\u0001\u0000\u0000\u0000zx\u0001\u0000"+
		"\u0000\u0000{|\u0005:\u0000\u0000|}\u0005:\u0000\u0000}.\u0001\u0000\u0000"+
		"\u0000~\u0080\u0007\u0005\u0000\u0000\u007f~\u0001\u0000\u0000\u0000\u0080"+
		"\u0081\u0001\u0000\u0000\u0000\u0081\u007f\u0001\u0000\u0000\u0000\u0081"+
		"\u0082\u0001\u0000\u0000\u0000\u00820\u0001\u0000\u0000\u0000\u0006\u0000"+
		"aiqx\u0081\u0001\u0006\u0000\u0000";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}