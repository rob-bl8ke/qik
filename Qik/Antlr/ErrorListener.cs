// using Antlr4.Runtime;
// using Antlr4.Runtime.Atn;
// using Antlr4.Runtime.Dfa;
// using Antlr4.Runtime.Sharpen;
// using System;
// using System.IO;
// using System.Linq;

// namespace CygSoft.Qik.Antlr
// {
//     internal class ErrorListener : BaseErrorListener
//     {
//         public event EventHandler<SyntaxErrorEventArgs> SyntaxErrorDetected;

//         public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
//         {
//             // TODO: Implement below commented out when have time to check and test the impact

//             // if (output is null) throw new ArgumentNullException($"{nameof(output)} cannot be null.");
//             // if (recognizer is null) throw new ArgumentNullException($"{nameof(recognizer)} cannot be null.");
//             // if (offendingSymbol is null) throw new ArgumentNullException($"{nameof(offendingSymbol)} cannot be null.");
//             // if (e is null) throw new ArgumentNullException($"RecognitionException cannot be null.");

//             var stack = ((Parser)recognizer).GetRuleInvocationStack();
//             stack.Reverse();

//             SyntaxErrorDetected?.Invoke(this, new SyntaxErrorEventArgs(UserFriendlyContext(stack[0].ToString()), line, charPositionInLine, offendingSymbol.ToString(), msg));
//             base.SyntaxError(output, recognizer, offendingSymbol, line, charPositionInLine, msg, e);
//         }

//         public override void ReportAmbiguity(Parser recognizer, DFA dfa, int startIndex, int stopIndex, bool exact, BitSet ambigAlts, ATNConfigSet configs)
//         {
//             base.ReportAmbiguity(recognizer, dfa, startIndex, stopIndex, exact, ambigAlts, configs);
//         }

//         public override void ReportAttemptingFullContext(Parser recognizer, DFA dfa, int startIndex, int stopIndex, BitSet conflictingAlts, SimulatorState conflictState)
//         {
//             base.ReportAttemptingFullContext(recognizer, dfa, startIndex, stopIndex, conflictingAlts, conflictState);
//         }

//         public override void ReportContextSensitivity(Parser recognizer, DFA dfa, int startIndex, int stopIndex, int prediction, SimulatorState acceptState)
//         {
//             base.ReportContextSensitivity(recognizer, dfa, startIndex, stopIndex, prediction, acceptState);
//         }

//         private string UserFriendlyContext(string stackId)
//         {
//             //
//             // TODO (Rob) You'll need to update this or find another strategy
//             switch (stackId)
//             {
//                 case "template":
//                     return "Main Script";
//                 case "ctrlDecl":
//                     return "Input Control";
//                 case "exprDecl":
//                     return "Expression Declaration";
//                 case "ifOptExpr":
//                     return "If Expression";
//                 case "declArgs":
//                     return "Declaration Parameters";
//                 case "declArg":
//                     return "Declaration Parameter";
//                 case "concatExpr":
//                     return "Concatenation Expression";
//                 case "func":
//                     return "Function Expression";
//                 case "funcArg":
//                     return "Function Argument";
//                 default:
//                     return stackId;
//             }
//         }
//     }
// }
