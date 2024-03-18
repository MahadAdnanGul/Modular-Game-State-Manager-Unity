# Modular Game State Manager - Unity

This is a game-independant game state management system. Feel free to incorporate into your game for implementing an efficient, bug-free and customizable state system solution.
The implementation supports event handling for entry/exit to and from any two states and allows the user to set priorities incase they want certain methods to execute before another using a modified UnityAction implementation called Ordered Action.
Includes a custom singleton and collection utility class.

Steps to use:

1. Add classes to project
2. Define your states in the EGameState enum.
3. Add the GameStateManager script to a game object. (Note that this script is a singleton)
4. Register/Unregister OnExit or OnEnter events between any two states.
5. Use SetState function to set a desired state and trigger all corresponding actions to that state transition.
 
