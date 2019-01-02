using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject m_player;

    private Vector3 m_v3Offset;

    // Start is called before the first frame update
    void Start()
    {
        m_v3Offset = transform.position - m_player.transform.position;
    }

    private void LateUpdate()
    {
        transform.position = m_player.transform.position + m_v3Offset;
    }
}
