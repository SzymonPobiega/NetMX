//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 3.4.1.9004
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// $ANTLR 3.4.1.9004 netmxpr.g 2012-03-23 06:57:19

// The variable 'variable' is assigned but its value is never used.
#pragma warning disable 168, 219
// Unreachable code detected.
#pragma warning disable 162
// Missing XML comment for publicly visible type or member 'Type_or_Member'
#pragma warning disable 1591


using System.Collections.Generic;
using Antlr.Runtime;
using Antlr.Runtime.Misc;
using ConditionalAttribute = System.Diagnostics.ConditionalAttribute;

namespace  NetMX.Expression 
{
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "3.4.1.9004")]
[System.CLSCompliant(false)]
public partial class netmxprLexer : Antlr.Runtime.Lexer
{
	public const int EOF=-1;
	public const int AND=4;
	public const int APOS=5;
	public const int COMMA=6;
	public const int DIV=7;
	public const int DOT=8;
	public const int Digits=9;
	public const int EQUALS=10;
	public const int EQUAL_OBJ=11;
	public const int FunctionName=12;
	public const int GE=13;
	public const int LE=14;
	public const int LESS=15;
	public const int LPAR=16;
	public const int Literal=17;
	public const int MINUS=18;
	public const int MORE=19;
	public const int MUL=20;
	public const int NOT=21;
	public const int Number=22;
	public const int OR=23;
	public const int PLUS=24;
	public const int QUOT=25;
	public const int RPAR=26;
	public const int Whitespace=27;

    // delegates
    // delegators

	public netmxprLexer()
	{
		OnCreated();
	}

	public netmxprLexer(ICharStream input )
		: this(input, new RecognizerSharedState())
	{
	}

	public netmxprLexer(ICharStream input, RecognizerSharedState state)
		: base(input, state)
	{

		OnCreated();
	}
	public override string GrammarFileName { get { return "netmxpr.g"; } }

	private static readonly bool[] decisionCanBacktrack = new bool[0];

