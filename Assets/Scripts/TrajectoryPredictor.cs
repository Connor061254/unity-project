using UnityEngine;

public class TrajectoryPredictor : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public int lineSegmentCount = 50;

    public float timeStep = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void UpdateTrajectory(Vector3 startPos, Vector3 initialVelocity)
    {
        lineRenderer.positionCount = lineSegmentCount;
        Vector3[] trajectoryPoints = new Vector3[lineSegmentCount];

        for (int i = 0; i < lineSegmentCount; i++)
        {
            float time = i * timeStep;

            Vector3 positionOffset = initialVelocity * time + 0.5f * Physics.gravity *Mathf.Pow(time, 2);

            trajectoryPoints[i] = startPos + positionOffset;
        }

        lineRenderer.SetPositions(trajectoryPoints);
    }

    public void ClearTrajectory()
    {
        lineRenderer.positionCount = 0;
    }
}
