using System;

namespace VSCC.State
{
    public class StringEventArgs : EventArgs
    {
        public string Value { get; set; }

        public StringEventArgs(ref string s) => this.Value = s;
    }

    public class AppEvents
    {
        public static event EventHandler OnStartup;
        public static event EventHandler OnExit;

        public static event EventHandler<StringEventArgs> OnLoad;
        public static event EventHandler<StringEventArgs> OnSave;
        public static event EventHandler OnClear;

        public static void InvokeSave(ref string save) => OnSave?.Invoke(null, new StringEventArgs(ref save));

        public static void InvokeLoad(ref string save) => OnLoad?.Invoke(null, new StringEventArgs(ref save));

        public static void InvokeClear() => OnClear?.Invoke(null, EventArgs.Empty);

        public static void InvokeStartup() => OnStartup?.Invoke(null, EventArgs.Empty);

        public static void InvokeExit() => OnExit?.Invoke(null, EventArgs.Empty);
    }
}
