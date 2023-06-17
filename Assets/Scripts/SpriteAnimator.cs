using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private bool loop = true;
    [SerializeField] private float speedModifier = 1f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool randomStartingFrame;

    private int m_Index = 0;
    private float m_Frame = 0;

    void Awake()
    {

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        m_Frame = 1;

        if (randomStartingFrame)
        {
            m_Index = Random.Range(0, sprites.Length - 1);
        }
    }


void Update()
{
    if (Time.timeScale == 0 || !loop && m_Index == sprites.Length) return;

    m_Frame += Time.deltaTime * speedModifier;

    if (m_Frame < 1f/12f)
    {
        return;
    }

    spriteRenderer.sprite = sprites[m_Index];

    m_Frame = 0;
    m_Index++;

    if (m_Index >= sprites.Length)
    {
        if (loop)
        {
            m_Index = 0;
        }
    }
}


    public void SetIndex(int index)
    {
        m_Index = index;
    }

    public void Draw()
    {
        spriteRenderer.sprite = sprites[m_Index];
    }
}