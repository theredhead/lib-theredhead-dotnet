using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace theredhead.aspect;

public abstract class DistilledSyntax
{
    public DistilledSyntax() {}

    public string Name { get; protected set; } = "";
    public virtual void Load(SyntaxNode node, GeneratorSyntaxContext context) {
        if (node is ClassDeclarationSyntax cls) {
            Name = cls.Identifier.Text;
        }
    }
}

