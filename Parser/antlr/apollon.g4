grammar apollon;

// Parser rules
program: statement* EOF;
query: body_part (',' body_part)* '.' EOF;

statement: fact | rule | constraint | docu | COMMENT;
fact: literal'.';
rule: head ':-' body '.';
constraint: ':-' naf_literal (',' naf_literal)* '.';
head: literal;
body: body_part (',' body_part)*;
body_part: operation | naf_literal;
literal: NEGATION? atom;
variable_placeholder: '@(' VARIABLE_TERM ')';

docu_string: docu_string_part+;
docu_string_part: general_term | variable_placeholder;
docu: docu_head (DOKU_SEPERATOR docu_string)? '.';
docu_head: NEGATION? CLASICAL_TERM (
		'(' (VARIABLE_TERM (',' VARIABLE_TERM)*)? ')'
	)?;

naf_literal: NAF? literal;
atom:
	CLASICAL_TERM (
		'(' (atom_param_part (',' atom_param_part)*)? ')'
	)?;
atom_param_part: general_term | NUMBER | literal;
general_term: VARIABLE_TERM | CLASICAL_TERM;

operation: VARIABLE_TERM operator (naf_literal | NUMBER);
operator: EQUALS | NOT_EQUALS;

// Lexer rules
NAF: 'not';
NEGATION: '-';
EQUALS: '=';
NOT_EQUALS: '!=';
COMMENT: '%' ~[\r\n]* -> skip;
WS: [ \t\r\n]+ -> skip;
CLASICAL_TERM: [a-z][a-zA-Z0-9_]*;
VARIABLE_TERM: [A-Z][a-zA-Z0-9_]*;
DOKU_SEPERATOR: '::';
NUMBER: [0-9]+;