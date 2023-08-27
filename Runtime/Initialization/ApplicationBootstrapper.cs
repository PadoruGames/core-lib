using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Padoru.Core.Diagnostics;
using Padoru.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = Padoru.Diagnostics.Debug;
using Object = UnityEngine.Object;

namespace Padoru.Core
{
    public static class ApplicationBootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static async void StartApplication()
        {
            var settings = Resources.Load<Settings>(Constants.SETTINGS_OBJECT_NAME);

            if(settings == null)
            {
                Debug.LogError("Failed to initialize application. Could not find settings object.");
                return;
            }

            ConfigLog(settings);

            if (settings.usePadoruConsole)
            {
                SetupPadoruConsole();
            }

            if (ShouldInitializeFramework(settings))
            {
                await SetupProjectContext(settings);
            }
        }

        private static void ConfigLog(Settings settings)
        {
            Debug.Configure(settings.logSettings, new UnityDefaultLogFormatter(), new UnityDefaultStackTraceFormatter());
            Debug.AddOutput(new UnityConsoleOutput());
        }

        private static void SetupPadoruConsole()
        {
            var commands = CreateConsoleCommands();
            
            var console = new Diagnostics.Console(
                new HeaderDrawer(), 
                new LogsAreaDrawer(), 
                new InputFieldDrawer(commands.Keys.ToArray()), 
                commands);

            Application.logMessageReceived += (message, trace, type) =>
            {
                console.Log(new ConsoleEntry()
                {
                    message = message + Environment.NewLine + trace,
                    logType = type.ToPadoruLogType(),
                    channel = ConsoleConstants.UNITY_LOG_CHANNEL,
                });
            };
            
            Locator.Register(console);

            ReportConsoleInitialization(commands);
        }

        private static async Task SetupProjectContext(Settings settings)
        {
            var projectContextPrefab = Resources.Load<Context>(settings.ProjectContextPrefabName);

            if (projectContextPrefab == null)
            {
                Debug.LogError("Could not find ProjectContext.");
                return;
            }

            Debug.Log($"Instantiating ProjectContext");
            var projectContext = Object.Instantiate(projectContextPrefab);
            Object.DontDestroyOnLoad(projectContext);

            Debug.Log($"ProjectContext registered to the Locator under the tag: {settings.ProjectContextPrefabName}");
            Locator.Register(projectContext, settings.ProjectContextPrefabName);

            Debug.Log($"Initializing ProjectContext");
            await projectContext.Init();
        }

        private static bool ShouldInitializeFramework(Settings settings)
        {
            var activeSceneName = SceneManager.GetActiveScene().name;

            return settings.scenes.Contains(activeSceneName);
        }

        private static Dictionary<string, ConsoleCommand> CreateConsoleCommands()
        {
            var data = AttributeUtils.GetTypesWithAttributeInstanced<ConsoleCommand, ConsoleCommandAttribute>();
            var commands = new Dictionary<string, ConsoleCommand>();

            foreach (var entry in data)
            {
                var commandName = entry.Attribute.CommandName.ToLower();
                commands.Add(commandName, entry.Instance);
            }

            return commands;
        }

        private static void ReportConsoleInitialization(Dictionary<string, ConsoleCommand> commands)
        {
            var sb = new StringBuilder();
            sb.Append("Debug Commands Console initialized. Commands:");
            sb.Append(Environment.NewLine);
            
            foreach (var command in commands)
            {
                sb.Append($"  - {command.Key}");
                sb.Append(Environment.NewLine);
            }
			
            Debug.Log(sb);
        }
    }
}