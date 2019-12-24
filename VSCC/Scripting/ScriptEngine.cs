using NLua;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using VSCC.State;

namespace VSCC.Scripting
{
    public class ScriptEngine
    {
        public Dictionary<string, EventHandler> Scripts { get; } = new Dictionary<string, EventHandler>();

        public Lua Lua { get; } = new Lua();
        public bool AutocalcFrozen => AppState.Current.FreezeAutocalc;

        public static Lazy<ScriptEngine> Instance = new Lazy<ScriptEngine>(false);

        public static void Create()
        {
            if (Instance.IsValueCreated)
            {
                throw new Exception("Value already created");
            }

            if (Thread.CurrentThread != AppState.Current.AppThread)
            {
                throw new Exception("Can't create value on a non-ui thread");
            }

            Instance.Value.Setup();
        }

        private void Setup()
        {
            Log(LogLevel.Fine, "Initializing script engine.");
            this.Lua["State"] = AppState.Current.State;
            this.Lua["Engine"] = this;
            Log(LogLevel.Fine, "Looking for scripts...");
            int loaded = 0, errored = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts")))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts"));
            }

            foreach (string file in Directory.EnumerateFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts"), "*.lua", SearchOption.AllDirectories))
            {
                Log(LogLevel.Fine, "Loading script " + Path.GetFileName(file));
                try
                {
                    this.Lua.DoFile(file);
                    Log(LogLevel.Fine, "Loaded script " + Path.GetFileName(file));
                    ++loaded;
                }
                catch
                {
                    Log(LogLevel.Fatal, "Loading of script " + Path.GetFileName(file) + " failed, script errored.");
                    ++errored;
                }
            }

            sw.Stop();
            Log(LogLevel.Fine, $"Done loading scripts, took { sw.ElapsedMilliseconds }ms, loaded { loaded } scripts, { errored } errored.");
        }

        public void DoFile(string file)
        {
            if (!Path.IsPathRooted(file))
            {
                file = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", file));
            }

            try
            {
                this.Lua.DoFile(file);
            }
            catch (Exception e)
            {
                Log(LogLevel.Error, "Doing file " + Path.GetFileName(file) + " failed, script errored.");
                Log(LogLevel.Error, "The error was: " + e.GetType().Name);
                foreach (string s in e.StackTrace.Split('\n'))
                {
                    Log(LogLevel.Error, s);
                }
            }
        }

        public void Log(object message) => this.Log(message.ToString());

        public void Log(string message) => this.Log(LogLevel.None, message);

        public void Log(int level, string message) => this.Log((LogLevel)level, message);

        public void Log(LogLevel level, string message)
        {
            if (AppState.Current.TScripting.TextBlock_Log.IsInitialized)
            {
                AppState.Current.TScripting.TextBlock_Log.Text += this.PrefixFromLevel(level) + ": " + message + "\n";
            }
        }

        public void AddEventHandler(string name, EventHandler handler)
        {
            if (name.Equals("start") || name.Equals("startup") || name.Equals("onstartup"))
            {
                AppEvents.OnStartup += handler;
                return;
            }

            if (name.Equals("exit") || name.Equals("close") || name.Equals("stop") || name.Equals("onexit") || name.Equals("onclose") || name.Equals("onstop"))
            {
                AppEvents.OnExit += handler;
                return;
            }

            if (name.Equals("clear") || name.Equals("onclear"))
            {
                AppEvents.OnClear += handler;
                return;
            }

            if (name.Equals("save") || name.Equals("onsave"))
            {
                AppEvents.OnSave += new EventHandler<StringEventArgs>((o, args) => handler?.Invoke(o, args));
                return;
            }

            if (name.Equals("load") || name.Equals("onload"))
            {
                AppEvents.OnLoad += new EventHandler<StringEventArgs>((o, args) => handler?.Invoke(o, args));
                return;
            }

            this.Log(LogLevel.Warning, "Unrecognized event " + name);
        }

        public void RegisterScript(string name, EventHandler handler)
        {
            if (!this.Scripts.ContainsKey(name))
            {
                this.Scripts.Add(name, handler);
            }
        }

        private string PrefixFromLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Info:
                    return "[Info]";

                case LogLevel.Fine:
                    return "[Fine]";

                case LogLevel.Warning:
                    return "[Warning]";

                case LogLevel.Error:
                    return "[Error]";

                case LogLevel.Fatal:
                    return "[Fatal]";

                default:
                    return "[Unknown]";
            }
        }
    }

    public enum LogLevel
    {
        Info,
        Fine,
        Warning,
        Error,
        Fatal,
        None
    }
}
