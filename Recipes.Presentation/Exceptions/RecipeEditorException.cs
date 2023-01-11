using System;

namespace Recipes.Presentation.Exceptions;

public class RecipeEditorException : Exception
{
    public RecipeEditorException()
    {
    }

    public RecipeEditorException(string? message) : base(message)
    {
    }
}