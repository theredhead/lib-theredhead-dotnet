using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace  theredhead.aspect;

public abstract class Generator<T> : IIncrementalGenerator where T : DistilledSyntax, new ()
{
    protected virtual string ToolName => $"{GetType().Namespace}.{GetType().Name}";
    protected virtual string ToolVersion => GetType().Assembly.ImageRuntimeVersion;

    protected virtual bool IsNodeOfInterest(SyntaxNode node) =>
        node is ClassDeclarationSyntax cls && cls.IsPartial() && cls.HasAttribute<CopyableAttribute>();

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider.CreateSyntaxProvider(
            predicate: (node, _) => IsNodeOfInterest(node),
            transform: (ctx, _) => Distill((ClassDeclarationSyntax)ctx.Node, ctx)
        ).Where(m => m is not null);

        context.RegisterSourceOutput(provider, Generate);
    }

    protected virtual T Distill(SyntaxNode node, GeneratorSyntaxContext context) {
        var distilled = new T();
        distilled.Load(node, context);
        return distilled;
    }

    private void Generate(SourceProductionContext context, T distilled)
    {
        var fileNameHint = $"{distilled.Name}.{GetType().Name}.g.cs";
        context.AddSource(fileNameHint, SourceText.From(
            GenerateCode(distilled), Encoding.UTF8
        ));
    }

    protected abstract string GenerateCode(T distilled);
}
