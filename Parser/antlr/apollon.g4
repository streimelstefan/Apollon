grammar apollon;

// Parser rules
program: statement* EOF;
query: body_part (',' body_part)* '.' EOF;

statement: fact | rule | constraint | docu | COMMENT;
fact: literal '.';
rule: head ':-' body '.';
constraint: ':-' body_part (',' body_part)* '.';
head: literal;
body: body_part (',' body_part)*;
body_part:
	inline_operation
	| naf_literal
	| generating_operation;
literal: NEGATION? atom;
variable_placeholder: '@(' VARIABLE_TERM ')';

docu_string: docu_string_part+;
docu_string_part:
	docu_string_string_part
	| variable_placeholder;
docu_string_string_part: general_term | 'is' | 'not';
docu: docu_head (DOKU_SEPERATOR docu_string)? '.';
docu_head:
	NEGATION? CLASICAL_TERM (
		'(' (VARIABLE_TERM (',' VARIABLE_TERM)*)? ')'
	)?;

naf_literal: NAF? literal;
atom:
	CLASICAL_TERM (
		'(' (atom_param_part (',' atom_param_part)*)? ')'
	)?;
atom_param_part: general_term | NUMBER | literal;
general_term: VARIABLE_TERM | CLASICAL_TERM;

inline_operation:
	VARIABLE_TERM inline_operators atom_param_part;
generating_operation:
	VARIABLE_TERM 'is' generating_operation_variable generating_operators
		generating_operation_operant;

generating_operation_variable: VARIABLE_TERM;
generating_operation_operant: VARIABLE_TERM | NUMBER;
inline_operators:
	EQUALS
	| NOT_EQUALS
	| LARGER
	| SMALLER
	| LARGER_EQUALS
	| SMALLER_EQUALS;
generating_operators: PLUS | NEGATION | TIMES | DIVIDE;

// Lexer rules
NAF: 'not';
NEGATION: '-';
EQUALS: '=';
LARGER: '>';
SMALLER: '<';
LARGER_EQUALS: '>=';
SMALLER_EQUALS: '<=';
PLUS: '+';
TIMES: '*';
DIVIDE: '/';
NOT_EQUALS: '!=';
COMMENT: '%' ~[\r\n]* -> skip;
WS: [ \t\r\n]+ -> skip;
CLASICAL_TERM: [a-z][a-zA-Z0-9_]*;
VARIABLE_TERM: [A-Z][a-zA-Z0-9_]*;
DOKU_SEPERATOR: '::';
NUMBER: [0-9]+;