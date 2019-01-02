using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadSceneOnClick : MonoBehaviour
{
    public void LoadByIndex( int p_sceneIndex )
    {
        SceneManager.LoadScene( p_sceneIndex);
    }

    public void LoadByName( string p_strSceneName )
    {
        SceneManager.LoadScene( p_strSceneName );
    }
}
