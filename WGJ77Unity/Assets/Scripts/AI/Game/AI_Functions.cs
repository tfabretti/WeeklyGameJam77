using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AI_Functions
{
	public static Transform VisionCastSphere( StateController p_controller )
	{
		EnemyStats gameStats = p_controller.m_gameEnemyStats;
		if ( gameStats.m_vision.m_sphereCastRadius > 0f )
		{
			if ( gameStats.m_vision.m_range > 0 )
				Debug.DrawRay( p_controller.m_agentEyesTransform.position, p_controller.m_agentEyesTransform.forward.normalized * gameStats.m_vision.m_range, Color.green );

			// If there is a collision match
			RaycastHit hit;
			if ( Physics.SphereCast( p_controller.m_agentEyesTransform.position, gameStats.m_vision.m_sphereCastRadius,
				p_controller.m_agentEyesTransform.forward, out hit, gameStats.m_vision.m_range ) )
			{
				StateController otherStateController = hit.transform.GetComponent<StateController>();
				// If the collided object has a StateController and has a dfferent team : attack ! :)
				if ( otherStateController != null && otherStateController.m_gameEnemyStats.m_teams.m_teamMask != gameStats.m_teams.m_teamMask )
				{
					return hit.transform;
				}
			}
		}

		return null;
	}

	public static Transform VisionDetectClosestTarget( StateController p_controller, int p_excludedLayerMasks )
	{
		EnemyStats gameStats = p_controller.m_gameEnemyStats;

		if ( gameStats.m_vision.m_range == 0 )
			return null;

		int collidingCount = Physics.OverlapSphereNonAlloc( p_controller.m_agentEyesTransform.position, gameStats.m_vision.m_range, p_controller.m_collidersArrayForTests, p_excludedLayerMasks );

		Vector3 v3VisionForward = Quaternion.Euler( 0, gameStats.m_vision.m_direction, 0 ) * p_controller.m_agentEyesTransform.forward;
		float halfVisionAngle = gameStats.m_vision.m_angle * 0.5f;
		float closestTargetDistance = gameStats.m_vision.m_range * 2;
		Transform closestTarget = null;
		for ( int i = 0 ; i < collidingCount ; ++i )
		{
			Vector3 v3EyesToCollider = p_controller.m_collidersArrayForTests[i].transform.position - p_controller.m_agentEyesTransform.position;

			float targetAngle = Vector3.Angle( v3EyesToCollider, v3VisionForward ); // This function returns an angle between 0 and 180
			if ( targetAngle < halfVisionAngle && v3EyesToCollider.magnitude < closestTargetDistance )
			{
				StateController otherStateController = p_controller.m_collidersArrayForTests[i].transform.GetComponent<StateController>();
				// If the collided object has a StateController and has a different team : attack ! :)
				if ( otherStateController != null && ( otherStateController.m_gameEnemyStats.m_teams.m_teamMask & gameStats.m_teams.m_teamMask ) == 0 )
				{
					closestTarget = p_controller.m_collidersArrayForTests[i].transform;
					closestTargetDistance = v3EyesToCollider.magnitude;
				}
			}
		}

		return closestTarget;
	}
}
