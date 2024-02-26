namespace Reapit.Services.Template.Core.UseCases;

public static class ValidationMessages
{
    // Constants
    
    public const string ValueMustBeGreaterThanZero = "Value must be greater than zero";
    
    public const string ValueMustBeUnique = "Value must be unique";
    
    // Methods
    
    public static string ValueExceedsMaximumOf(int max) => $"Value exceeds maximum value of {max}";
    
    public static string ValueExceedsMaximumLengthOf(int max) => $"Value exceeds maximum length of {max} characters";
}