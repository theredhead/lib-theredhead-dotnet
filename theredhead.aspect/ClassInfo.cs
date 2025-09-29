using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace theredhead.aspect;

public class ClassInfo : DistilledSyntax 
{
    public string Namespace {get; private set;} = "";
    public ClassDeclarationSyntax? Syntax {get; private set;}
    public IEnumerable<MethodDeclarationSyntax> Methods { get; private set; } = Enumerable.Empty<MethodDeclarationSyntax>();
    public IEnumerable<PropertyDeclarationSyntax> Properties { get; private set; } = Enumerable.Empty<PropertyDeclarationSyntax>();
    public IEnumerable<FieldDeclarationSyntax> Fields { get; private set; } = Enumerable.Empty<FieldDeclarationSyntax>();


    public override void Load(SyntaxNode node, GeneratorSyntaxContext context)
    {
        if (node is not ClassDeclarationSyntax cls) return; //throw new ArgumentException("Expected a ClassDeclarationSyntax node");
        
        Syntax = cls;
        Namespace = context.SemanticModel.GetDeclaredSymbol(cls)?.ContainingNamespace?.ToString() ?? "";
        Name = cls.Identifier.Text;
        Properties = cls.Members.OfType<PropertyDeclarationSyntax>();
        Fields = cls.Members.OfType<FieldDeclarationSyntax>();
    }
}
