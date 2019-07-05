using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAI : MonoBehaviour {

    [SerializeField]
    float distX = 10f, distY = 0f, speed = 4f;

    [SerializeField]
    bool moveX = true, moveY = false;

    float transformX = 0f, transformY = 0f;
    public float TransformX
    {
        get { return transformX; }
        private set { this.transformX = value; }
    }
    public float TransformY
    {
        get { return transformY; }
        private set { this.transformY = value; }
    }

    Player player;
    // Use this for initialization
    void Start () {
        player = GetComponent<Player>();
    }

    void Move(bool moveX, bool moveY)
    {
        if (moveX) {
           transformX = Mathf.PingPong(Time.time * speed, distX);
           transform.position = new Vector3(transformX, transform.position.y, transform.position.z);
        }
        if (moveY)
        {
            transformY = Mathf.PingPong(Time.time * speed, distY);
            transform.position = new Vector3(transform.position.x, transformY, transform.position.z);
        }
        

    }
	
	// Update is called once per frame
	void Update () {

        Move(moveX, moveY);
    }
}
