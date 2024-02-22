using System;

namespace Ara3D.Services.Experimental
{
    public class WindowsSystemEvent : IEvent
    {
        public DateTimeOffset Created = DateTimeOffset.Now;
    }

    // These are based on System Events 
    // https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.systemevents?view=dotnet-plat-ext-6.0

    public class DisplaySettingsChangedEvent : WindowsSystemEvent { }
    public class DisplaySettingsChangingEvent : WindowsSystemEvent { }
    public class EventsThreadShutdownEvent : WindowsSystemEvent { }
    public class InstalledFontsChangedEvent : WindowsSystemEvent { }
    public class PaletteChangedEvent : WindowsSystemEvent { }
    public class PowerModeChangedEvent : WindowsSystemEvent { }
    public class SessionEndedEvent : WindowsSystemEvent { }
    public class SessionEndingEvent : WindowsSystemEvent { }
    public class SessionChangingEvent : WindowsSystemEvent { }
    public class TimeChangedEvent : WindowsSystemEvent { }
    public class UserPreferenceChangedEvent : WindowsSystemEvent { }
    public class UserPreferenceChangingEvent : WindowsSystemEvent { }
    public class TimerEvent : WindowsSystemEvent
    {
        public IntPtr TimerId;
    }

    // TODO: i need a system event service that registers for all of these, and deallocates them at the appropriate time. 
}