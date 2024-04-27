grammar apollon;

// Parser rules
program: statement* EOF;
statement: fact | rule | constraint | COMMENT;
fact: literal '.';
rule: head ':-' body '.';
constraint: ':-' naf_literal (',' naf_literal)* '.';
head: literal;
body: body_part (',' body_part)*;
body_part: operation | naf_literal;
literal: NEGATION? atom;

naf_literal: NAF? literal;
atom:
	CLASICAL_TERM (
		'(' (atom_param_part (',' atom_param_part)*)? ')'
	)?;
atom_param_part: general_term | NUMBER | atom;
general_term: VARIABLE_TERM | CLASICAL_TERM;

operation: VARIABLE_TERM operator (atom | NUMBER);
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
NUMBER: [0-9]+;