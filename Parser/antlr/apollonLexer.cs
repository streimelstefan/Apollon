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
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public partial class apollonLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, NAF=6, NEGATION=7, COMMENT=8, 
		WS=9, CLASICAL_TERM=10, VARIABLE_TERM=11;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"T__0", "T__1", "T__2", "T__3", "T__4", "NAF", "NEGATION", "COMMENT", 
		"WS", "CLASICAL_TERM", "VARIABLE_TERM"
	};


	public apollonLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public apollonLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'.'", "':-'", "','", "'('", "')'", "'not'", "'-'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, "NAF", "NEGATION", "COMMENT", "WS", 
		"CLASICAL_TERM", "VARIABLE_TERM"
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

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static apollonLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,11,70,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,6,
		2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,1,0,1,0,1,1,1,1,1,1,1,2,1,2,1,3,1,3,
		1,4,1,4,1,5,1,5,1,5,1,5,1,6,1,6,1,7,1,7,5,7,43,8,7,10,7,12,7,46,9,7,1,
		7,1,7,1,8,4,8,51,8,8,11,8,12,8,52,1,8,1,8,1,9,1,9,5,9,59,8,9,10,9,12,9,
		62,9,9,1,10,1,10,5,10,66,8,10,10,10,12,10,69,9,10,0,0,11,1,1,3,2,5,3,7,
		4,9,5,11,6,13,7,15,8,17,9,19,10,21,11,1,0,5,2,0,10,10,13,13,3,0,9,10,13,
		13,32,32,1,0,97,122,4,0,48,57,65,90,95,95,97,122,1,0,65,90,73,0,1,1,0,
		0,0,0,3,1,0,0,0,0,5,1,0,0,0,0,7,1,0,0,0,0,9,1,0,0,0,0,11,1,0,0,0,0,13,
		1,0,0,0,0,15,1,0,0,0,0,17,1,0,0,0,0,19,1,0,0,0,0,21,1,0,0,0,1,23,1,0,0,
		0,3,25,1,0,0,0,5,28,1,0,0,0,7,30,1,0,0,0,9,32,1,0,0,0,11,34,1,0,0,0,13,
		38,1,0,0,0,15,40,1,0,0,0,17,50,1,0,0,0,19,56,1,0,0,0,21,63,1,0,0,0,23,
		24,5,46,0,0,24,2,1,0,0,0,25,26,5,58,0,0,26,27,5,45,0,0,27,4,1,0,0,0,28,
		29,5,44,0,0,29,6,1,0,0,0,30,31,5,40,0,0,31,8,1,0,0,0,32,33,5,41,0,0,33,
		10,1,0,0,0,34,35,5,110,0,0,35,36,5,111,0,0,36,37,5,116,0,0,37,12,1,0,0,
		0,38,39,5,45,0,0,39,14,1,0,0,0,40,44,5,37,0,0,41,43,8,0,0,0,42,41,1,0,
		0,0,43,46,1,0,0,0,44,42,1,0,0,0,44,45,1,0,0,0,45,47,1,0,0,0,46,44,1,0,
		0,0,47,48,6,7,0,0,48,16,1,0,0,0,49,51,7,1,0,0,50,49,1,0,0,0,51,52,1,0,
		0,0,52,50,1,0,0,0,52,53,1,0,0,0,53,54,1,0,0,0,54,55,6,8,0,0,55,18,1,0,
		0,0,56,60,7,2,0,0,57,59,7,3,0,0,58,57,1,0,0,0,59,62,1,0,0,0,60,58,1,0,
		0,0,60,61,1,0,0,0,61,20,1,0,0,0,62,60,1,0,0,0,63,67,7,4,0,0,64,66,7,3,
		0,0,65,64,1,0,0,0,66,69,1,0,0,0,67,65,1,0,0,0,67,68,1,0,0,0,68,22,1,0,
		0,0,69,67,1,0,0,0,5,0,44,52,60,67,1,6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
