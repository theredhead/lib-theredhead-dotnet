using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace theredhead.aspect;

[Generator]
public class EqualsGenerator : BaseIncrementalGenerator<ClassInfo>
{
    protected override bool IsNodeOfInterest(SyntaxNode node) => node is ClassDeclarationSyntax cls && cls.IsPartial();

    protected bool HasEqualsMethod(ClassInfo @class) => @class.Methods.Any(m => m.Identifier.Text == "Equals" && m.ParameterList.Parameters.Count == 1);
    protected bool HasGetHashCodeMethod(ClassInfo @class) => @class.Methods.Any(m => m.Identifier.Text == "GetHashCode" && m.ParameterList.Parameters.Count == 0);

    protected override string GenerateCode(ClassInfo @class)
    {
        if (HasEqualsMethod(@class) && HasGetHashCodeMethod(@class)) return string.Empty;
        
        var equals = HasEqualsMethod(@class) ? string.Empty : $$"""
                public override bool Equals(object? other) 
                {
                    if (other is null) return false;
                    if (ReferenceEquals(this, other)) return true;
                    if (other.GetType() != this.GetType()) return false;
                    return Equals(({{ @class.Name }}) other);
                }

                public bool Equals({{ @class.Name }} other)
                {
                    return
                       {{ string.Join("\n\t\t\t&& ", @class.Properties.Select(p => $"this.{p.Identifier.Text} == other.{p.Identifier.Text}")) }};
                }
        """;
        var getHashCode = HasGetHashCodeMethod(@class) ? string.Empty : $$"""
                public override int GetHashCode() 
                {
                    return HashCode.Combine({{ string.Join(", ", @class.Properties.Select(p => $"this.{p.Identifier.Text}")) }});
                }
        """;

        // lang=cs
        return $$"""
        // <generated />
        #nullable enable
        namespace {{ @class.Namespace }} 
        {
            public partial class {{ @class.Name }} 
            {
                {{ equals }}
                {{ getHashCode }}
            }
        }
        //EOF
        """;
    }}