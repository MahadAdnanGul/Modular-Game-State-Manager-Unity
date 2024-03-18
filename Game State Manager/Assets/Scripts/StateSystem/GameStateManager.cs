using System;
using UnityEngine;
using static SingletonShortcuts;

namespace StateSystem
{
    /// <summary>
    /// store state & handle gameflow
    /// Register your events here to detect game state changes
    /// You can ask for current state here
    /// </summary>
    [AddComponentMenu("CustomModules/GameStateManager")]
    
    //Give it an early execution order to ensure the system is ready before any other object tries to reference it.
    [DefaultExecutionOrder(-9997)]
    public class GameStateManager : MonoBehaviour
    {
        
        public EGameState State { get; private set;}
        private EGameState m_aboutToEnterState;

        // On state changed (new state, old state)
        public OrderedAction<EGameState, EGameState> OnStateChanged { get; } = new OrderedAction<EGameState, EGameState>();

        /// <summary>
        /// On state entered (new state, old state)
        /// </summary>
        private OrderedAction<EGameState, EGameState>[] m_onEnterStateEvents;
        
        /// <summary>
        /// On state exited (old state, new state)
        /// </summary>
        private OrderedAction<EGameState, EGameState>[] m_onExitStateEvents;

        /// <summary>
        /// Time since entered this state (in seconds)
        /// </summary>
        private float[] m_timeSinceStateEnter;

        private void Awake()
        {
            InitSingleton(this);

            int stateCount = Enum.GetNames(typeof(EGameState)).Length;
            m_onEnterStateEvents = new OrderedAction<EGameState, EGameState>[stateCount];
            for (int i = 0; i < m_onEnterStateEvents.Length; i++)
            {
                m_onEnterStateEvents[i] = new OrderedAction<EGameState, EGameState>();
            }
            
            m_onExitStateEvents = new OrderedAction<EGameState, EGameState>[stateCount];
            for (int i = 0; i < m_onExitStateEvents.Length; i++)
            {
                m_onExitStateEvents[i] = new OrderedAction<EGameState, EGameState>();
            }
            
            
            m_timeSinceStateEnter = new float[stateCount];
        }

        private void Start()
        {
            SetState(EGameState.LOADING);
        }

        private void Update()
        {
            m_timeSinceStateEnter[(int) State] += Time.unscaledDeltaTime;
        }

        private void InternalOnStateChanged(EGameState newState, EGameState oldState)
        {
            // Reset timer for this state
            m_timeSinceStateEnter[(int) newState] = 0;
            
            OnStateChanged.Invoke(newState, State);

            m_onEnterStateEvents[(int) newState].Invoke(newState, State);

            State = newState;
        }
        
        public void SetState(EGameState newState)
        {
            if (State == newState || newState == m_aboutToEnterState)
            {
                Debug.LogError($"Trying to enter state {newState} but we're already in that state!");
                return;
            }

            m_aboutToEnterState = newState;
            
            InternalOnStateChanged(newState,State);
        }
        

        public void RegisterOnEnterState(EGameState state, OrderedAction<EGameState, EGameState>.Del callback, int priority = 0)
        {
            m_onEnterStateEvents[(int) state].Add(callback, priority);
        }

        public void RegisterOnExitState(EGameState state, OrderedAction<EGameState, EGameState>.Del callback, int priority = 0)
        {
            m_onExitStateEvents[(int)state].Add(callback, priority);
        }

        public void UnregisterOnEnterState(EGameState state, OrderedAction<EGameState, EGameState>.Del callback)
        {
            m_onEnterStateEvents[(int)state].Remove(callback);
        }

        public void UnregisterOnExitState(EGameState state, OrderedAction<EGameState, EGameState>.Del callback)
        {
            m_onExitStateEvents[(int)state].Remove(callback);
        }

        public float GetTimeSinceStateEnter(EGameState state)
        {
            return m_timeSinceStateEnter[(int) state];
        }
        
    }
}
