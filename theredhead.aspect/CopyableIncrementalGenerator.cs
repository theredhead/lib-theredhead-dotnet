using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using FieldDeclarationSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax;

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
public class CopyableIncrementalGenerator : IIncrementalGenerator
{
    private string ToolName => $"{GetType().Namespace}.{GetType().Name}";
    private string ToolVersion => GetType().Assembly.ImageRuntimeVersion;

    private bool IsNodeOfInterest(SyntaxNode node) =>
        node is ClassDeclarationSyntax cls && cls.IsPartial() && cls.HasAttribute<CopyableAttribute>();

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider.CreateSyntaxProvider(
            predicate: (node, _) => IsNodeOfInterest(node),
            transform: (ctx, _) => Distill((ClassDeclarationSyntax)ctx.Node, ctx)
        ).Where(m => m is not null);

        context.RegisterSourceOutput(provider, Generate);
    }

    private ClassInfo Distill(SyntaxNode node, GeneratorSyntaxContext context) {
        var distilled = new ClassInfo();
        distilled.Load(node, context);
        return distilled;
    }

    private void Generate(SourceProductionContext context, ClassInfo blockInfo)
    {
        var fileNameHint = $"{blockInfo.Name}.{GetType().Name}.g.cs";
        context.AddSource(fileNameHint, SourceText.From(
            GenerateCode(blockInfo), Encoding.UTF8
        ));
    }

    protected string GenerateCode(ClassInfo blockInfo)
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
        #nullable enable
        namespace {{ blockInfo.Namespace }} {
            [global::System.CodeDom.Compiler.GeneratedCode("{{ ToolName }}", "{{ ToolVersion }}")]
            public partial class {{ blockInfo.Name }} {
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
