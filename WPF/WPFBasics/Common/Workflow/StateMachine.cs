using System;
using System.Collections.Generic;
using WPFBasics.Common;
using WPFBasics.Common.Services;

namespace WPFBasics.Common.Workflow
{
    /// <summary>
    /// Generische, dynamische StateMachine f�r Workflows und Status�berg�nge.
    /// </summary>
    /// <typeparam name="TState">Typ des Status (z.B. Enum oder string).</typeparam>
    /// <typeparam name="TTrigger">Typ des Triggers (z.B. Enum oder string).</typeparam>
    public class StateMachine<TState, TTrigger>
    {
        /// <summary>
        /// Logger f�r Fehler und Informationen.
        /// </summary>
        public LogService Logger { get; init; }

        /// <summary>
        /// Gibt den aktuellen Status zur�ck.
        /// </summary>
        public TState State { get; private set; }

        /// <summary>
        /// Definiert die m�glichen Status�berg�nge.
        /// </summary>
        public Dictionary<(TState, TTrigger), TState> Transitions { get; } = new();

        /// <summary>
        /// Optional: Aktionen, die beim Eintritt in einen Status ausgef�hrt werden.
        /// </summary>
        public Dictionary<TState, Action> OnEnter { get; } = new();

        /// <summary>
        /// Optional: Aktionen, die beim Verlassen eines Status ausgef�hrt werden.
        /// </summary>
        public Dictionary<TState, Action> OnExit { get; } = new();

        /// <summary>
        /// Initialisiert die StateMachine mit einem Startstatus.
        /// </summary>
        /// <param name="initialState">Der Startstatus.</param>
        public StateMachine(TState initialState)
        {
            State = initialState;
        }

        /// <summary>
        /// F�gt einen Status�bergang hinzu.
        /// </summary>
        /// <param name="from">Ausgangsstatus.</param>
        /// <param name="trigger">Trigger f�r den �bergang.</param>
        /// <param name="to">Zielstatus.</param>
        public void AddTransition(TState from, TTrigger trigger, TState to)
        {
            Transitions[(from, trigger)] = to;
        }

        /// <summary>
        /// Registriert eine Aktion, die beim Eintritt in einen Status ausgef�hrt wird.
        /// </summary>
        public void OnEnterState(TState state, Action action)
        {
            OnEnter[state] = action;
        }

        /// <summary>
        /// Registriert eine Aktion, die beim Verlassen eines Status ausgef�hrt wird.
        /// </summary>
        public void OnExitState(TState state, Action action)
        {
            OnExit[state] = action;
        }

        /// <summary>
        /// F�hrt einen Status�bergang mit dem angegebenen Trigger aus.
        /// </summary>
        /// <param name="trigger">Der ausl�sende Trigger.</param>
        /// <returns>True, wenn der �bergang erfolgreich war, sonst false.</returns>
        public bool Fire(TTrigger trigger)
        {
            try
            {
                if (Transitions.TryGetValue((State, trigger), out var nextState))
                {
                    if (OnExit.TryGetValue(State, out var exitAction))
                        exitAction?.Invoke();

                    var previousState = State;
                    State = nextState;

                    if (OnEnter.TryGetValue(State, out var enterAction))
                        enterAction?.Invoke();

                    Logger?.Log($"StateMachine: �bergang von {previousState} nach {State} mit Trigger {trigger}.", LogLevel.Info);
                    return true;
                }
                else
                {
                    Logger?.Log($"StateMachine: Kein �bergang von {State} mit Trigger {trigger} definiert.", LogLevel.Warning);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Logger?.LogException(ex, "StateMachine.Fire");
                Logger?.LogException(ex, "StateMachine.Fire");
                return false;
            }
        }
    }
}
