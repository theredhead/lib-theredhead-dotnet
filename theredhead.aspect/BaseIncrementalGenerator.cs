using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace theredhead.aspect;

public abstract class BaseIncrementalGenerator<T> where T : DistilledSyntax, new()
{
    protected abstract bool IsNodeOfInterest(SyntaxNode node);
    protected virtual T Distill(SyntaxNode node, GeneratorSyntaxContext context) {
        var distilled = new T();
        distilled.Load(node, context);
        return distilled;
    }

    protected virtual string GetFileNameHint(T blockInfo) => $"{blockInfo.Name}.{GetType().Name}.generated.cs";

    protected abstract string GenerateCode(T blockInfo);
    
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

