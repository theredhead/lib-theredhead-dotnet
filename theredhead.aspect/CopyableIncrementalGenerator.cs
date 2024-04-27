using System.Text;
using System.Xml.Serialization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace theredhead.aspect;

[AttributeUsage(AttributeTargets.Class)]
public class CopyableAttribute : Attribute {
}

[Generator]
public class CopyableIncrementalGenerator : IIncrementalGenerator
{
    private class ClassInfo {
        public ClassDeclarationSyntax Syntax {get;}
        public string Name {get; set;} = "";
        public string Namespace {get; set;} = "";

        public ClassInfo(ClassDeclarationSyntax syntax)
        {
            Syntax = syntax;
        }
    }

    private static bool isPartialClass(SyntaxNode node) => 
        node is ClassDeclarationSyntax cls && cls.Modifiers.Any(SyntaxKind.PartialKeyword);

    private static bool hasAttribute<T>(SyntaxNode node) where T : Attribute => 
        node is ClassDeclarationSyntax cls && cls.AttributeLists.Any(al => al.Attributes.Any(a => a.Name.ToString() == typeof(T).Name));

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // act on all partial classes
        // context.RegisterForSyntaxNotifications(() => new PartialClassSyntaxReceiver());

        var provider = context.SyntaxProvider.CreateSyntaxProvider(
            predicate: static (node, _) => isPartialClass(node), // && hasAttribute<CopyableAttribute>(node),
            transform: static (ctx, _) => Transform((ClassDeclarationSyntax)ctx.Node, ctx)
        ).Where(m => m is not null);

        context.RegisterSourceOutput(provider, Generate);
    }

    private void Generate(SourceProductionContext context, ClassInfo @class)
    {
        context.AddSource($"{@class.Name}.Copyable.g.cs", SourceText.From(
            GeneratePartial(@class), Encoding.UTF8
        ));
    }

    private static ClassInfo Transform(ClassDeclarationSyntax node, GeneratorSyntaxContext ctx)
    {
        return new ClassInfo(node) 
        {
            Name = node.Identifier.Text,
            Namespace = ctx.SemanticModel.GetDeclaredSymbol(node)?.ContainingNamespace?.ToString() ?? ""
        };
    }

    private static string GeneratePartial(ClassInfo @class)
    {
        var sb = new StringBuilder();

        sb.AppendLine("// <generated />");
        sb.AppendLine($"namespace {@class.Namespace} {{");
        sb.AppendLine(" [global::System.Diagnostics.DebuggerNonUserCode]");
        sb.AppendLine($"    public partial class {@class.Name} {{");
        sb.AppendLine($"         public {@class.Name} Copy() {{");
        sb.AppendLine($"             var copy = new {@class.Name}();");   
        // foreach (var member in @class.Syntax.Members)
        // {
        //     if (member is PropertyDeclarationSyntax prop)
        //     {
        //         sb.AppendLine($"        copy.{prop.Identifier.Text} = {prop.Identifier.Text};");
        //     }
        // }
        sb.AppendLine("             return copy;");
        sb.AppendLine("         }");
        sb.AppendLine("    }");
        sb.AppendLine("}");

        return sb.ToString();
    }
}


public abstract class DistilledSyntax
{
    public abstract void Load(SyntaxNode node, GeneratorSyntaxContext context);
}

public abstract class BaseIncrementalGenerator<T> : IIncrementalGenerator where T : DistilledSyntax, new()
{
    protected abstract bool IsNodeOfInterest(SyntaxNode node);{
    protected virtual T Distill(SyntaxNode node, GeneratorSyntaxContext context) {
        var distilled = new T();
        distilled.Load(node, context);
        return distilled;
    }
    protected abstract string GenerateCode(T @class);
    
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider.CreateSyntaxProvider(
            predicate: (node, _) => IsNodeOfInterest(node),
            transform: (ctx, _) => Distill((ClassDeclarationSyntax)ctx.Node, ctx)
        ).Where(m => m is not null);

        context.RegisterSourceOutput(provider, Generate);
    }

    private void Generate(SourceProductionContext context, T blockInfo)
    {
        var fileNameHint = $"{blockInfo.Name}.{GetType().Name}.g.cs";
        context.AddSource(fileNameHint, SourceText.From(
            GenerateCode(blockInfo), Encoding.UTF8
        ));
    }
}