using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingRobot : MonoBehaviour
{
    private Transform player;
    public float targetDist;
    public float speed;
    public float rotateSpeed;

    public int lives = 50;

    void Start()
    {
        player = GameManager.player.gameObject.transform;
    }
    void Update()
    {
        Vector3 distVector = player.position - transform.position;
        float sqrdist = Vector3.SqrMagnitude(distVector);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, player.position - transform.position, out hit))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(player.position - transform.position), rotateSpeed * Time.deltaTime);
            Vector3 place2be = -distVector.normalized * targetDist + player.position;
            Debug.DrawLine(transform.position, place2be, Color.green);
            Debug.Log(Vector3.MoveTowards(transform.position, place2be, speed * Time.deltaTime));
            transform.position = Vector3.MoveTowards(transform.position, place2be, speed * Time.deltaTime);
            
        }
    }

}
