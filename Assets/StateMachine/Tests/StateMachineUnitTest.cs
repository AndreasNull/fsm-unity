using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace ADikt.StateMachine.Test
{
    public class StateMachineUnitTest
    {
        public class PayloadClass
        {
            public PayloadClass(string some)
            {
                someString = some;
            }

            public string someString { get; set; }
        }

        [Test]
        public void StateMachineUnitTestSimplePasses()
        {
            const string STATE_1 = "Test State 1";
            const string STATE_2 = "Test State 2";
            const int CHANGE_STATE_PARAM_ID = 0;

            // prepare test
            TestState testState = new TestState();
            TestState testState2 = new TestState();
            PayloadClass payloadClass1 = new PayloadClass("Yes");
            PayloadClass payloadClass2 = new PayloadClass("No");

            StateMachine stateMachine = new StateMachine();
            stateMachine.AddState(testState, STATE_1, "Test State 2", payloadClass1);
            stateMachine.AddState(testState2, STATE_2, "Test State 1", payloadClass2);
            stateMachine.RegisterParam(CHANGE_STATE_PARAM_ID, false, "Change State");
            stateMachine.AddTransition(
                new Transition(testState.name, testState2.name, 
                new BoolCondition(CHANGE_STATE_PARAM_ID, true)));

            // init state machine
            //stateMachine.Init();

            //Assert.AreEqual("None", stateMachine.currentState);
            //Assert.AreEqual(StateMachine.Status.Stopped, stateMachine.status);

            // start state machine
            stateMachine.StartStateMachine();

            Assert.AreEqual(testState.name, stateMachine.currentStateName);
            Assert.AreEqual(testState.testStatus, 1);
            Assert.AreEqual(testState.updateCount, 0);
            Assert.AreEqual(StateMachine.Status.Running, stateMachine.status);

            // test update
            stateMachine.Update();
            Assert.AreEqual(testState.updateCount, 1);
            stateMachine.Update();
            Assert.AreEqual(testState.updateCount, 2);

            // test pause
            stateMachine.PauseStateMachine();
            Assert.AreEqual(StateMachine.Status.Paused, stateMachine.status);
            stateMachine.Update();
            Assert.AreEqual(testState.updateCount, 2);

            // test resume
            stateMachine.PlayStateMachine();
            Assert.AreEqual(StateMachine.Status.Running, stateMachine.status);
            stateMachine.Update();
            Assert.AreEqual(testState.updateCount, 3);

            // test transition
            stateMachine.Update();
            Assert.AreEqual(testState2.name, stateMachine.currentStateName);
            Assert.AreEqual(testState2.updateCount, 0);
            stateMachine.Update();
            Assert.AreEqual(testState2.updateCount, 1);
        }
    }

    public class TestState : State
    {
        const int CHANGE_STATE_PARAM_ID = 0;

        public int testStatus { get; set; }
        public int updateCount { get; set; }

        public override void OnInit()
        {
            testStatus = 0;
        }

        public override void OnEnter()
        {
            Assert.NotNull(payload[0]);
            updateCount = 0;
            testStatus++;
        }

        public override void OnExit()
        {
            testStatus--;
        }

        public override void OnUpdate()
        {
            updateCount++;

            if (updateCount > 3)
            {
                SetParam(CHANGE_STATE_PARAM_ID, true);
            }
        }
    }
}
