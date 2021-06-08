using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndicator : MonoBehaviour
{
    private PlayerStats player;
    private SpriteRenderer sprite;
    
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] Color[] color = new Color[4];

    [SerializeField] private SpriteRenderer playerNumber;
    [SerializeField] private Sprite[] playerNumbers;

    private Color playerColor;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        player = gameObject.GetComponentInParent<PlayerStats>();
        playerColor = color[player.playerNum - 1];
        playerNumber.sprite = playerNumbers[player.playerNum - 1];
    }

    // Update is called once per frame
    void Update()
    {
        sprite.color = new Color(playerColor.r, playerColor.g, playerColor.b, playerSprite.color.a);
        sprite.enabled = playerSprite.enabled;
        playerNumber.enabled = playerSprite.enabled;
        playerNumber.color = new Color(1, 1, 1, playerSprite.color.a);
        playerNumber.transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
