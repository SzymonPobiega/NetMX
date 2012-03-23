grammar netmxpr;

options
{
    language=CSharp2;
	output=AST;
}

/*
Grammar of simple expression
*/

tokens {  
  LPAR  =  '(';
  RPAR  =  ')';
  MINUS  =  '-';
  PLUS  =  '+';
  MUL  =  '*';
  DIV  =  '/';
  DOT	=  '.';
  COMMA  =  ',';  
  LESS  =  '<';
  MORE  =  '>';
  EQUALS  =  '=';
  EQUAL_OBJ  =  'eq';
  OR = 'or';
  AND = 'and';
  NOT  =  '~';
  LE  =  '<=';
  GE  =  '>=';  
  APOS  =  '\'';
  QUOT  =  '\"';
}

@lexer::namespace { NetMX.Expression }
@parser::namespace { NetMX.Expression }

public parse  :  expr
  ;

expr  :  orExpr
  ;

primaryExpr
  :  '('! expr ')'!
  |  Literal
  |  Number  
  |  functionCall
  ;

functionCall
  :  FunctionName^ '('! expr ')'!
  ;

orExpr  :  andExpr ('or'^ andExpr)*
  ;

andExpr  :  equalityExpr ('and'^ equalityExpr)*
  ;

equalityExpr
  :  relationalExpr (('='|'eq')^ relationalExpr)*
  ;

relationalExpr
  :  additiveExpr (('<'|'>'|'<='|'>=')^ additiveExpr)*
  ;

additiveExpr
  :  multiplicativeExpr (('+'|'-')^ multiplicativeExpr)*
  ;

multiplicativeExpr
  :  unaryExpr (('*'|'/')^ unaryExpr)*
  ;

unaryExpr
  :  '~'* primaryExpr
  ;

FunctionName
  :  ('a'..'z')+
  ;
  
Number  :  Digits ('.' Digits?)?
  |  '.' Digits
  ;

fragment
Digits  :  ('0'..'9')+
  ;

Literal  :  '"' ~'"'* '"'
  |  '\'' ~'\''* '\''
  ;

Whitespace
  :  (' '|'\t'|'\n'|'\r')+ {Skip();}
  ;