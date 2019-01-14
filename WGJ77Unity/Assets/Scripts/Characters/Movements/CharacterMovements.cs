using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterMovements : MonoBehaviour
{
	public string m_animationIdle = "";
	public string m_animationMove = "";
	public string m_animationAttack = "";

	public string m_buttonAttackName = "Attack";

	public DragonBones.UnityArmatureComponent m_animator = null; // Reference to the animator

	protected bool m_canMove = true;

	// Update is called once per frame
	void FixedUpdate()
	{
		if ( m_canMove == true )
		{
			DoMovements();
		}

		DoNoMoveAction();
	}

	protected abstract void DoMovements();

	protected virtual void DoNoMoveAction()
	{
	}

	public void EnableMovements()
	{
		m_canMove = true;
	}

	public void DisableMovements()
	{
		m_canMove = false;
	}
}
