using System.Collections.Generic;
using Antlr4.Runtime;
using CygSoft.Qik.Antlr;

namespace Qik.UiWidgets
{
    public class UiWidgetFactory
    {
        public UiWidget[] BuildFromScript(string scriptText)
        {
            List<UiWidget> widgets = new();

            var inputStream = new AntlrInputStream(scriptText);
            var lexer = new QikTemplateLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new QikTemplateParser(tokens);

            var tree = parser.template();

            var controlVisitor = new UiWidgetVisitor(widgets);
            controlVisitor.Visit(tree);

            return widgets.ToArray();
        }
    }
}