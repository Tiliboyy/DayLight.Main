using Neuron.Core;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DayLight.Core.API;

public class StaticUtils
{
    
    
    public static List<T> GetEnumValues<T>()
    {
        return Enum.GetValues(typeof(T)).Cast<T>().ToList();
    }
    public static string GetColorHexCode(string color) => !Enum.TryParse(color, true, out Misc.PlayerInfoColorTypes colorEnum)
        ? Colors[Misc.PlayerInfoColorTypes.White]
        : Colors[colorEnum];
    public static Dictionary<Misc.PlayerInfoColorTypes, string> Colors { get; } = Misc.AllowedColors;

    /// <summary>
    /// Returns an instance of the specified object by either resolving it using
    /// ninject bindings (I.e. the object is already present in the ninject kernel)
    /// or by creating a new instance of the type using the ninject kernel making
    /// injection usable.
    /// </summary>
    /// <exception cref=""></exception>
    public static T Get<T>()
    {
        if (Exist<T>()) return Globals.Kernel.Get<T>();
        Logger.Warn(typeof(T).Name + " was requested but doesn't exist inside the kernel.");
        return default;
    }

    public static T GetOrCreate<T>(bool bind = true)
        => Exist<T>() ? Get<T>() : Create<T>(bind);

    public static T Create<T>(bool bind)
    {
        var instance = Globals.Get<T>();
        if (bind) Bind(instance);
        Inject(instance);
        return instance;
    }

    public static void Bind<T>(T instance) => Globals.Kernel.Bind<T>().ToConstant(instance).InSingletonScope();
    
    public static bool Exist<T>() => Globals.Kernel.GetBindings(typeof(T)).Any();

    /// <inheritdoc cref="Get{T}"/>
    public static object Get(Type type)
    {
        if (Exist(type)) return Globals.Kernel.Get(type);
        Logger.Warn(type.Name + " was requested but doesn't exist inside the kernel.");
        return null;
    }

    public static object GetOrCreate(Type type, bool bind = true)
        => Exist(type) ? Get(type) : Create(type, bind);

    public static object Create(Type type, bool bind)
    {
        var instance = Globals.Kernel.Get(type);
        if (bind) Bind(type, instance);
        Inject(instance);
        return instance;
    }

    public static void Bind(Type type, object instance) =>
        Globals.Kernel.Bind(type).ToConstant(instance).InSingletonScope();

    public static bool Exist(Type type) => Globals.Kernel.GetBindings(type).Any();
    
    public static void Inject(object instance) => Globals.Kernel.Inject(instance);
}
