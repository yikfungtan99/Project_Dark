using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FacingDirection
{
    LEFT,
    RIGHT
}

public class PlatformerPlayerSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private FacingDirection currentFacingDirection;

    private Vector3 lastCoordinate;

    // Update is called once per frame
    void Update()
    {
        //CHECK IF LAST POSITION IS TOWARDS THE LEFT OF CURRENT POSITION
        CheckHeadingDirection();
        FlipSprite();
    }

    private void CheckHeadingDirection()
    {
        float input = Input.GetAxisRaw("Horizontal");
        if (input != 0)
        {
            currentFacingDirection = input > 0 ? FacingDirection.RIGHT : FacingDirection.LEFT;
        }
    }

    private void FlipSprite()
    {
        sprite.flipX = currentFacingDirection == FacingDirection.RIGHT ? true : false;
    }
}