	[Conditional("ANTLR_TRACE")]
	protected virtual void OnCreated() {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void EnterRule(string ruleName, int ruleIndex) {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void LeaveRule(string ruleName, int ruleIndex) {}

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_AND() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_AND() {}

    // $ANTLR start "AND"
    [GrammarRule("AND")]
    private void mAND()
    {
    	EnterRule_AND();
    	EnterRule("AND", 1);
    	TraceIn("AND", 1);
    		try
    		{
    		int _type = AND;
    		int _channel = DefaultTokenChannel;
    		// netmxpr.g:9:5: ( 'and' )
    		DebugEnterAlt(1);
    		// netmxpr.g:9:7: 'and'
    		{
    		DebugLocation(9, 7);
    		Match("and"); 


    		}

    		state.type = _type;
    		state.channel = _channel;
    	}
    	finally
    	{
    		TraceOut("AND", 1);
    		LeaveRule("AND", 1);
    		LeaveRule_AND();
        }
    }
    // $ANTLR end "AND"

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_APOS() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_APOS() {}

    // $ANTLR start "APOS"
    [GrammarRule("APOS")]
    private void mAPOS()
    {
    	EnterRule_APOS();
    	EnterRule("APOS", 2);
    	TraceIn("APOS", 2);
    		try
    		{
    		int _type = APOS;
    		int _channel = DefaultTokenChannel;
    		// netmxpr.g:10:6: ( '\\'' )
    		DebugEnterAlt(1);
    		// netmxpr.g:10:8: '\\''
    		{
    		DebugLocation(10, 8);
    		Match('\''); 

    		}

    		state.type = _type;
    		state.channel = _channel;
    	}
    	finally
    	{
    		TraceOut("APOS", 2);
    		LeaveRule("APOS", 2);
    		LeaveRule_APOS();
        }
    }
    // $ANTLR end "APOS"

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_COMMA() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_COMMA() {}

    // $ANTLR start "COMMA"
    [GrammarRule("COMMA")]
    private void mCOMMA()
    {
    	EnterRule_COMMA();
    	EnterRule("COMMA", 3);
    	TraceIn("COMMA", 3);
    		try
    		{
    		int _type = COMMA;
    		int _channel = DefaultTokenChannel;
    		// netmxpr.g:11:7: ( ',' )
    		DebugEnterAlt(1);
    		// netmxpr.g:11:9: ','
    		{
    		DebugLocation(11, 9);
    		Match(','); 

    		}

    		state.type = _type;
    		state.channel = _channel;
    	}
    	finally
    	{
    		TraceOut("COMMA", 3);
    		LeaveRule("COMMA", 3);
    		LeaveRule_COMMA();
        }
    }
    // $ANTLR end "COMMA"

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_DIV() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_DIV() {}

    // $ANTLR start "DIV"
    [GrammarRule("DIV")]
    private void mDIV()
    {
    	EnterRule_DIV();
    	EnterRule("DIV", 4);
    	TraceIn("DIV", 4);
    		try
    		{
    		int _type = DIV;
    		int _channel = DefaultTokenChannel;
    		// netmxpr.g:12:5: ( '/' )
    		DebugEnterAlt(1);
    		// netmxpr.g:12:7: '/'
    		{
    		DebugLocation(12, 7);
    		Match('/'); 

    		}

    		state.type = _type;
    		state.channel = _channel;
    	}
    	finally
    	{
    		TraceOut("DIV", 4);
    		LeaveRule("DIV", 4);
    		LeaveRule_DIV();
        }
    }
    // $ANTLR end "DIV"

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_DOT() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_DOT() {}

    // $ANTLR start "DOT"
    [GrammarRule("DOT")]
    private void mDOT()
    {
    	EnterRule_DOT();
    	EnterRule("DOT", 5);
    	TraceIn("DOT", 5);
    		try
    		{
    		int _type = DOT;
    		int _channel = DefaultTokenChannel;
    		// netmxpr.g:13:5: ( '.' )
    		DebugEnterAlt(1);
    		// netmxpr.g:13:7: '.'
    		{
    		DebugLocation(13, 7);
    		Match('.'); 

    		}

    		state.type = _type;
    		state.channel = _channel;
    	}
    	finally
    	{
    		TraceOut("DOT", 5);
    		LeaveRule("DOT", 5);
    		LeaveRule_DOT();
        }
    }
    // $ANTLR end "DOT"

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_EQUALS() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_EQUALS() {}

    // $ANTLR start "EQUALS"
    [GrammarRule("EQUALS")]
    private void mEQUALS()
    {
    	EnterRule_EQUALS();
    	EnterRule("EQUALS", 6);
    	TraceIn("EQUALS", 6);
    		try
    		{
    		int _type = EQUALS;
    		int _channel = DefaultTokenChannel;
    		// netmxpr.g:14:8: ( '=' )
    		DebugEnterAlt(1);
    		// netmxpr.g:14:10: '='
    		{
    		DebugLocation(14, 10);
    		Match('='); 

    		}

    		state.type = _type;
    		state.channel = _channel;
    	}
    	finally
    	{
    		TraceOut("EQUALS", 6);
    		LeaveRule("EQUALS", 6);
    		LeaveRule_EQUALS();
        }
    }
    // $ANTLR end "EQUALS"

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_EQUAL_OBJ() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_EQUAL_OBJ() {}

    // $ANTLR start "EQUAL_OBJ"
    [GrammarRule("EQUAL_OBJ")]
    private void mEQUAL_OBJ()
    {
    	EnterRule_EQUAL_OBJ();
    	EnterRule("EQUAL_OBJ", 7);
    	TraceIn("EQUAL_OBJ", 7);
    		try
    		{
    		int _type = EQUAL_OBJ;
    		int _channel = DefaultTokenChannel;
    		// netmxpr.g:15:11: ( 'eq' )
    		DebugEnterAlt(1);
    		// netmxpr.g:15:13: 'eq'
    		{
    		DebugLocation(15, 13);
    		Match("eq"); 


    		}

    		state.type = _type;
    		state.channel = _channel;
    	}
    	finally
    	{
    		TraceOut("EQUAL_OBJ", 7);
    		LeaveRule("EQUAL_OBJ", 7);
    		LeaveRule_EQUAL_OBJ();
        }
    }
    // $ANTLR end "EQUAL_OBJ"

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_GE() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_GE() {}

    // $ANTLR start "GE"
    [GrammarRule("GE")]
    private void mGE()
    {
    	EnterRule_GE();
    	EnterRule("GE", 8);
    	TraceIn("GE", 8);
    		try
    		{
    		int _type = GE;
    		int _channel = DefaultTokenChannel;
    		// netmxpr.g:16:4: ( '>=' )
    		DebugEnterAlt(1);
    		// netmxpr.g:16:6: '>='
    		{
    		DebugLocation(16, 6);
    		Match(">="); 


    		}

    		state.type = _type;
    		state.channel = _channel;
    	}
    	finally
    	{
    		TraceOut("GE", 8);
    		LeaveRule("GE", 8);
    		LeaveRule_GE();
        }
    }
    // $ANTLR end "GE"

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_LE() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_LE() {}

    // $ANTLR start "LE"
    [GrammarRule("LE")]
    private void mLE()
    {
    	EnterRule_LE();
    	EnterRule("LE", 9);
    	TraceIn("LE", 9);
    		try
    		{
    		int _type = LE;
    		int _channel = DefaultTokenChannel;
    		// netmxpr.g:17:4: ( '<=' )
    		DebugEnterAlt(1);
    		// netmxpr.g:17:6: '<='
    		{
    		DebugLocation(17, 6);
    		Match("<="); 


    		}

    		state.type = _type;
    		state.channel = _channel;
    	}
    	finally
    	{
    		TraceOut("LE", 9);
    		LeaveRule("LE", 9);
    		LeaveRule_LE();
        }
    }
    // $ANTLR end "LE"

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_LESS() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_LESS() {}

    // $ANTLR start "LESS"
    [GrammarRule("LESS")]
    private void mLESS()
    {
    	EnterRule_LESS();
    	EnterRule("LESS", 10);
    	TraceIn("LESS", 10);
    		try
    		{
    		int _type = LESS;
    		int _channel = DefaultTokenChannel;
    		// netmxpr.g:18:6: ( '<' )
    		DebugEnterAlt(1);
    		// netmxpr.g:18:8: '<'
    		{
    		DebugLocation(18, 8);
    		Match('<'); 

    		}

    		state.type = _type;
    		state.channel = _channel;
    	}
    	finally
    	{
    		TraceOut("LESS", 10);
    		LeaveRule("LESS", 10);
    		LeaveRule_LESS();
        }
    }
    // $ANTLR end "LESS"

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_LPAR() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_LPAR() {}

    // $ANTLR start "LPAR"
    [GrammarRule("LPAR")]
    private void mLPAR()
    {
    	EnterRule_LPAR();
    	EnterRule("LPAR", 11);
    	TraceIn("LPAR", 11);
    		try
    		{
    		int _type = LPAR;
    		int _channel = DefaultTokenChannel;
    		// netmxpr.g:19:6: ( '(' )
    		DebugEnterAlt(1);
    		// netmxpr.g:19:8: '('
    		{
    		DebugLocation(19, 8);
    		Match('('); 

    		}

    		state.type = _type;
    		state.channel = _channel;
    	}
    	finally
    	{
    		TraceOut("LPAR", 11);
    		LeaveRule("LPAR", 11);
    		LeaveRule_LPAR();
        }
    }
    // $ANTLR end "LPAR"

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_MINUS() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_MINUS() {}

    // $ANTLR start "MINUS"
    [GrammarRule("MINUS")]
    private void mMINUS()
    {
    	EnterRule_MINUS();
    	EnterRule("MINUS", 12);
    	TraceIn("MINUS", 12);
    		try
    		{
    		int _type = MINUS;
    		int _channel = DefaultTokenChannel;
    		// netmxpr.g:20:7: ( '-' )
    		DebugEnterAlt(1);
    		// netmxpr.g:20:9: '-'
    		{
    		DebugLocation(20, 9);
    		Match('-'); 

    		}

    		state.type = _type;
    		state.channel = _channel;
    	}
    	finally
    	{
    		TraceOut("MINUS", 12);
    		LeaveRule("MINUS", 12);
    		LeaveRule_MINUS();
        }
    }
    // $ANTLR end "MINUS"

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_MORE() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_MORE() {}

    // $ANTLR start "MORE"
    [GrammarRule("MORE")]
    private void mMORE()
    {
    	EnterRule_MORE();
    	EnterRule("MORE", 13);
    	TraceIn("MORE", 13);
    		try
    		{
    		int _type = MORE;
    		int _channel = DefaultTokenChannel;
    		// netmxpr.g:21:6: ( '>' )
    		DebugEnterAlt(1);
    		// netmxpr.g:21:8: '>'
    		{
    		DebugLocation(21, 8);
    		Match('>'); 

    		}

    		state.type = _type;
    		state.channel = _channel;
    	}
    	finally
    	{
    		TraceOut("MORE", 13);
    		LeaveRule("MORE", 13);
    		LeaveRule_MORE();
        }
    }
    // $ANTLR end "MORE"

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_MUL() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_MUL() {}

    // $ANTLR start "MUL"
    [GrammarRule("MUL")]
    private void mMUL()
    {
    	EnterRule_MUL();
    	EnterRule("MUL", 14);
    	TraceIn("MUL", 14);
    		try
    		{
    		int _type = MUL;
    		int _channel = DefaultTokenChannel;
    		// netmxpr.g:22:5: ( '*' )
    		DebugEnterAlt(1);
    		// netmxpr.g:22:7: '*'
    		{
    		DebugLocation(22, 7);
    		Match('*'); 

    		}

    		state.type = _type;
    		state.channel = _channel;
    	}
    	finally
    	{
    		TraceOut("MUL", 14);
    		LeaveRule("MUL", 14);
    		LeaveRule_MUL();
        }
    }
    // $ANTLR end "MUL"

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_NOT() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_NOT() {}

    // $ANTLR start "NOT"
    [GrammarRule("NOT")]
    private void mNOT()
    {
    	EnterRule_NOT();
    	EnterRule("NOT", 15);
    	TraceIn("NOT", 15);
    		try
    		{
    		int _type = NOT;
    		int _channel = DefaultTokenChannel;
    		// netmxpr.g:23:5: ( '~' )
    		DebugEnterAlt(1);
    		// netmxpr.g:23:7: '~'
    		{
    		DebugLocation(23, 7);
    		Match('~'); 

    		}

    		state.type = _type;
    		state.channel = _channel;
    	}
    	finally
    	{
    		TraceOut("NOT", 15);
    		LeaveRule("NOT", 15);
    		LeaveRule_NOT();
        }
    }
    // $ANTLR end "NOT"

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_OR() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_OR() {}

    // $ANTLR start "OR"
    [GrammarRule("OR")]
    private void mOR()
    {
    	EnterRule_OR();
    	EnterRule("OR", 16);
    	TraceIn("OR", 16);
    		try
    		{
    		int _type = OR;
    		int _channel = DefaultTokenChannel;
    		// netmxpr.g:24:4: ( 'or' )
    		DebugEnterAlt(1);
    		// netmxpr.g:24:6: 'or'
    		{
    		DebugLocation(24, 6);
    		Match("or"); 


    		}

    		state.type = _type;
    		state.channel = _channel;
    	}
    	finally
    	{
    		TraceOut("OR", 16);
    		LeaveRule("OR", 16);
    		LeaveRule_OR();
        }
    }
    // $ANTLR end "OR"

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_PLUS() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_PLUS() {}

    // $ANTLR start "PLUS"
    [GrammarRule("PLUS")]
    private void mPLUS()
    {
    	EnterRule_PLUS();
    	EnterRule("PLUS", 17);
    	TraceIn("PLUS", 17);
    		try
    		{
    		int _type = PLUS;
    		int _channel = DefaultTokenChannel;
    		// netmxpr.g:25:6: ( '+' )
    		DebugEnterAlt(1);
    		// netmxpr.g:25:8: '+'
    		{
    		DebugLocation(25, 8);
    		Match('+'); 

    		}

    		state.type = _type;
    		state.channel = _channel;
    	}
    	finally
    	{
    		TraceOut("PLUS", 17);
    		LeaveRule("PLUS", 17);
    		LeaveRule_PLUS();
        }
    }
    // $ANTLR end "PLUS"

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_QUOT() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_QUOT() {}

    // $ANTLR start "QUOT"
    [GrammarRule("QUOT")]
    private void mQUOT()
    {
    	EnterRule_QUOT();
    	EnterRule("QUOT", 18);
    	TraceIn("QUOT", 18);
    		try
    		{
    		int _type = QUOT;
    		int _channel = DefaultTokenChannel;
    		// netmxpr.g:26:6: ( '\\\"' )
    		DebugEnterAlt(1);
    		// netmxpr.g:26:8: '\\\"'
    		{
    		DebugLocation(26, 8);
    		Match('\"'); 

    		}

    		state.type = _type;
    		state.channel = _channel;
    	}
    	finally
    	{
    		TraceOut("QUOT", 18);
    		LeaveRule("QUOT", 18);
    		LeaveRule_QUOT();
        }
    }
    // $ANTLR end "QUOT"

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_RPAR() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_RPAR() {}

    // $ANTLR start "RPAR"
    [GrammarRule("RPAR")]
    private void mRPAR()
    {
    	EnterRule_RPAR();
    	EnterRule("RPAR", 19);
    	TraceIn("RPAR", 19);
    		try
    		{
    		int _type = RPAR;
    		int _channel = DefaultTokenChannel;
    		// netmxpr.g:27:6: ( ')' )
    		DebugEnterAlt(1);
    		// netmxpr.g:27:8: ')'
    		{
    		DebugLocation(27, 8);
    		Match(')'); 

    		}

    		state.type = _type;
    		state.channel = _channel;
    	}
    	finally
    	{
    		TraceOut("RPAR", 19);
    		LeaveRule("RPAR", 19);
    		LeaveRule_RPAR();
        }
    }
    // $ANTLR end "RPAR"

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_FunctionName() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_FunctionName() {}

    // $ANTLR start "FunctionName"
    [GrammarRule("FunctionName")]
    private void mFunctionName()
    {
    	EnterRule_FunctionName();
    	EnterRule("FunctionName", 20);
    	TraceIn("FunctionName", 20);
    		try
    		{
    		int _type = FunctionName;
    		int _channel = DefaultTokenChannel;
    		// netmxpr.g:82:3: ( ( 'a' .. 'z' )+ )
    		DebugEnterAlt(1);
    		// netmxpr.g:82:6: ( 'a' .. 'z' )+
    		{
    		DebugLocation(82, 6);
    		// netmxpr.g:82:6: ( 'a' .. 'z' )+
    		int cnt1=0;
    		try { DebugEnterSubRule(1);
    		while (true)
    		{
    			int alt1=2;
    			try { DebugEnterDecision(1, decisionCanBacktrack[1]);
    			int LA1_1 = input.LA(1);

    			if (((LA1_1>='a' && LA1_1<='z')))
    			{
    				alt1 = 1;
    			}


    			} finally { DebugExitDecision(1); }
    			switch (alt1)
    			{
    			case 1:
    				DebugEnterAlt(1);
    				// netmxpr.g:
    				{
    				DebugLocation(82, 6);
    				input.Consume();


    				}
    				break;

    			default:
    				if (cnt1 >= 1)
    					goto loop1;

    				EarlyExitException eee1 = new EarlyExitException( 1, input );
    				DebugRecognitionException(eee1);
    				throw eee1;
    			}
    			cnt1++;
    		}
    		loop1:
    			;

    		} finally { DebugExitSubRule(1); }


    		}

    		state.type = _type;
    		state.channel = _channel;
    	}
    	finally
    	{
    		TraceOut("FunctionName", 20);
    		LeaveRule("FunctionName", 20);
    		LeaveRule_FunctionName();
        }
    }
    // $ANTLR end "FunctionName"

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_Number() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_Number() {}

    // $ANTLR start "Number"
    [GrammarRule("Number")]
    private void mNumber()
    {
    	EnterRule_Number();
    	EnterRule("Number", 21);
    	TraceIn("Number", 21);
    		try
    		{
    		int _type = Number;
    		int _channel = DefaultTokenChannel;
    		// netmxpr.g:85:9: ( Digits ( '.' ( Digits )? )? | '.' Digits )
    		int alt4=2;
    		try { DebugEnterDecision(4, decisionCanBacktrack[4]);
    		int LA4_1 = input.LA(1);

    		if (((LA4_1>='0' && LA4_1<='9')))
    		{
    			alt4 = 1;
    		}
    		else if ((LA4_1=='.'))
    		{
    			alt4 = 2;
    		}
    		else
    		{
    			NoViableAltException nvae = new NoViableAltException("", 4, 0, input, 1);
    			DebugRecognitionException(nvae);
    			throw nvae;
    		}
    		} finally { DebugExitDecision(4); }
    		switch (alt4)
    		{
    		case 1:
    			DebugEnterAlt(1);
    			// netmxpr.g:85:12: Digits ( '.' ( Digits )? )?
    			{
    			DebugLocation(85, 12);
    			mDigits(); 
    			DebugLocation(85, 19);
    			// netmxpr.g:85:19: ( '.' ( Digits )? )?
    			int alt3=2;
    			try { DebugEnterSubRule(3);
    			try { DebugEnterDecision(3, decisionCanBacktrack[3]);
    			int LA3_1 = input.LA(1);

    			if ((LA3_1=='.'))
    			{
    				alt3 = 1;
    			}
    			} finally { DebugExitDecision(3); }
    			switch (alt3)
    			{
    			case 1:
    				DebugEnterAlt(1);
    				// netmxpr.g:85:20: '.' ( Digits )?
    				{
    				DebugLocation(85, 20);
    				Match('.'); 
    				DebugLocation(85, 24);
    				// netmxpr.g:85:24: ( Digits )?
    				int alt2=2;
    				try { DebugEnterSubRule(2);
    				try { DebugEnterDecision(2, decisionCanBacktrack[2]);
    				int LA2_1 = input.LA(1);

    				if (((LA2_1>='0' && LA2_1<='9')))
    				{
    					alt2 = 1;
    				}
    				} finally { DebugExitDecision(2); }
    				switch (alt2)
    				{
    				case 1:
    					DebugEnterAlt(1);
    					// netmxpr.g:85:24: Digits
    					{
    					DebugLocation(85, 24);
    					mDigits(); 

    					}
    					break;

    				}
    				} finally { DebugExitSubRule(2); }


    				}
    				break;

    			}
    			} finally { DebugExitSubRule(3); }


    			}
    			break;
    		case 2:
    			DebugEnterAlt(2);
    			// netmxpr.g:86:6: '.' Digits
    			{
    			DebugLocation(86, 6);
    			Match('.'); 
    			DebugLocation(86, 10);
    			mDigits(); 

    			}
    			break;

    		}
    		state.type = _type;
    		state.channel = _channel;
    	}
    	finally
    	{
    		TraceOut("Number", 21);
    		LeaveRule("Number", 21);
    		LeaveRule_Number();
        }
    }
    // $ANTLR end "Number"

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_Digits() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_Digits() {}

    // $ANTLR start "Digits"
    [GrammarRule("Digits")]
    private void mDigits()
    {
    	EnterRule_Digits();
    	EnterRule("Digits", 22);
    	TraceIn("Digits", 22);
    		try
    		{
    		// netmxpr.g:91:9: ( ( '0' .. '9' )+ )
    		DebugEnterAlt(1);
    		// netmxpr.g:91:12: ( '0' .. '9' )+
    		{
    		DebugLocation(91, 12);
    		// netmxpr.g:91:12: ( '0' .. '9' )+
    		int cnt5=0;
    		try { DebugEnterSubRule(5);
    		while (true)
    		{
    			int alt5=2;
    			try { DebugEnterDecision(5, decisionCanBacktrack[5]);
    			int LA5_1 = input.LA(1);

    			if (((LA5_1>='0' && LA5_1<='9')))
    			{
    				alt5 = 1;
    			}


    			} finally { DebugExitDecision(5); }
    			switch (alt5)
    			{
    			case 1:
    				DebugEnterAlt(1);
    				// netmxpr.g:
    				{
    				DebugLocation(91, 12);
    				input.Consume();


    				}
    				break;

    			default:
    				if (cnt5 >= 1)
    					goto loop5;

    				EarlyExitException eee5 = new EarlyExitException( 5, input );
    				DebugRecognitionException(eee5);
    				throw eee5;
    			}
    			cnt5++;
    		}
    		loop5:
    			;

    		} finally { DebugExitSubRule(5); }


    		}

    	}
    	finally
    	{
    		TraceOut("Digits", 22);
    		LeaveRule("Digits", 22);
    		LeaveRule_Digits();
        }
    }
    // $ANTLR end "Digits"

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_Literal() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_Literal() {}

    // $ANTLR start "Literal"
    [GrammarRule("Literal")]
    private void mLiteral()
    {
    	EnterRule_Literal();
    	EnterRule("Literal", 23);
    	TraceIn("Literal", 23);
    		try
    		{
    		int _type = Literal;
    		int _channel = DefaultTokenChannel;
    		// netmxpr.g:93:10: ( '\"' (~ '\"' )* '\"' | '\\'' (~ '\\'' )* '\\'' )
    		int alt8=2;
    		try { DebugEnterDecision(8, decisionCanBacktrack[8]);
    		int LA8_1 = input.LA(1);

    		if ((LA8_1=='\"'))
    		{
    			alt8 = 1;
    		}
    		else if ((LA8_1=='\''))
    		{
    			alt8 = 2;
    		}
    		else
    		{
    			NoViableAltException nvae = new NoViableAltException("", 8, 0, input, 1);
    			DebugRecognitionException(nvae);
    			throw nvae;
    		}
    		} finally { DebugExitDecision(8); }
    		switch (alt8)
    		{
    		case 1:
    			DebugEnterAlt(1);
    			// netmxpr.g:93:13: '\"' (~ '\"' )* '\"'
    			{
    			DebugLocation(93, 13);
    			Match('\"'); 
    			DebugLocation(93, 17);
    			// netmxpr.g:93:17: (~ '\"' )*
    			try { DebugEnterSubRule(6);
    			while (true)
    			{
    				int alt6=2;
    				try { DebugEnterDecision(6, decisionCanBacktrack[6]);
    				int LA6_1 = input.LA(1);

    				if (((LA6_1>='\u0000' && LA6_1<='!')||(LA6_1>='#' && LA6_1<='\uFFFF')))
    				{
    					alt6 = 1;
    				}


    				} finally { DebugExitDecision(6); }
    				switch ( alt6 )
    				{
    				case 1:
    					DebugEnterAlt(1);
    					// netmxpr.g:
    					{
    					DebugLocation(93, 17);
    					input.Consume();


    					}
    					break;

    				default:
    					goto loop6;
    				}
    			}

    			loop6:
    				;

    			} finally { DebugExitSubRule(6); }

    			DebugLocation(93, 23);
    			Match('\"'); 

    			}
    			break;
    		case 2:
    			DebugEnterAlt(2);
    			// netmxpr.g:94:6: '\\'' (~ '\\'' )* '\\''
    			{
    			DebugLocation(94, 6);
    			Match('\''); 
    			DebugLocation(94, 11);
    			// netmxpr.g:94:11: (~ '\\'' )*
    			try { DebugEnterSubRule(7);
    			while (true)
    			{
    				int alt7=2;
    				try { DebugEnterDecision(7, decisionCanBacktrack[7]);
    				int LA7_1 = input.LA(1);

    				if (((LA7_1>='\u0000' && LA7_1<='&')||(LA7_1>='(' && LA7_1<='\uFFFF')))
    				{
    					alt7 = 1;
    				}


    				} finally { DebugExitDecision(7); }
    				switch ( alt7 )
    				{
    				case 1:
    					DebugEnterAlt(1);
    					// netmxpr.g:
    					{
    					DebugLocation(94, 11);
    					input.Consume();


    					}
    					break;

    				default:
    					goto loop7;
    				}
    			}

    			loop7:
    				;

    			} finally { DebugExitSubRule(7); }

    			DebugLocation(94, 18);
    			Match('\''); 

    			}
    			break;

    		}
    		state.type = _type;
    		state.channel = _channel;
    	}
    	finally
    	{
    		TraceOut("Literal", 23);
    		LeaveRule("Literal", 23);
    		LeaveRule_Literal();
        }
    }
    // $ANTLR end "Literal"

    [Conditional("ANTLR_TRACE")]
    protected virtual void EnterRule_Whitespace() {}
    [Conditional("ANTLR_TRACE")]
    protected virtual void LeaveRule_Whitespace() {}

    // $ANTLR start "Whitespace"
    [GrammarRule("Whitespace")]
    private void mWhitespace()
    {
    	EnterRule_Whitespace();
    	EnterRule("Whitespace", 24);
    	TraceIn("Whitespace", 24);
    		try
    		{
    		int _type = Whitespace;
    		int _channel = DefaultTokenChannel;
    		// netmxpr.g:98:3: ( ( ' ' | '\\t' | '\\n' | '\\r' )+ )
    		DebugEnterAlt(1);
    		// netmxpr.g:98:6: ( ' ' | '\\t' | '\\n' | '\\r' )+
    		{
    		DebugLocation(98, 6);
    		// netmxpr.g:98:6: ( ' ' | '\\t' | '\\n' | '\\r' )+
    		int cnt9=0;
    		try { DebugEnterSubRule(9);
    		while (true)
    		{
    			int alt9=2;
    			try { DebugEnterDecision(9, decisionCanBacktrack[9]);
    			int LA9_1 = input.LA(1);

    			if (((LA9_1>='\t' && LA9_1<='\n')||LA9_1=='\r'||LA9_1==' '))
    			{
    				alt9 = 1;
    			}


    			} finally { DebugExitDecision(9); }
    			switch (alt9)
    			{
    			case 1:
    				DebugEnterAlt(1);
    				// netmxpr.g:
    				{
    				DebugLocation(98, 6);
    				input.Consume();


    				}
    				break;

    			default:
    				if (cnt9 >= 1)
    					goto loop9;

    				EarlyExitException eee9 = new EarlyExitException( 9, input );
    				DebugRecognitionException(eee9);
    				throw eee9;
    			}
    			cnt9++;
    		}
    		loop9:
    			;

    		} finally { DebugExitSubRule(9); }

    		DebugLocation(98, 28);
    		Skip();

    		}

    		state.type = _type;
    		state.channel = _channel;
    	}
    	finally
    	{
    		TraceOut("Whitespace", 24);
    		LeaveRule("Whitespace", 24);
    		LeaveRule_Whitespace();
        }
    }
    // $ANTLR end "Whitespace"

    public override void mTokens()
    {
    	// netmxpr.g:1:8: ( AND | APOS | COMMA | DIV | DOT | EQUALS | EQUAL_OBJ | GE | LE | LESS | LPAR | MINUS | MORE | MUL | NOT | OR | PLUS | QUOT | RPAR | FunctionName | Number | Literal | Whitespace )
    	int alt10=23;
    	try { DebugEnterDecision(10, decisionCanBacktrack[10]);
    	switch (input.LA(1))
    	{
    	case 'a':
    		{
    		int LA10_2 = input.LA(2);

    		if ((LA10_2=='n'))
    		{
    			int LA10_3 = input.LA(3);

    			if ((LA10_3=='d'))
    			{
    				int LA10_4 = input.LA(4);

    				if (((LA10_4>='a' && LA10_4<='z')))
    				{
    					alt10 = 20;
    				}
    				else
    				{
    					alt10 = 1;
    				}
    			}
    			else
    			{
    				alt10 = 20;
    			}
    		}
    		else
    		{
    			alt10 = 20;
    		}
    		}
    		break;
    	case '\'':
    		{
    		int LA10_2 = input.LA(2);

    		if (((LA10_2>='\u0000' && LA10_2<='\uFFFF')))
    		{
    			alt10 = 22;
    		}
    		else
    		{
    			alt10 = 2;
    		}
    		}
    		break;
    	case ',':
    		{
    		alt10 = 3;
    		}
    		break;
    	case '/':
    		{
    		alt10 = 4;
    		}
    		break;
    	case '.':
    		{
    		int LA10_2 = input.LA(2);

    		if (((LA10_2>='0' && LA10_2<='9')))
    		{
    			alt10 = 21;
    		}
    		else
    		{
    			alt10 = 5;
    		}
    		}
    		break;
    	case '=':
    		{
    		alt10 = 6;
    		}
    		break;
    	case 'e':
    		{
    		int LA10_2 = input.LA(2);

    		if ((LA10_2=='q'))
    		{
    			int LA10_3 = input.LA(3);

    			if (((LA10_3>='a' && LA10_3<='z')))
    			{
    				alt10 = 20;
    			}
    			else
    			{
    				alt10 = 7;
    			}
    		}
    		else
    		{
    			alt10 = 20;
    		}
    		}
    		break;
    	case '>':
    		{
    		int LA10_2 = input.LA(2);

    		if ((LA10_2=='='))
    		{
    			alt10 = 8;
    		}
    		else
    		{
    			alt10 = 13;
    		}
    		}
    		break;
    	case '<':
    		{
    		int LA10_2 = input.LA(2);

    		if ((LA10_2=='='))
    		{
    			alt10 = 9;
    		}
    		else
    		{
    			alt10 = 10;
    		}
    		}
    		break;
    	case '(':
    		{
    		alt10 = 11;
    		}
    		break;
    	case '-':
    		{
    		alt10 = 12;
    		}
    		break;
    	case '*':
    		{
    		alt10 = 14;
    		}
    		break;
    	case '~':
    		{
    		alt10 = 15;
    		}
    		break;
    	case 'o':
    		{
    		int LA10_2 = input.LA(2);

    		if ((LA10_2=='r'))
    		{
    			int LA10_3 = input.LA(3);

    			if (((LA10_3>='a' && LA10_3<='z')))
    			{
    				alt10 = 20;
    			}
    			else
    			{
    				alt10 = 16;
    			}
    		}
    		else
    		{
    			alt10 = 20;
    		}
    		}
    		break;
    	case '+':
    		{
    		alt10 = 17;
    		}
    		break;
    	case '\"':
    		{
    		int LA10_2 = input.LA(2);

    		if (((LA10_2>='\u0000' && LA10_2<='\uFFFF')))
    		{
    			alt10 = 22;
    		}
    		else
    		{
    			alt10 = 18;
    		}
    		}
    		break;
    	case ')':
    		{
    		alt10 = 19;
    		}
    		break;
    	case 'b':
    	case 'c':
    	case 'd':
    	case 'f':
    	case 'g':
    	case 'h':
    	case 'i':
    	case 'j':
    	case 'k':
    	case 'l':
    	case 'm':
    	case 'n':
    	case 'p':
    	case 'q':
    	case 'r':
    	case 's':
    	case 't':
    	case 'u':
    	case 'v':
    	case 'w':
    	case 'x':
    	case 'y':
    	case 'z':
    		{
    		alt10 = 20;
    		}
    		break;
    	case '0':
    	case '1':
    	case '2':
    	case '3':
    	case '4':
    	case '5':
    	case '6':
    	case '7':
    	case '8':
    	case '9':
    		{
    		alt10 = 21;
    		}
    		break;
    	case '\t':
    	case '\n':
    	case '\r':
    	case ' ':
    		{
    		alt10 = 23;
    		}
    		break;
    	default:
    		{
    			NoViableAltException nvae = new NoViableAltException("", 10, 0, input, 1);
    			DebugRecognitionException(nvae);
    			throw nvae;
    		}
    	}

    	} finally { DebugExitDecision(10); }
    	switch (alt10)
    	{
    	case 1:
    		DebugEnterAlt(1);
    		// netmxpr.g:1:10: AND
    		{
    		DebugLocation(1, 10);
    		mAND(); 

    		}
    		break;
    	case 2:
    		DebugEnterAlt(2);
    		// netmxpr.g:1:14: APOS
    		{
    		DebugLocation(1, 14);
    		mAPOS(); 

    		}
    		break;
    	case 3:
    		DebugEnterAlt(3);
    		// netmxpr.g:1:19: COMMA
    		{
    		DebugLocation(1, 19);
    		mCOMMA(); 

    		}
    		break;
    	case 4:
    		DebugEnterAlt(4);
    		// netmxpr.g:1:25: DIV
    		{
    		DebugLocation(1, 25);
    		mDIV(); 

    		}
    		break;
    	case 5:
    		DebugEnterAlt(5);
    		// netmxpr.g:1:29: DOT
    		{
    		DebugLocation(1, 29);
    		mDOT(); 

    		}
    		break;
    	case 6:
    		DebugEnterAlt(6);
    		// netmxpr.g:1:33: EQUALS
    		{
    		DebugLocation(1, 33);
    		mEQUALS(); 

    		}
    		break;
    	case 7:
    		DebugEnterAlt(7);
    		// netmxpr.g:1:40: EQUAL_OBJ
    		{
    		DebugLocation(1, 40);
    		mEQUAL_OBJ(); 

    		}
    		break;
    	case 8:
    		DebugEnterAlt(8);
    		// netmxpr.g:1:50: GE
    		{
    		DebugLocation(1, 50);
    		mGE(); 

    		}
    		break;
    	case 9:
    		DebugEnterAlt(9);
    		// netmxpr.g:1:53: LE
    		{
    		DebugLocation(1, 53);
    		mLE(); 

    		}
    		break;
    	case 10:
    		DebugEnterAlt(10);
    		// netmxpr.g:1:56: LESS
    		{
    		DebugLocation(1, 56);
    		mLESS(); 

    		}
    		break;
    	case 11:
    		DebugEnterAlt(11);
    		// netmxpr.g:1:61: LPAR
    		{
    		DebugLocation(1, 61);
    		mLPAR(); 

    		}
    		break;
    	case 12:
    		DebugEnterAlt(12);
    		// netmxpr.g:1:66: MINUS
    		{
    		DebugLocation(1, 66);
    		mMINUS(); 

    		}
    		break;
    	case 13:
    		DebugEnterAlt(13);
    		// netmxpr.g:1:72: MORE
    		{
    		DebugLocation(1, 72);
    		mMORE(); 

    		}
    		break;
    	case 14:
    		DebugEnterAlt(14);
    		// netmxpr.g:1:77: MUL
    		{
    		DebugLocation(1, 77);
    		mMUL(); 

    		}
    		break;
    	case 15:
    		DebugEnterAlt(15);
    		// netmxpr.g:1:81: NOT
    		{
    		DebugLocation(1, 81);
    		mNOT(); 

    		}
    		break;
    	case 16:
    		DebugEnterAlt(16);
    		// netmxpr.g:1:85: OR
    		{
    		DebugLocation(1, 85);
    		mOR(); 

    		}
    		break;
    	case 17:
    		DebugEnterAlt(17);
    		// netmxpr.g:1:88: PLUS
    		{
    		DebugLocation(1, 88);
    		mPLUS(); 

    		}
    		break;
    	case 18:
    		DebugEnterAlt(18);
    		// netmxpr.g:1:93: QUOT
    		{
    		DebugLocation(1, 93);
    		mQUOT(); 

    		}
    		break;
    	case 19:
    		DebugEnterAlt(19);
    		// netmxpr.g:1:98: RPAR
    		{
    		DebugLocation(1, 98);
    		mRPAR(); 

    		}
    		break;
    	case 20:
    		DebugEnterAlt(20);
    		// netmxpr.g:1:103: FunctionName
    		{
    		DebugLocation(1, 103);
    		mFunctionName(); 

    		}
    		break;
    	case 21:
    		DebugEnterAlt(21);
    		// netmxpr.g:1:116: Number
    		{
    		DebugLocation(1, 116);
    		mNumber(); 

    		}
    		break;
    	case 22:
    		DebugEnterAlt(22);
    		// netmxpr.g:1:123: Literal
    		{
    		DebugLocation(1, 123);
    		mLiteral(); 

    		}
    		break;
    	case 23:
    		DebugEnterAlt(23);
    		// netmxpr.g:1:131: Whitespace
    		{
    		DebugLocation(1, 131);
    		mWhitespace(); 

    		}
    		break;

    	}

    }


	#region DFA

	protected override void InitDFAs()
	{
		base.InitDFAs();
	}

 
	#endregion

}

} // namespace  NetMX.Expression 
