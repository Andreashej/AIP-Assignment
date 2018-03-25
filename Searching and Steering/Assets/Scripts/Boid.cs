using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Transform leader;
    public float seekForce;
    public float separationForce;
    public float maxSpeed;
    public float approachRadius;

    [Range(0, 5)]
    public float separationRadius;

    [Range(0, 10)]
    public float seekWeight;
    [Range(0, 10)]
    public float separationWeight;
    [Range(0, 10)]
    public float cohesionWeight;

    public Vector3 acceleration;
    public Vector3 velocity;

    public List<Transform> boidList;
    public List<Transform> neighbouringBoids;

    void Start()
    {
        boidList = new List<Transform>();
        neighbouringBoids = new List<Transform>();
        leader = GameObject.FindGameObjectWithTag("Leader").transform;

        GameObject[] entities = GameObject.FindGameObjectsWithTag("Follower");
        for (int i = 0; i < entities.Length; i++)
        {
            if (entities[i] != gameObject)
            {
                boidList.Add(entities[i].transform);
            }
        }
        boidList.Add(leader);

        string message = gameObject.name + ": I have " + boidList.Count.ToString() + " friends, and we're following " + leader.name + "!";
        //Debug.Log(message);
    }

    // Update is called once per frame
    void Update()
    {

        acceleration = CalculateSteering();

        velocity += acceleration * Time.deltaTime;
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }
        transform.position += velocity * Time.deltaTime;

        Debug.DrawLine(transform.position, transform.position + acceleration, Color.red);
        Debug.DrawLine(transform.position, transform.position + velocity, Color.green);
    }

    Vector3 CalculateSteering()
    {
        neighbouringBoids = CalculateNeighbours();
        Vector3 steer = Vector3.zero;

        steer += SeekWithApproach(leader) * seekWeight;
        steer += Separate() * separationWeight;
        steer += Cohesion() * cohesionWeight;

        steer /= (seekWeight + separationWeight + cohesionWeight);
        steer.y = 0;
        return steer;
    }

    List<Transform> CalculateNeighbours()
    {
        List<Transform> neighbours = new List<Transform>();
        foreach (Transform boid in boidList)
        {
            if (Vector3.Distance(transform.position, boid.position) <= separationRadius)
            {
                neighbours.Add(boid);
            }
        }
        return neighbours;
    }
    Vector3 SeekWithApproach(Transform target)
    {
        Vector3 steer;
        Vector3 desired = (target.position - transform.position);
        float distanceFromTarget = desired.magnitude;
        desired.Normalize();

        if (distanceFromTarget < approachRadius)
        {
            desired *= distanceFromTarget / approachRadius * maxSpeed;
        }
        else
        {
            desired *= maxSpeed;
        }

        steer = desired - velocity;
        if (steer.magnitude > seekForce)
        {
            steer = steer.normalized * seekForce;
        }

        Debug.DrawLine(transform.position, transform.position + steer, Color.blue);

        return steer;
    }

    Vector3 Separate()
    {
        Vector3 desired = Vector3.zero;
        Vector3 steer = Vector3.zero;

        if (neighbouringBoids.Count > 0)
        {
            for (int i = 0; i < neighbouringBoids.Count; i++)
            {
                desired += (-1f + Vector3.Distance(neighbouringBoids[i].position, transform.position) / separationRadius) * (neighbouringBoids[i].position - transform.position).normalized;

                //Debug.Log(desired);
            }
            desired /= neighbouringBoids.Count;
            desired *= separationForce;

            steer = desired - velocity;
            if (steer.magnitude > separationForce)
            {
                steer = steer.normalized * separationForce;
            }
        }

        Debug.DrawLine(transform.position, transform.position + steer, Color.yellow);
        return steer;
    }

    Vector3 Cohesion()
    {
        Vector3 cohesionCenter = Vector3.zero;
        Vector3 desired = Vector3.zero;
        Vector3 steer = Vector3.zero;
        if (neighbouringBoids.Count > 0)
        {
            for (int i = 0; i < neighbouringBoids.Count; i++)
            {
                cohesionCenter += neighbouringBoids[i].position;
            }
            cohesionCenter += transform.position;
            cohesionCenter /= neighbouringBoids.Count + 1;

            desired = cohesionCenter - transform.position;
            steer = desired - velocity;

        }
        Debug.DrawLine(transform.position, transform.position + steer, Color.magenta);
        Debug.DrawLine(Vector3.zero, cohesionCenter, Color.white);
        return steer;
    }
}