using Botticelli.Framework.Exceptions;

namespace Botticelli.Framework.SendOptions
{
    /// <summary>
    /// Additional options for sending messages for partial messenger
    /// (for example you can use InlineKeyboardMarkup  as T)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SendOptionsBuilder<T>
        where T : class
    {
        private T innerObject = default;

        protected SendOptionsBuilder()
        {
        }

        public T Create(params object[] args)
        {
            if (innerObject != default) 
                throw new BotException($"You shouldn't use {nameof(Create)}() method twice!");

            var constructors = typeof(T)
                               .GetConstructors()
                               .Where(c => c.IsPublic);

            // no params? ok => let's seek a parameterless constructor!
            if ((args == null || !args.Any()) && constructors.Any(c => !c.GetParameters().Any()))
            {
                innerObject = Activator.CreateInstance<T>();

                return innerObject;
            }

            // Let's see if we can process parameter set and put it to a constructor|initializer
            foreach (var c in constructors)
            {
               // c.CallingConvention = 
            }



            return innerObject;
        }

        public T Set(Func<T> func)
        {
            func?.Invoke();

            return innerObject;
        }

        public T Build()
            => innerObject;

        public static SendOptionsBuilder<T> CreateBuilder() => new();
    }
}
