using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/State")]
public class AI_State : ScriptableObject
{
    public AI_Action[] m_actions;
    public AI_Transition[] m_transitions;
    public Color m_sceneGizmoColor = Color.grey;

    public void UpdateState( AI_StateController p_controller )
    {
        DoActions( p_controller );
        CheckTransitions( p_controller );
    }

    private void DoActions( AI_StateController p_controller )
    {
        for ( int i = 0 ; i < m_actions.Length ; ++i )
        {
            m_actions[i].Act( p_controller );
        }
    }

    private void CheckTransitions( AI_StateController p_controller )
    {
        for ( int i = 0 ; i < m_transitions.Length ; i++ )
        {
            bool decisionSucceeded = m_transitions[i].m_decision.Decide( p_controller );

            if ( decisionSucceeded )
            {
                p_controller.TransitionToState( m_transitions[i].m_trueState );
            }
            else
            {
                p_controller.TransitionToState( m_transitions[i].m_falseState );
            }
        }
    }
}
