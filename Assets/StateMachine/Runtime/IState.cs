namespace ADikt.StateMachine
{
    public interface IState
    {
        string name { get; }

        void Init(StateMachine machine, string name, params object[] payload);
        void Enter();
        void Exit();

        void OnEnter();
        void OnExit();
        void OnUpdate();
        void OnMessageReceived(int message, params object[] payload);


        void OnValueChange(int paramId, bool value);
        void OnValueChange(int paramId, int value);
        void OnValueChange(int paramId, float value);
    }
}
