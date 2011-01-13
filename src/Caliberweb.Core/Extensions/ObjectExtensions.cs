using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Caliberweb.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static IDisposable SubscribeToAllEvents<T>(this T instance, Action<string> action)
        {
            var events = typeof(T).GetEvents().ToList();
            var map = new Dictionary<EventInfo, Delegate>(events.Count);

            events.ForEach(e=>
            {
                var @delegate = CreateDelegate(e, action, map);
                e.AddEventHandler(instance, @delegate);
            });

            return new DisposableAction(()=> events.ForEach(e => e.RemoveEventHandler(instance, map[e])));
        }

        private static Delegate CreateDelegate(EventInfo @event, Action<string> action, IDictionary<EventInfo, Delegate> map)
        {
            const string METHOD = "Invoke";

            var handlerType = @event.EventHandlerType;
            
            var eventParams = handlerType.GetMethod(METHOD).GetParameters();

            var parameters = eventParams.Select(p => Expression.Parameter(p.ParameterType, "x"));
            
            var body = Expression.Call(Expression.Constant(action),
                                       action.GetType().GetMethod(METHOD),
                                       Expression.Constant(@event.Name, typeof (string)));

            var lambda = Expression.Lambda(body, parameters.ToArray());
            
            var @delegate = Delegate.CreateDelegate(handlerType, lambda.Compile(), METHOD, false);
            
            map.Add(@event, @delegate);

            return @delegate;
        }
    }
}