using System;
using System.IO;
using Ninject;
using Ninject.Extensions.Logging.NLog4;
using Ninject.Extensions.Logging.NLog4.Infrastructure;
using NLog;
using OpenAOE.Engine;
using OpenAOE.Games.AGE2;
using OpenAOE.Services.Config;
using OpenAOE.Services.Config.Implementation;

namespace OpenAOE
{
    public static class Program
    {
        static void Main(string[] args)
        {
            // Create a logger for the bootstrap process
            var log = LogManager.GetCurrentClassLogger(typeof(Program));
            log.Info("Starting...");

            // Read default config file
            var configPath = Path.Combine(Environment.CurrentDirectory, "config.toml");
            log.Info($"Reading config from {configPath}");

            string config = null;

            if (!File.Exists(configPath))
            {
                log.Warn("Config file not found. Defaults will be used.");
            }
            else
            {
                try
                {
                    config = File.ReadAllText(configPath);
                }
                catch (Exception e)
                {
                    log.Error(e, $"Error while reading config file at {configPath}");
                }
            }

            // Create the global config service
            var configService = new ConfigService(new NLogLogger(typeof(ConfigService)),
                new TomlConfigValueProvider(config ?? ""));

            var settings = new NinjectSettings()
            {
                LoadExtensions = false
            };

            // Bootstrap DI and run
            using (var kernel = new StandardKernel(settings, new AppModule(), new EngineModule(),
                new Age2Module(), new NLogModule()))
            {
                kernel.Bind<IConfigService>().ToConstant(configService);

                using (var app = kernel.Get<Application>())
                {
                    app.Run();
                }
            }

#if DEBUG
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey(true);
#endif
        }

        /*
        public static List<EntityTemplate> TestTemplates = new List<EntityTemplate>()
        {
            new EntityTemplate("Unit", new List<IComponent>()
            {
                new Transform() {  },
                new Movable()
                {
                    TargetPosition = null,
                    MoveSpeed = 5f
                }
            })
        };

        public static List<EntityData> TestData = new List<EntityData>()
        {
            new EntityData(0, new List<IComponent>()
            {
                new Transform() {  },
                new Movable()
                {
                    TargetPosition = null,
                    MoveSpeed = 8f
                }
            })
        };


        static void Main(string[] args)
        {
            var context = new StandardKernel(
                new NinjectSettings()
                {
                    LoadExtensions = false
                }, new EngineModule(), new Games.AGE2.Module(), new NLogModule());


            var log = context.Get<ILoggerFactory>().GetCurrentClassLogger();
            log.Info("Starting");

            var engineFactory = context.Get<IEngineFactory>();

            log.Info("Creating Engine"); 
            var engine = engineFactory.Create(TestData, TestTemplates);

            for (var i = 0; i < 100; ++i)
            {
                EngineTickInput input;
                if (i == 1)
                {
                    input = new EngineTickInput(new Command[] {new MoveCommand(0, new FixVector2(20, 20))});
                }
                else
                {
                    input = new EngineTickInput();
                }

                var tick = engine.Tick(input);
                tick.Start();
                tick.Wait();
                engine.Synchronize();
            }

            log.Info("Done");
            Console.ReadKey(true);
            log.Info("Exiting");
        }*/
    }
}
