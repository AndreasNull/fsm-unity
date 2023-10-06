# Finite State Machine
This is a state machine solution on demand. The base solution consists of the below classes. There are more types and classes hidden to this solution. This solution is very close to Unity’s Animator solution. The idea is that we define states, parameters and the transitions between the states using the parameters in order to define conditions.

## Structure
This is a typical definition structure for a state machine.
1. Define states as enums
2. Define parameters as enums
3. Add states 
4. Register parameter
5. Add transitions 

### State Machine
To create a state machine we use the `StateMachineManager`. After we define the state machine we have to call the `StateMachineManager::StartStateMachine()` method to start the state machine.

## Examples
### Simple State Machine
A simple example of a door state machine with 2 states `Opened` and `Closed`. This state machine manages an Animator component.
```C#
public class DoorStateMachineExample : MonoBehaviour
{
    #region Inspector
    [SerializeField] Animator m_Animator;
    #endregion

    private StateMachineManager m_SMManager;
    private enum States { Opened, Closed }
    private enum Params { SpacePressed }

    // Start is called before the first frame update
    void Start()
    {
        // create state machine
        m_SMManager = StateMachineManager.Create(this.gameObject);

        // add states
        m_SMManager.AddState(new ClosedState(), States.Closed, m_Animator);
        m_SMManager.AddState(new OpenedState(), States.Opened, m_Animator);

        // register trigger parameter
        m_SMManager.RegisterTriggerParam(Params.SpacePressed);

        // add transitions
        m_SMManager.AddTransition(States.Opened, States.Closed, new TriggerCondition(Params.SpacePressed)); // first state
        m_SMManager.AddTransition(States.Closed, States.Opened, new TriggerCondition(Params.SpacePressed));

        // start state machine
        m_SMManager.StartStateMachine();
    }

    private void Update()
    {
        // fire SpacePressed trigger param on Space key pressed
        if (Input.GetKeyDown(KeyCode.Space))
            m_SMManager.SetTriggerParam((int)Params.SpacePressed);
    }

    /// <summary>
    /// Opened state.
    /// </summary>
    private class OpenedState : State
    {
        Animator m_Animator;

        // get animator component from payload
        public override void OnInit() => m_Animator = (Animator)payload[0];
        public override void OnEnter() => m_Animator.SetBool("Open", true);
        
    }

    /// <summary>
    /// Closed state.
    /// </summary>
    private class ClosedState : State
    {
        Animator m_Animator;

        // get animator component from payload
        public override void OnInit() => m_Animator = (Animator)payload[0];
        public override void OnEnter() => m_Animator.SetBool("Open", false);
        
    }
}
```
### Simple State Machine Simplified
The same state machine as above, but with one class-state defined using payloads.
```c#
public class DoorStateMachineSimplifiedExample : MonoBehaviour
{
    #region Inspector
        [SerializeField] Animator m_Animator;
    #endregion

        private StateMachineManager m_SMManager;
    private enum States { Opened, Closed }
    private enum Params { SpacePressed }

    // Start is called before the first frame update
    void Start()
    {
        // create state machine
        m_SMManager = StateMachineManager.Create(this.gameObject);

        // add states
        m_SMManager.AddState(new AnimatorBoolState(), States.Closed, m_Animator, "Open", false);
        m_SMManager.AddState(new AnimatorBoolState(), States.Opened, m_Animator, "Open", true);

        // register trigger parameter
        m_SMManager.RegisterTriggerParam(Params.SpacePressed);

        // add transitions
        m_SMManager.AddTransition(States.Opened, States.Closed, new TriggerCondition(Params.SpacePressed));
        m_SMManager.AddTransition(States.Closed, States.Opened, new TriggerCondition(Params.SpacePressed));

        // start state machine
        m_SMManager.StartStateMachine();
    }

    private void Update()
    {
        // fire SpacePressed trigger param on Space key pressed
        if (Input.GetKeyDown(KeyCode.Space))
            m_SMManager.SetTriggerParam((int)Params.SpacePressed);
    }

    /// <summary>
    /// Opened state.
    /// </summary>
    private class AnimatorBoolState : State
    {
        Animator m_Animator;
        string m_AnimParamName;
        bool m_AnimParamValue;

        public override void OnInit()
        {
            // get animator component from payload
            m_Animator = (Animator)payload[0];
            // get animator param name
            m_AnimParamName = (string)payload[1];
            // get animator param value
            m_AnimParamValue = (bool)payload[2];
        }

        public override void OnEnter() => m_Animator.SetBool(m_AnimParamName, m_AnimParamValue);

    }
}
```

### Simple State Machine With Generic States
Same as the above state machine, replacing the defined state-class with the generic `ActionState`.
```c#
public class DoorStateMachineGenericStatesExample : MonoBehaviour
{
    #region Inspector
    [SerializeField] Animator m_Animator;
    #endregion

    private StateMachineManager m_SMManager;
    private enum States { Opened, Closed }
    private enum Params { SpacePressed }

    // Start is called before the first frame update
    void Start()
    {
        // create state machine
        m_SMManager = StateMachineManager.Create(this.gameObject);

        // add generic action states
        m_SMManager.AddActionState(States.Closed, () => m_Animator.SetBool("Open", false));
        m_SMManager.AddActionState(States.Opened, () => m_Animator.SetBool("Open", true));

        // register trigger parameter
        m_SMManager.RegisterTriggerParam(Params.SpacePressed);

        // add transitions
        m_SMManager.AddTransition(States.Opened, States.Closed, new TriggerCondition(Params.SpacePressed));
        m_SMManager.AddTransition(States.Closed, States.Opened, new TriggerCondition(Params.SpacePressed));

        // start state machine
        m_SMManager.StartStateMachine();
    }

    private void Update()
    {
        // fire SpacePressed trigger param on Space key pressed
        if (Input.GetKeyDown(KeyCode.Space))
            m_SMManager.SetTriggerParam((int)Params.SpacePressed);
    }
}
```
Same as above but now the door is closed automatically after x seconds.

```c#
public class DoorStateMachineGenericStatesAutoExample : MonoBehaviour
{
    #region Inspector
    [SerializeField] Animator m_Animator;
    [SerializeField] float m_Delay;
    #endregion

        private StateMachineManager m_SMManager;
    private enum States { Opened, Delay, Closed }
    private enum Params { SpacePressed }

    // Start is called before the first frame update
    void Start()
    {
        // create state machine
        m_SMManager = StateMachineManager.Create(this.gameObject);

        // add generic action states
        m_SMManager.AddActionState(States.Closed, () => m_Animator.SetBool("Open", false));
        m_SMManager.AddActionState(States.Opened, () => m_Animator.SetBool("Open", true));
        // add generic delay state
        m_SMManager.AddDelayState(States.Delay, m_Delay, States.Closed);

        // register trigger parameter
        m_SMManager.RegisterTriggerParam(Params.SpacePressed);

        // add transitions
        m_SMManager.AddTransition(States.Closed, States.Opened, new TriggerCondition(Params.SpacePressed));
        m_SMManager.AddTransition(States.Opened, States.Delay);

        // start state machine
        m_SMManager.StartStateMachine();
    }

    private void Update()
    {
        // fire SpacePressed trigger param on Space key pressed
        if (Input.GetKeyDown(KeyCode.Space))
            m_SMManager.SetTriggerParam((int)Params.SpacePressed);
    }
}
```



# Credits

**Andreas Diktyopoulos**

andreas.diktyopoylos(at)gmail.com

