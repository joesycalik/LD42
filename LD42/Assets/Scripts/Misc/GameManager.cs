using UnityEngine;
using System.IO;
using System.Net;
using UnityEngine.UI;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{

    private static GameManager m_instance = null;

    public int highScore;
    public int bestTime;

    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                var go = new GameObject("GameManager");
                m_instance = go.AddComponent<GameManager>();
            }
            return m_instance;
        }
    }

    void Awake()
    {
        if (m_instance != this && m_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        m_instance = this;
        DontDestroyOnLoad(this.gameObject);
    } //End Awake
}