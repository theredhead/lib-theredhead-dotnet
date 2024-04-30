using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace theredhead.aspect;

public static class FieldDeclarationSyntaxExtensions
{
    public static bool HasAttribute<T>(this FieldDeclarationSyntax field) where T : Attribute
        => field.AttributeLists.Any(al => al.Attributes.Any(a => a.Name.ToString() == typeof(T).Name));

    // public static string GetFieldType(this FieldDeclarationSyntax field) =>
    //     field. Type.ToString();

}