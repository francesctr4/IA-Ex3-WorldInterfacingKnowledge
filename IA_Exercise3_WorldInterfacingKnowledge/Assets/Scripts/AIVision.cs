using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIVision : MonoBehaviour
{
    public NavMeshWanderAndSeek wolf;
    public Camera cam;
    public bool playerDetected = false;

    private Plane[] cameraFrustrum;
    private MeshRenderer targetRenderer;

    private void Start()
    {
        targetRenderer = wolf.target.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        var targetBounds = targetRenderer.bounds;

        cameraFrustrum = GeometryUtility.CalculateFrustumPlanes(cam);

        if (GeometryUtility.TestPlanesAABB(cameraFrustrum, targetBounds))
        {
            playerDetected = true;
            SetStateToSeek();
        }
        else
        {
            playerDetected = false;
            SetStateToWander();
        }

    }

    public void SetStateToSeek()
    {
        wolf.state = NavMeshWanderAndSeek.WolfStates.SEEK;
    }

    public void SetStateToWander()
    {
        wolf.state = NavMeshWanderAndSeek.WolfStates.WANDER;
    }

}
