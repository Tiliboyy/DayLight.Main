namespace DayLight.Core;

public class Logger
{
    public static void Info(object Info) => DayLightCore.NeuronLogger.Info(Info);
    public static void Error(object Error) => DayLightCore.NeuronLogger.Error(Error);
    public static void Debug(object Debug) => DayLightCore.NeuronLogger.Debug(Debug);
    public static void Warn(object Warning) => DayLightCore.NeuronLogger.Warn(Warning);

}
