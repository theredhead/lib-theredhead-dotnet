using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace theredhead.aspect;

/// <summary>
/// Provides extension methods for <see cref="ClassDeclarationSyntax"/>.
/// </summary>
public static class ClassDeclarationSyntaxExtensions
    {
        /// <summary>
        /// Determines whether the class declaration has the specified attribute.
        /// </summary>
        /// <typeparam name="T">The type of the attribute.</typeparam>
        /// <param name="cls">The class declaration syntax.</param>
        /// <returns><c>true</c> if the class declaration has the specified attribute; otherwise, <c>false</c>.</returns>
        public static bool HasAttribute<T>(this ClassDeclarationSyntax cls) where T : Attribute =>
            cls.AttributeLists.Any(al => al.Attributes.Any(a => typeof(T).Name.StartsWith(a.Name.ToString())));

        /// <summary>
        /// Determines whether the class declaration is partial.
        /// </summary>
        /// <param name="cls">The class declaration syntax.</param>
        /// <returns><c>true</c> if the class declaration is partial; otherwise, <c>false</c>.</returns>
        public static bool IsPartial(this ClassDeclarationSyntax cls) => 
            cls.Modifiers.Any(SyntaxKind.PartialKeyword);
        
        /// <summary>
        /// Gets the namespace of the class declaration.
        /// </summary>
        /// <param name="cls">The class declaration syntax.</param>
        /// <param name="model">The semantic model.</param>
        /// <returns>The namespace of the class declaration, or an empty string if the namespace cannot be determined.</returns>
        public static string GetNamespace(this ClassDeclarationSyntax cls, SemanticModel model) =>  
            model.GetDeclaredSymbol(cls)?.ContainingNamespace?.ToString() ?? "";

        /// <summary>
        /// Gets the properties declared in the class declaration.
        /// </summary>
        /// <param name="cls">The class declaration syntax.</param>
        /// <returns>An enumerable collection of property declaration syntax nodes.</returns>
        public static IEnumerable<PropertyDeclarationSyntax> GetProperties(this ClassDeclarationSyntax cls) =>
            cls.Members.OfType<PropertyDeclarationSyntax>();

        /// <summary>
        /// Gets the fields declared in the class declaration.
        /// </summary>
        /// <param name="cls">The class declaration syntax.</param>
        /// <returns>An enumerable collection of field declaration syntax nodes.</returns>
        public static IEnumerable<FieldDeclarationSyntax> GetFields(this ClassDeclarationSyntax cls) => 
            cls.Members.OfType<FieldDeclarationSyntax>();
    }

