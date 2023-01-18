using System;

namespace Recipes.Presentation.Exceptions;

public class RecipeEditorException : Exception
{
    public RecipeEditorException() { }

    public RecipeEditorException(string? message) : base(message) { }

    public RecipeEditorException(string? message, Exception? innerException) : base(message, innerException) { }
}