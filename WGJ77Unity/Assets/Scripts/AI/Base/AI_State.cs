using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/State")]
public class AI_State : ScriptableObject
{
#if UNITY_EDITOR
	[Tooltip( "Actions played when entering the state.")]
#endif
	public AI_Action[] m_onEnterActions;

#if UNITY_EDITOR
	[Tooltip( "Actions played when updating the state." )]
#endif
	public AI_Action[] m_actions;

#if UNITY_EDITOR
	[Tooltip( "Actions played when exiting the state." )]
#endif
	public AI_Action[] m_onExitActions;

#if UNITY_EDITOR
	[Tooltip( "Transitions contain decisions that can launch given states if true or false." )]
#endif
	public AI_Transition[] m_transitions;

#if UNITY_EDITOR
	[Tooltip( "This color is used to draw debug lines and test the state." )]
#endif
	public Color m_sceneGizmoColor = Color.grey;


    public void UpdateState( AI_StateController p_controller )
    {
        DoActions( p_controller );
        CheckTransitions( p_controller, false );
    }

	public void EnterState( AI_StateController p_controller )
	{
		DoOnEnterActions( p_controller );
		CheckTransitions( p_controller, true );
	}

	public void ExitState( AI_StateController p_controller )
	{
		DoOnExitActions( p_controller );
	}

	private void DoOnEnterActions( AI_StateController p_controller )
	{
		for ( int i = 0 ; i < m_onEnterActions.Length ; ++i )
		{
			m_onEnterActions[i].Act( p_controller );
		}
	}

	private void DoActions( AI_StateController p_controller )
    {
        for ( int i = 0 ; i < m_actions.Length ; ++i )
        {
            m_actions[i].Act( p_controller );
        }
    }

	private void DoOnExitActions( AI_StateController p_controller )
	{
		for ( int i = 0 ; i < m_onExitActions.Length ; ++i )
		{
			m_onExitActions[i].Act( p_controller );
		}
	}

	private void CheckTransitions( AI_StateController p_controller, bool p_ignoreDelays )
    {
        for ( int i = 0 ; i < m_transitions.Length ; i++ )
        {
			if ( !p_ignoreDelays )
			{
				// Delay is valid test (min and max are valid)
				float transitionDelay = m_transitions[i].m_minimumDelayRetest;
				if ( m_transitions[i].m_maximumDelayRetest > m_transitions[i].m_minimumDelayRetest )
				{
					float minimumDelay = m_transitions[i].m_minimumDelayRetest;
					float maxumimDelay = m_transitions[i].m_maximumDelayRetest;
					transitionDelay = minimumDelay + ( maxumimDelay - minimumDelay ) * p_controller.m_transitionsDelayCoefficient;
				}
				// If the dealy is valid : test
				if ( transitionDelay > 0 )
				{
					float currentCycle = Mathf.Repeat( p_controller.m_stateTimeElapsed, transitionDelay );
					float previousCycle = Mathf.Repeat( p_controller.m_stateTimeElapsed - Time.deltaTime, transitionDelay );
					// If the delay is not up yet : skip
					if ( previousCycle < currentCycle )
						continue;
				}
			}

			// Decision test
			bool decisionSucceeded = m_transitions[i].m_decision.Decide( p_controller );
			if ( decisionSucceeded && m_transitions[i].m_useTrueState )
				p_controller.TransitionToState( m_transitions[i].m_trueState );
			else if( !decisionSucceeded && m_transitions[i].m_useFalseState )
				p_controller.TransitionToState( m_transitions[i].m_falseState );
        }
    }
}
