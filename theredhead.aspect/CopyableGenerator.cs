using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace theredhead.aspect;

[Generator]
public class CopyableGenerator : BaseIncrementalGenerator<ClassInfo>
{
    protected override bool IsNodeOfInterest(SyntaxNode node) => node is ClassDeclarationSyntax cls && cls.IsPartial() && cls.HasAttribute<CopyableAttribute>();

    protected override string GenerateCode(ClassInfo @class)
    {
        var sb = new StringBuilder();

        string ArgumentName(string propertyName) => propertyName.ToLowerInvariant();

        var copyMethodName = @class.Syntax?.GetAttributeArgument<CopyableAttribute>("copyMethodName") ?? "Copy";
        
        var methodParameters = @class.Properties.Select(p
            => $"{p.Type}? {ArgumentName(p.Identifier.Text)} = default");
        var methodParametersText = string.Join("\n\t\t\t,", methodParameters);

        var propertyAssignments = @class.Properties.Select(p
            => $"copy.{p.Identifier.Text} = {ArgumentName(p.Identifier.Text)} ?? this.{p.Identifier.Text};");
        var propertyAssignmentsText = string.Join("\n\t\t\t", propertyAssignments);

        // lang=cs
        return $$"""
        // <generated />
        #nullable enable
        namespace {{ @class.Namespace }} 
        {
            [global::System.CodeDom.Compiler.GeneratedCode("{{ ToolName }}", "{{ ToolVersion }}")]
            public partial class {{ @class.Name }} 
            {
                public {{ @class.Name }}() {}
                
                public {{ @class.Name }} {{ copyMethodName }}(
                     {{ methodParametersText }}
                ) {
                    var copy = new {{@class.Name}}();
                    {{ propertyAssignmentsText }}
                    return copy;
                }
            }
        }
        //EOF
        """;
    }}
