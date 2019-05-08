namespace StateData
{
    public class StateMachine<T>
    {
        public State<T> currentState { get; private set; }
        public T Owner;

        public StateMachine(T _o)
        {
            Owner = _o;
            currentState = null;
        }

        public void ChangeState(State<T> _newstate)
        {
            if(currentState != null) //if current state exists
                currentState.ExitState(Owner); //play exit state

            currentState = _newstate; //change to new state
            currentState.EnterState(Owner); //play new state entry state
        }

        public void Update()
        {
            if (currentState != null) //if current state exists
                currentState.UpdateState(Owner); //play Update State
        }
    }

    public abstract class State<T>
    {
        public abstract void EnterState(T _owner); //runs once on state entry

        public abstract void ExitState(T _owner); //run once on state change

        public abstract void UpdateState(T _owner); //runs every frame
    }

}
