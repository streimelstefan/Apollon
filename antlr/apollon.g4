grammar apollon;

// Parser rules
program: statement* EOF;
statement: fact | rule | COMMENT;
fact: literal '.';
rule: head ':-' body '.';
head: literal;
body: naf_literal (',' naf_literal)*;
literal: NEGATION? atom;
naf_literal: NAF? literal;
atom: CLASICAL_TERM ( '(' GENERAL_TERM (',' GENERAL_TERM)* ')')?;

// Lexer rules
CLASICAL_TERM: [a-z][a-zA-Z0-9_]*;
VARIABLE_TERM: [A-Z][a-zA-Z0-9_]*;
GENERAL_TERM: CLASICAL_TERM | VARIABLE_TERM;
NAF: 'not';
NEGATION: '-';
COMMENT: '%' ~[\r\n]* -> skip;
WS: [ \t\r\n]+ -> skip;