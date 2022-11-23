using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlinkPlatform : MonoBehaviour
{
    Tilemap tilemap;
    TilemapCollider2D tilemapCollider;


    // Start is called before the first frame update
    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        tilemapCollider = GetComponent<TilemapCollider2D>();
        StartCoroutine(Blink());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Blink()
    {
        while (true)
        {
            tilemap.color = new Color(1, 1, 1, 1f);
            tilemapCollider.enabled = true;
            yield return new WaitForSeconds(2f);

            tilemap.color = new Color(1, 1, 1, 0f);
            tilemapCollider.enabled = false;
            yield return new WaitForSeconds(2f);
        }
    }
}
