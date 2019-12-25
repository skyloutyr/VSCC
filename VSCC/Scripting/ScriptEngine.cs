using NLua;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using VSCC.Controls.Windows;
using VSCC.State;
using Xceed.Wpf.Toolkit;

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
            this.Lua.State.Encoding = System.Text.Encoding.UTF8;
            this.Lua["State"] = AppState.Current.State;
            this.Lua["Engine"] = this;
            this.Lua["Roll20"] = new Roll20Provider();
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

        public bool ShowContextWindow(string text, object table)
        {
            if (table is LuaTable t)
            {
                ScriptContextWindow scw = new ScriptContextWindow();
                scw.Label_Description.Content = text;
                Dictionary<object, string> reverseLookup = new Dictionary<object, string>();
                string[] allKeys = new string[t.Keys.Count];
                t.Keys.CopyTo(allKeys, 0);
                foreach (string key in allKeys)
                {
                    object value = t[key];
                    if (key is string title)
                    {
                        UIElement elem = null;
                        if (value is bool bo)
                        {
                            elem = new CheckBox() 
                            { 
                                IsChecked = bo, 
                                VerticalAlignment = VerticalAlignment.Top, 
                                HorizontalAlignment = HorizontalAlignment.Left,
                            };
                        }
                        else
                        {
                            if (value is string s)
                            {
                                elem = new TextBox()
                                {
                                    VerticalAlignment = VerticalAlignment.Top,
                                    HorizontalAlignment = HorizontalAlignment.Left,
                                    Width = 300,
                                    Text = s
                                };
                            }
                            else
                            {
                                if (value is int i)
                                {
                                    elem = new IntegerUpDown()
                                    {
                                        VerticalAlignment = VerticalAlignment.Top,
                                        HorizontalAlignment = HorizontalAlignment.Left,
                                        Value = i
                                    };
                                }
                                else
                                {
                                    if (value is float f)
                                    {
                                        elem = new SingleUpDown()
                                        {
                                            VerticalAlignment = VerticalAlignment.Top,
                                            HorizontalAlignment = HorizontalAlignment.Left,
                                            Value = f
                                        };
                                    }
                                    else
                                    {
                                        if (value is double d)
                                        {
                                            elem = new DoubleUpDown()
                                            {
                                                VerticalAlignment = VerticalAlignment.Top,
                                                HorizontalAlignment = HorizontalAlignment.Left,
                                                Value = d
                                            };
                                        }
                                        else
                                        {
                                            if (value is long l)
                                            {
                                                elem = new LongUpDown()
                                                {
                                                    VerticalAlignment = VerticalAlignment.Top,
                                                    HorizontalAlignment = HorizontalAlignment.Left,
                                                    Value = l
                                                };
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (elem != null)
                        {
                            reverseLookup.Add(elem, title);
                            WrapPanel wp = new WrapPanel()
                            {
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Top,
                                Width = scw.Width
                            };

                            wp.Children.Add(new Label()
                            {
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Top,
                                Content = title
                            });

                            wp.Children.Add(elem);
                            scw.WrapPanel_Content.Children.Add(wp);
                        }
                    }
                }

                bool b = scw.ShowDialog() ?? false;
                if (b)
                {
                    foreach (KeyValuePair<object, string> kv in reverseLookup)
                    {
                        if (kv.Key is CheckBox cb)
                        {
                            t[kv.Value] = cb.IsChecked ?? false;
                        }
                        else
                        {
                            if (kv.Key is TextBox tb)
                            {
                                t[kv.Value] = tb.Text;
                            }
                            else
                            {
                                if (kv.Key is IntegerUpDown iud)
                                {
                                    t[kv.Value] = iud.Value ?? 0;
                                }
                                else
                                {
                                    if (kv.Key is SingleUpDown fud)
                                    {
                                        t[kv.Value] = fud.Value ?? 0;
                                    }
                                    else
                                    {
                                        if (kv.Key is DoubleUpDown dud)
                                        {
                                            t[kv.Value] = dud.Value ?? 0;
                                        }
                                        else
                                        {
                                            if (kv.Key is LongUpDown lud)
                                            {
                                                t[kv.Value] = lud.Value ?? 0;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return b;
            }

            return false;
        }

        public void RegisterScript(string name, EventHandler handler)
        {
            if (!this.Scripts.ContainsKey(name))
            {
                this.Scripts.Add(name, handler);
            }
        }

        public bool ShowMessage(string title, string text)
        {
            return System.Windows.MessageBox.Show(text, title, MessageBoxButton.OKCancel) == MessageBoxResult.OK;
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

    public class Roll20Provider
    {
        public bool IsConnected() => Roll20.R20WSServer.Connected;

        public void PrettyRoll(string r1, string r2, string type, string action) => Roll20.R20WSServer.Roll(r1, r2, type, action);

        public void Roll(int numDice, int numSides) => Roll20.R20WSServer.Send(new Roll20.RollPacket() { NumDice = numDice, NumSides = numSides });

        public void Message(string message) => Roll20.R20WSServer.Send(new Roll20.MessagePacket() { Text = message });
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
