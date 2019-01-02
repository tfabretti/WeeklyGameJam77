using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AI_Transition
{
    public AI_Decision m_decision;
    public AI_State m_trueState;
    public AI_State m_falseState;
}