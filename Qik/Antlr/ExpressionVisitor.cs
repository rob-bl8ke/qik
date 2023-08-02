using System;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using CygSoft.Qik.Functions;

namespace CygSoft.Qik.Antlr
{
    internal class ExpressionVisitor : QikTemplateBaseVisitor<IFunction>
    {
        private readonly ISymbolTable symbolTable;
        private readonly IFunctionFactory functionFactory;

        internal ExpressionVisitor(ISymbolTable symbolTable, IFunctionFactory functionFactory)
        {
            this.functionFactory = functionFactory ?? throw new ArgumentNullException($"{nameof(functionFactory)} cannot be null.");
            this.symbolTable = symbolTable ?? throw new ArgumentNullException($"{nameof(symbolTable)} cannot be null.");
        }

        public override IFunction VisitFuncDecl([NotNull] QikTemplateParser.FuncDeclContext context)
        {
            IFunction func = null;
            var id = context.VARIABLE().GetText();
            
            if (context.stat() != null)
            {
                func = GetStatementFunction(context.stat());
                symbolTable.AddSymbol(new ExpressionSymbol(id, func));
            }

            else if (context.switchExpr() != null)
            {
                func = VisitSwitchExpr(context.switchExpr());
                symbolTable.AddSymbol(new ExpressionSymbol(id, func));
            }

            else if (context.ifExpr() != null)
            {
                func = VisitIfExpr(context.ifExpr());
                symbolTable.AddSymbol(new ExpressionSymbol(id, func));
            }
            
            return func;
        }

        public override IFunction VisitFunc(QikTemplateParser.FuncContext context)
        {
            IFunction func = null;

            if (context.IDENTIFIER() != null)
            {
                string funcIdentifier = context.IDENTIFIER().GetText();
                List<IFunction> functionArguments = CreateFunctionArguments(context.funcArg());

                func = functionFactory.GetFunction(funcIdentifier, functionArguments);
            }
            return func;
        }

        public override IFunction VisitIffExpr([NotNull] QikTemplateParser.IffExprContext context) => GetIifFunction(context);
        public override IFunction VisitIfExpr([NotNull] QikTemplateParser.IfExprContext context) => GetIfFunction(context);
        public override IFunction VisitSwitchExpr([NotNull] QikTemplateParser.SwitchExprContext context) => GetSwitchFunction(context);

        public override IFunction VisitExpr(QikTemplateParser.ExprContext context)
        {
            if (context.STRING() != null)
            {
                return new TextFunction("String", context.STRING().GetText().StripOuterQuotes());
            }

            else if (context.VARIABLE() != null)
            {
                return new VariableFunction("Variable", symbolTable, context.VARIABLE().GetText());
            }

            else if (context.CONST() != null)
            {
                string constantText = context.CONST().GetText();
                if (constantText == "NEWLINE")
                    return new NewlineFunction("Constant");
                else
                    return new ConstantFunction("Constant", context.CONST().GetText());
            }

            else if (context.INT() != null)
                return new IntegerFunction("Int", context.INT().GetText());

            else if (context.FLOAT() != null)
                return new FloatFunction("Float", context.FLOAT().GetText());

            // recurse...
            else if (context.func() != null)
                return VisitFunc(context.func());

            else
                return null;
        }

        private List<IFunction> CreateFunctionArguments(IReadOnlyList<QikTemplateParser.FuncArgContext> funcArgs)
        {
            var functionArguments = new List<IFunction>();

            foreach (var funcArg in funcArgs)
            {
                var concatExpr = funcArg.concatExpr();
                var expr = funcArg.expr();

                if (concatExpr != null)
                {
                    var concatenateFunc = GetConcatenateFunction(concatExpr);
                    functionArguments.Add(concatenateFunc);
                }
                else if (expr != null)
                {
                    var function = VisitExpr(expr);
                    functionArguments.Add(function);
                }
            }
            return functionArguments;
        }

        private IFunction GetSwitchFunction(QikTemplateParser.SwitchExprContext context)
        {
            var subjectFunction = VisitExpr(context.switchStat().expr());

            var caseFuncs = new Dictionary<string, IFunction>();

            foreach (var possibleCase in context.caseStat())
            {
                var testVal = possibleCase.STRING().GetText().StripOuterQuotes();
                caseFuncs.Add(testVal, GetStatementFunction(possibleCase.stat()));
            }

            IFunction elseFunction = GetStatementFunction(context.elseStat().stat());

            return new SwitchFunction("SwitchFunction", subjectFunction, caseFuncs, elseFunction);
        }

        private IFunction GetIfFunction([NotNull] QikTemplateParser.IfExprContext context)
        {
            var ifComparison = context.ifStat().compExpr();
            var ifExpressionOperator = ifComparison.children[1].GetText();
            var ifLeftOperand = VisitExpr(ifComparison.expr()[0]);
            var ifRightOperand = VisitExpr(ifComparison.expr()[1]);
            var ifResolutionFunc = GetStatementFunction(context.ifStat().stat());

            var ifFunction = new IfCase(ifLeftOperand, ifRightOperand, ifExpressionOperator, ifResolutionFunc);
            var elseIfFunctions = new List<IfCase>();

            if (context.elseIfStat() is not null && context.elseIfStat().Length > 0)
            {
                foreach (var elseIf in context.elseIfStat())
                {
                    var elseIfComparison = elseIf.compExpr();
                    var elseIfExpressionOperator = elseIfComparison.children[1].GetText();
                    var elseIfLeftOperand = VisitExpr(elseIfComparison.expr()[0]);
                    var elseIfRightOperand = VisitExpr(elseIfComparison.expr()[1]);
                    var elseIfResolutionFunc = GetStatementFunction(elseIf.stat());

                    var elseIfFunction = new IfCase(elseIfLeftOperand, elseIfRightOperand, elseIfExpressionOperator, elseIfResolutionFunc);
                    elseIfFunctions.Add(elseIfFunction);
                }
            }

            var elseFunction = GetStatementFunction(context.elseStat().stat());

            return new IfFunction("IfFunction", ifFunction, elseIfFunctions, elseFunction);
        }

        private IFunction GetIifFunction(QikTemplateParser.IffExprContext context)
        {
            var comparison = context.compExpr();
            var compOperator = comparison.children[1].GetText();
            var leftOperand = VisitExpr(comparison.expr()[0]);
            var rightOperand = VisitExpr(comparison.expr()[1]);

            var trueFunc = GetStatementFunction(context.iffTrueStat().stat());
            var falseFunc = GetStatementFunction(context.iffFalseStat().stat());

            return new IifFunction("Iif", leftOperand, rightOperand, compOperator, trueFunc, falseFunc);
        }

        private IFunction GetStatementFunction(QikTemplateParser.StatContext context)
        {
            IFunction statementFunction = null;

            if (context.expr() is not null)
            {
                statementFunction = VisitExpr(context.expr());
            }
            else if (context.iffExpr() is not null)
            {
                statementFunction = VisitIffExpr(context.iffExpr());
            }
            else if (context.concatExpr() is not null)
            {
                statementFunction = GetConcatenateFunction(context.concatExpr());
            }

            return statementFunction;
        }

        private IFunction GetConcatenateFunction(QikTemplateParser.ConcatExprContext context)
        {
            var func = new ConcatenateFunction("Concatenation");
            var expressions = context.expr();

            foreach (var expr in expressions)
            {
                IFunction result = VisitExpr(expr);
                func.AddFunction(result);
            }

            return func;
        }
    }
}
