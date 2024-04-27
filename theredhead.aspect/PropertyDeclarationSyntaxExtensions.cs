using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace theredhead.aspect;

/// <summary>
/// Provides extension methods for working with <see cref="PropertyDeclarationSyntax"/>.
/// </summary>
public static class PropertyDeclarationSyntaxExtensions
{
    /// <summary>
    /// Determines whether the property has the specified attribute.
    /// </summary>
    /// <typeparam name="T">The type of the attribute.</typeparam>
    /// <param name="prop">The property declaration syntax.</param>
    /// <returns><c>true</c> if the property has the specified attribute; otherwise, <c>false</c>.</returns>
    public static bool HasAttribute<T>(this PropertyDeclarationSyntax prop) where T : Attribute => 
        prop.AttributeLists.Any(al => al.Attributes.Any(a => a.Name.ToString() == typeof(T).Name)); 

    /// <summary>
    /// Determines whether the property has a setter.
    /// </summary>
    /// <param name="prop">The property declaration syntax.</param>
    /// <returns><c>true</c> if the property has a setter; otherwise, <c>false</c>.</returns>
    public static bool HasSetter(this PropertyDeclarationSyntax prop) => 
        prop.Modifiers.Any(SyntaxKind.SetKeyword);

    /// <summary>
    /// Gets the name of the property.
    /// </summary>
    /// <param name="prop">The property declaration syntax.</param>
    /// <returns>The name of the property.</returns>
    public static string GetPropertyName(this PropertyDeclarationSyntax prop) => 
        prop.Identifier.Text;

    /// <summary>
    /// Gets the type of the property.
    /// </summary>
    /// <param name="prop">The property declaration syntax.</param>
    /// <returns>The type of the property.</returns>
    public static string GetPropertyType(this PropertyDeclarationSyntax prop) => 
        prop.Type.ToString();
            
    /// <summary>
    /// Determines whether the property is an auto property.
    /// </summary>
    /// <param name="prop">The property declaration syntax.</param>
    /// <returns><c>true</c> if the property is an auto property; otherwise, <c>false</c>.</returns>
    public static bool IsAutoProperty(this PropertyDeclarationSyntax prop) => 
        prop.Modifiers.Any(SyntaxKind.PublicKeyword) && prop.Modifiers.Any(SyntaxKind.GetKeyword) && prop.Modifiers.Any(SyntaxKind.SetKeyword);
            
    /// <summary>
    /// Determines whether the property is a read-only property.
    /// </summary>
    /// <param name="prop">The property declaration syntax.</param>
    /// <returns><c>true</c> if the property is a read-only property; otherwise, <c>false</c>.</returns>
    public static bool IsReadOnlyProperty(this PropertyDeclarationSyntax prop) => 
        prop.Modifiers.Any(SyntaxKind.PublicKeyword) && prop.Modifiers.Any(SyntaxKind.GetKeyword) && !prop.Modifiers.Any(SyntaxKind.SetKeyword);
}

