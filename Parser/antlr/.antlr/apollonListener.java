// Generated from c://Users//strei//Documents//dev//Apollon//Parser//antlr//apollon.g4 by ANTLR 4.13.1
import org.antlr.v4.runtime.tree.ParseTreeListener;

/**
 * This interface defines a complete listener for a parse tree produced by
 * {@link apollonParser}.
 */
public interface apollonListener extends ParseTreeListener {
	/**
	 * Enter a parse tree produced by {@link apollonParser#program}.
	 * @param ctx the parse tree
	 */
	void enterProgram(apollonParser.ProgramContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#program}.
	 * @param ctx the parse tree
	 */
	void exitProgram(apollonParser.ProgramContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#query}.
	 * @param ctx the parse tree
	 */
	void enterQuery(apollonParser.QueryContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#query}.
	 * @param ctx the parse tree
	 */
	void exitQuery(apollonParser.QueryContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#statement}.
	 * @param ctx the parse tree
	 */
	void enterStatement(apollonParser.StatementContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#statement}.
	 * @param ctx the parse tree
	 */
	void exitStatement(apollonParser.StatementContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#fact}.
	 * @param ctx the parse tree
	 */
	void enterFact(apollonParser.FactContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#fact}.
	 * @param ctx the parse tree
	 */
	void exitFact(apollonParser.FactContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#rule}.
	 * @param ctx the parse tree
	 */
	void enterRule(apollonParser.RuleContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#rule}.
	 * @param ctx the parse tree
	 */
	void exitRule(apollonParser.RuleContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#constraint}.
	 * @param ctx the parse tree
	 */
	void enterConstraint(apollonParser.ConstraintContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#constraint}.
	 * @param ctx the parse tree
	 */
	void exitConstraint(apollonParser.ConstraintContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#head}.
	 * @param ctx the parse tree
	 */
	void enterHead(apollonParser.HeadContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#head}.
	 * @param ctx the parse tree
	 */
	void exitHead(apollonParser.HeadContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#body}.
	 * @param ctx the parse tree
	 */
	void enterBody(apollonParser.BodyContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#body}.
	 * @param ctx the parse tree
	 */
	void exitBody(apollonParser.BodyContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#body_part}.
	 * @param ctx the parse tree
	 */
	void enterBody_part(apollonParser.Body_partContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#body_part}.
	 * @param ctx the parse tree
	 */
	void exitBody_part(apollonParser.Body_partContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#literal}.
	 * @param ctx the parse tree
	 */
	void enterLiteral(apollonParser.LiteralContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#literal}.
	 * @param ctx the parse tree
	 */
	void exitLiteral(apollonParser.LiteralContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#variable_placeholder}.
	 * @param ctx the parse tree
	 */
	void enterVariable_placeholder(apollonParser.Variable_placeholderContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#variable_placeholder}.
	 * @param ctx the parse tree
	 */
	void exitVariable_placeholder(apollonParser.Variable_placeholderContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#docu_string}.
	 * @param ctx the parse tree
	 */
	void enterDocu_string(apollonParser.Docu_stringContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#docu_string}.
	 * @param ctx the parse tree
	 */
	void exitDocu_string(apollonParser.Docu_stringContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#docu_string_part}.
	 * @param ctx the parse tree
	 */
	void enterDocu_string_part(apollonParser.Docu_string_partContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#docu_string_part}.
	 * @param ctx the parse tree
	 */
	void exitDocu_string_part(apollonParser.Docu_string_partContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#docu_string_string_part}.
	 * @param ctx the parse tree
	 */
	void enterDocu_string_string_part(apollonParser.Docu_string_string_partContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#docu_string_string_part}.
	 * @param ctx the parse tree
	 */
	void exitDocu_string_string_part(apollonParser.Docu_string_string_partContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#docu}.
	 * @param ctx the parse tree
	 */
	void enterDocu(apollonParser.DocuContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#docu}.
	 * @param ctx the parse tree
	 */
	void exitDocu(apollonParser.DocuContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#docu_head}.
	 * @param ctx the parse tree
	 */
	void enterDocu_head(apollonParser.Docu_headContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#docu_head}.
	 * @param ctx the parse tree
	 */
	void exitDocu_head(apollonParser.Docu_headContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#naf_literal}.
	 * @param ctx the parse tree
	 */
	void enterNaf_literal(apollonParser.Naf_literalContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#naf_literal}.
	 * @param ctx the parse tree
	 */
	void exitNaf_literal(apollonParser.Naf_literalContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#atom}.
	 * @param ctx the parse tree
	 */
	void enterAtom(apollonParser.AtomContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#atom}.
	 * @param ctx the parse tree
	 */
	void exitAtom(apollonParser.AtomContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#atom_param_part}.
	 * @param ctx the parse tree
	 */
	void enterAtom_param_part(apollonParser.Atom_param_partContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#atom_param_part}.
	 * @param ctx the parse tree
	 */
	void exitAtom_param_part(apollonParser.Atom_param_partContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#general_term}.
	 * @param ctx the parse tree
	 */
	void enterGeneral_term(apollonParser.General_termContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#general_term}.
	 * @param ctx the parse tree
	 */
	void exitGeneral_term(apollonParser.General_termContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#inline_operation}.
	 * @param ctx the parse tree
	 */
	void enterInline_operation(apollonParser.Inline_operationContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#inline_operation}.
	 * @param ctx the parse tree
	 */
	void exitInline_operation(apollonParser.Inline_operationContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#generating_operation}.
	 * @param ctx the parse tree
	 */
	void enterGenerating_operation(apollonParser.Generating_operationContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#generating_operation}.
	 * @param ctx the parse tree
	 */
	void exitGenerating_operation(apollonParser.Generating_operationContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#generating_operation_variable}.
	 * @param ctx the parse tree
	 */
	void enterGenerating_operation_variable(apollonParser.Generating_operation_variableContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#generating_operation_variable}.
	 * @param ctx the parse tree
	 */
	void exitGenerating_operation_variable(apollonParser.Generating_operation_variableContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#generating_operation_operant}.
	 * @param ctx the parse tree
	 */
	void enterGenerating_operation_operant(apollonParser.Generating_operation_operantContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#generating_operation_operant}.
	 * @param ctx the parse tree
	 */
	void exitGenerating_operation_operant(apollonParser.Generating_operation_operantContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#inline_operators}.
	 * @param ctx the parse tree
	 */
	void enterInline_operators(apollonParser.Inline_operatorsContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#inline_operators}.
	 * @param ctx the parse tree
	 */
	void exitInline_operators(apollonParser.Inline_operatorsContext ctx);
	/**
	 * Enter a parse tree produced by {@link apollonParser#generating_operators}.
	 * @param ctx the parse tree
	 */
	void enterGenerating_operators(apollonParser.Generating_operatorsContext ctx);
	/**
	 * Exit a parse tree produced by {@link apollonParser#generating_operators}.
	 * @param ctx the parse tree
	 */
	void exitGenerating_operators(apollonParser.Generating_operatorsContext ctx);
}