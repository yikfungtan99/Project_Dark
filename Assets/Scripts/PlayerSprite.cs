using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{

    [SerializeField] private AnimatorOverrideController[] animators = new AnimatorOverrideController[4];

    private PlayerStats player;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerStats>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.runtimeAnimatorController = animators[player.playerNum - 1];
    }
}
