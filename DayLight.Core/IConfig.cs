using Syml;

namespace DayLight.Core;

public interface  IConfig : IDocumentSection
{
    public bool Enabled { get; set; }
    public bool Debug { get; set; }
}
