using System.Reflection;

namespace Application.Helper;

public static class FindPropertyHelper
{
    public static bool FindProperty<T>(string? fieldName)
    {
        if (string.IsNullOrEmpty(fieldName))
            return false;
        var type = typeof(T);
        return type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
            .Any(p => string.Equals(p.Name, fieldName, StringComparison.OrdinalIgnoreCase));
    }
}