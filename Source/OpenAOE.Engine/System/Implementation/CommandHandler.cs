using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenAOE.Engine.Data;

namespace OpenAOE.Engine.System.Implementation
{
    internal interface ICommandHandler
    {
        bool CanHandle(Command command);
        void OnCommand(Command command);
    }

    /// <summary>
    /// Wrapper around a class that implements <c>Triggers.IOnCommand</c>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class CommandHandler<T> : ICommandHandler where T : Command
    {
        private readonly Triggers.IOnCommand<T> _handler;

        public CommandHandler(Triggers.IOnCommand<T> handler)
        {
            _handler = handler;
        }

        public bool CanHandle(Command command)
        {
            return command is T;
        }

        public void OnCommand(Command command)
        {
            _handler.OnCommand((T)command);
        }
    }

    internal static class CommandHandlerUtil
    {
        private static MethodInfo _createCommandHandlerMethod =
            typeof(CommandHandlerUtil).GetMethod(nameof(CreateCommandHandler),
                BindingFlags.NonPublic | BindingFlags.Static);

        public static IEnumerable<ICommandHandler> GetCommandHandlers(object obj)
        {
            foreach (var inter in obj.GetType().GetInterfaces())
            {
                if (inter.IsGenericType && inter.GetGenericTypeDefinition() == typeof(Triggers.IOnCommand<>))
                {
                    var method = _createCommandHandlerMethod.MakeGenericMethod(inter.GetGenericArguments().First());

                    yield return (ICommandHandler) method.Invoke(null, new [] {obj});
                }
            }
        }

        static ICommandHandler CreateCommandHandler<T>(Triggers.IOnCommand<T> handler) where T : Command
        {
            return new CommandHandler<T>(handler);
        }
    }
}