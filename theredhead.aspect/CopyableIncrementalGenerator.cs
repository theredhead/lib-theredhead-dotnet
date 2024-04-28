using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace theredhead.aspect;

[AttributeUsage(AttributeTargets.Class)]
public class CopyableAttribute : Attribute {

}

public class ClassInfo : DistilledSyntax 
{
    public string Namespace {get; private set;} = "";
    public ClassDeclarationSyntax? Syntax {get; private set;}
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

[Generator]
public class CopyableIncrementalGenerator : BaseIncrementalGenerator<ClassInfo>
{
    protected override bool IsNodeOfInterest(SyntaxNode node) => 
        node is ClassDeclarationSyntax cls && cls.IsPartial() && cls.HasAttribute<CopyableAttribute>();

    protected override string GenerateCode(ClassInfo blockInfo)
    {
        var sb = new StringBuilder();

        string ArgumentName(string propertyName) => propertyName.ToLowerInvariant();

        var methodParameters = blockInfo.Properties.Select(p
            => $"{p.Type}? {ArgumentName(p.Identifier.Text)} = default");
        var methodParametersText = string.Join(", ", methodParameters);

        var propertyAssignments = blockInfo.Properties.Select(p
            => $"copy.{p.Identifier.Text} = {ArgumentName(p.Identifier.Text)} ?? this.{p.Identifier.Text};");
        var propertyAssignmentsText = string.Join("\n\t\t\t", propertyAssignments);

        // lang=cs
        return $$"""
        // <generated />
        namespace {{ blockInfo.Namespace }} {
            [global::System.CodeDom.Compiler.GeneratedCode]
            public partial class {blockInfo.Name} {
                public {{ blockInfo.Name }} Copy({{ methodParametersText }}) {
                    var copy = new {{blockInfo.Name}}();
                    {{ propertyAssignmentsText }};
                    return copy;
                }
            }
        }
        //EOF
        """;
    }
}
