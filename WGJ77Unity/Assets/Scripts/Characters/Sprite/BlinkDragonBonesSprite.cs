using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu( menuName = "DragonBones/SpriteBlinker" )]
public class BlinkDragonBonesSprite : ScriptableObject
{
	public float m_blinkingInterval = 0.3f;
	public int m_blinksCount = 4;
	private Color m_blinkingColor = Color.red;


	public void DamageableOnHitEvent( MeshRenderer p_sprite )
	{
		Blink( p_sprite, m_blinksCount );
	}

	public void Blink( MeshRenderer p_sprite, int p_strobeCount )
	{
		DragonBones.UnityArmatureComponent DBComponent = p_sprite.transform.GetComponent<DragonBones.UnityArmatureComponent>();
		if ( DBComponent != null )
			DBComponent.StartCoroutine( BlinkCoroutine( DBComponent, 0, ( ( p_strobeCount * 2 ) - 1 ), p_sprite, p_sprite.enabled ) );
	}

	private IEnumerator BlinkCoroutine( DragonBones.UnityArmatureComponent DBComponent, int p_i, int p_stopAt, MeshRenderer p_mySprite, bool p_startEnable )
	{
		if ( p_i <= p_stopAt )
		{
			p_mySprite.enabled = !p_mySprite.enabled;

			yield return new WaitForSeconds( m_blinkingInterval );
			DBComponent.StartCoroutine( BlinkCoroutine( DBComponent, ( p_i + 1 ), p_stopAt, p_mySprite, p_startEnable ) );
		}
		else
			p_mySprite.enabled = p_startEnable;
	}

	// --- Color change : broken

	public void StrobeColor( DragonBones.UnityArmatureComponent p_sprite, int p_strobeCount, Color p_colorToStrobe )
	{
		DragonBones.ColorTransform oldColor = p_sprite.color;
		DragonBones.ColorTransform newColor = new DragonBones.ColorTransform();
		newColor.redOffset = (int)p_colorToStrobe.r * 255;
		newColor.greenOffset = (int)p_colorToStrobe.g * 255;
		newColor.blueOffset = (int)p_colorToStrobe.b * 255;
		//newColor.alphaOffset = (int)p_colorToStrobe.a * 255;
		p_sprite.StartCoroutine( StrobeColorHelper( 0, ( ( p_strobeCount * 2 ) - 1 ), p_sprite, oldColor, newColor ) );

	}

	public void StrobeAlpha( DragonBones.UnityArmatureComponent p_sprite, int p_strobeCount, float a )
	{
		DragonBones.ColorTransform oldColor = p_sprite.color;
		Color toStrobe = new Color( 0, 0, 0, a );
		StrobeColor( p_sprite, p_strobeCount, toStrobe );
	}

	private IEnumerator StrobeColorHelper( int p_i, int p_stopAt, DragonBones.UnityArmatureComponent p_mySprite, DragonBones.ColorTransform p_color, DragonBones.ColorTransform p_colorToStrobe )
	{
		if ( p_i <= p_stopAt )
		{
			if ( p_i % 2 == 0 )
			{
				p_mySprite.color = p_colorToStrobe;
			}
			else
				p_mySprite.color = p_color;

			yield return new WaitForSeconds( m_blinkingInterval );
			p_mySprite.StartCoroutine( StrobeColorHelper( ( p_i + 1 ), p_stopAt, p_mySprite, p_color, p_colorToStrobe ) );
		}
	}
}
