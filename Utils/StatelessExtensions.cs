using Stateless;
using System;
using System.Threading.Tasks;

namespace TeslaNE42Vision2D.Utils
{
    public static class StatelessExtensions
    {
        public static StateMachine<TState, TTrigger>.StateConfiguration OnExitFrom<TState, TTrigger>(
            this StateMachine<TState, TTrigger>.StateConfiguration stateConfiguration,
            TTrigger trigger,
            Action action)
        {
            return stateConfiguration.OnExit(transition =>
            {
                if (transition.Trigger.Equals(trigger))
                {
                    action();
                }
            });
        }

        public static StateMachine<TState, TTrigger>.StateConfiguration OnExitFrom<TState, TTrigger, TArg0, TArg1>(
          this StateMachine<TState, TTrigger>.StateConfiguration stateConfiguration,
          StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1> trigger,
          Action<TArg0, TArg1> action)
        {
            return stateConfiguration.OnExit(transition =>
            {
                if (transition.Trigger.Equals(trigger.Trigger) && transition.Parameters.Length >= 2)
                {
                    TArg0 arg0 = UnpackParameter<TArg0>(transition.Parameters, 0);
                    TArg1 arg1 = UnpackParameter<TArg1>(transition.Parameters, 1);
                    action(arg0, arg1);
                }
            });
        }

        public static StateMachine<TState, TTrigger>.StateConfiguration OnExitFromAsync<TState, TTrigger>(
           this StateMachine<TState, TTrigger>.StateConfiguration stateConfiguration,
           TTrigger trigger,
           Func<Task> action)
        {
            return stateConfiguration.OnExitAsync(transition =>
            {
                if (transition.Trigger.Equals(trigger))
                {
                    return action();
                }

                return Task.CompletedTask;
            });
        }

        public static StateMachine<TState, TTrigger>.StateConfiguration OnExitFromAsync<TState, TTrigger, TArg0, TArg1>(
            this StateMachine<TState, TTrigger>.StateConfiguration stateConfiguration,
            StateMachine<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1> trigger,
            Func<TArg0, TArg1, Task> action)
        {
            return stateConfiguration.OnExitAsync(transition =>
            {
                if (transition.Trigger.Equals(trigger.Trigger) && transition.Parameters.Length >= 2)
                {
                    TArg0 arg0 = UnpackParameter<TArg0>(transition.Parameters, 0);
                    TArg1 arg1 = UnpackParameter<TArg1>(transition.Parameters, 1);
                    return action(arg0, arg1);
                }

                return Task.CompletedTask;
            });
        }

        private static T UnpackParameter<T>(object[] parameters, int index)
        {
            if (parameters == null || parameters.Length <= index)
            {
                throw new ArgumentException("Invalid parameter index.");
            }

            return (T)parameters[index];
        }
    }
}
