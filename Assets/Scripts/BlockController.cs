using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BlockController : MonoBehaviour
{
    public int x, y;
    public enum BlockType
    {
        A, B, C, D, E
    }
    public BlockType blockType;
    public Text typeText;
    public LevelController theLevel;
    void Start()
    {
        theLevel = FindObjectOfType<LevelController>();
    }

    public void SetType(BlockType type)
    {
        switch (type)
        {
            case BlockType.A:
                typeText.text = "A";
                GetComponent<SpriteRenderer>().color = new Color(4f / 255f, 14f / 255f, 249f / 255f);
                break;
            case BlockType.B:
                typeText.text = "B";
                GetComponent<SpriteRenderer>().color = new Color(214f / 255f, 18f / 255f, 20f / 255f);
                break;
            case BlockType.C:
                typeText.text = "C";
                GetComponent<SpriteRenderer>().color = new Color(250f / 255f, 14f / 255f, 250f / 255f);
                break;
            case BlockType.D:
                typeText.text = "D";
                GetComponent<SpriteRenderer>().color = new Color(236f / 255f, 249f / 255f, 58f / 255f);
                break;
            case BlockType.E:
                typeText.text = "E";
                GetComponent<SpriteRenderer>().color = new Color(14f / 255f, 233f / 255f, 239f / 255f);
                break;
        }
        blockType = type;
    }

    void OnMouseDown()
    {
        theLevel.DestroyBlock(this);
        //  Debug.Log(typeText.text);
    }
}
