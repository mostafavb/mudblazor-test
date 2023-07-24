using System.Numerics;
using System.Collections;


namespace Ui.WebAssembly.Utilities;

internal class TypeIdentifier
{
    private static readonly HashSet<Type> _numericTypes = new HashSet<Type>
        {        
            typeof(int),
            typeof(double),
            typeof(decimal),
            typeof(long),
            typeof(short),
            typeof(sbyte),
            typeof(byte),
            typeof(ulong),
            typeof(ushort),
            typeof(uint),
            typeof(float),
            typeof(BigInteger),
            typeof(int?),
            typeof(double?),
            typeof(decimal?),
            typeof(long?),
            typeof(short?),
            typeof(sbyte?),
            typeof(byte?),
            typeof(ulong?),
            typeof(ushort?),
            typeof(uint?),
            typeof(float?),
            typeof(BigInteger?)
        };

    public static bool IsIntegralNumber(Type? type) =>
        type == typeof(byte) || type == typeof(sbyte) ||
        type == typeof(short) || type == typeof(ushort) ||
        type == typeof(int) || type == typeof(uint) ||
        type == typeof(long) || type == typeof(ulong) ||
        type == typeof(BigInteger) ||
        type == typeof(byte?) || type == typeof(sbyte?) ||
        type == typeof(short?) || type == typeof(ushort?) ||
        type == typeof(int?) || type == typeof(uint?) ||
        type == typeof(long?) || type == typeof(ulong?) ||
        type == typeof(BigInteger?);

    internal static bool IsString(Type? type)
    {       
        if ((object)type == null)
        {
            return false;
        }

        if (type == typeof(string))
        {
            return true;
        }

        return false;
    }

    public static bool IsNumber(Type? type)
    {
        if ((object)type != null)
        {
            return _numericTypes.Contains(type);
        }

        return false;
    }

    public static bool IsEnum(Type? type)
    {
        if ((object)type == null)
        {
            return false;
        }

        if (type!.IsEnum)
        {
            return true;
        }

        return Nullable.GetUnderlyingType(type)?.IsEnum ?? false;
    }
    public static bool IsEnumerable(Type? type)
    {
        if ((object)type == null)
        {
            return false;
        }

       return typeof(IEnumerable).IsAssignableFrom(type)
                          || (type.IsGenericType && typeof(IEnumerable<>)
                            .IsAssignableFrom(type.GetGenericTypeDefinition()));       
    }

    public static bool IsDateTime(Type? type)
    {
        if ((object)type == null)
        {
            return false;
        }

        if (type == typeof(DateTime))
        {
            return true;
        }

        Type underlyingType = Nullable.GetUnderlyingType(type);
        if ((object)underlyingType != null)
        {
            return underlyingType == typeof(DateTime);
        }

        return false;
    }

    public static bool IsBoolean(Type? type)
    {
        if ((object)type == null)
        {
            return false;
        }

        if (type == typeof(bool))
        {
            return true;
        }

        Type underlyingType = Nullable.GetUnderlyingType(type);
        if ((object)underlyingType != null)
        {
            return underlyingType == typeof(bool);
        }

        return false;
    }

    public static bool IsGuid(Type? type)
    {
        if ((object)type == null)
        {
            return false;
        }

        if (type == typeof(Guid))
        {
            return true;
        }

        Type underlyingType = Nullable.GetUnderlyingType(type);
        if ((object)underlyingType != null)
        {
            return underlyingType == typeof(Guid);
        }

        return false;
    }

}