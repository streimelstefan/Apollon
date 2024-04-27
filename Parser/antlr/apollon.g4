grammar apollon;

// Parser rules
program: statement* EOF;
statement: fact | rule | constraint | COMMENT;
fact: literal '.';
rule: head ':-' body '.';
constraint: ':-' body '.';
head: literal;
body: naf_literal (',' naf_literal)*;
literal: NEGATION? atom;
naf_literal: NAF? literal;
atom:
	CLASICAL_TERM (
		'(' (atom_param_part (',' atom_param_part)*)? ')'
	)?;
atom_param_part: general_term | atom;
general_term: VARIABLE_TERM | CLASICAL_TERM;

// Lexer rules
NAF: 'not';
NEGATION: '-';
COMMENT: '%' ~[\r\n]* -> skip;
WS: [ \t\r\n]+ -> skip;
CLASICAL_TERM: [a-z][a-zA-Z0-9_]*;
VARIABLE_TERM: [A-Z][a-zA-Z0-9_]*;