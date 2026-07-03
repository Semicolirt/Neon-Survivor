using System.Collections.Generic;
using UnityEngine;

public class SeparateSystem : MonoBehaviour
{
    [Header("Separation Data")]
    public SeparateSettingsSO settings;     // Kéo SO vào đây

    private Rigidbody2D rb;
    private Transform myTransform;
    private Vector2 currentSeparationForce;
    
    // Cache list to avoid GC allocation
    private List<Transform> nearbyUnitsCache = new List<Transform>(32);

    // Fallback values nếu quên gán SO
    private float Radius => settings != null ? settings.separationRadius : 1.8f;
    private float Weight => settings != null ? settings.separationWeight : 2.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myTransform = transform;
    }

    private void OnEnable()
    {
        SpatialGrid.Instance?.Register(myTransform);
    }

    private void OnDisable()
    {
        SpatialGrid.Instance?.Unregister(myTransform);
    }

    private void FixedUpdate()
    {
        if (SpatialGrid.Instance != null)
        {
            SpatialGrid.Instance.UpdateUnitPosition(myTransform);
        }

        CalculateSeparationForce();
        ApplyForce();
    }

    private void CalculateSeparationForce()
    {
        currentSeparationForce = Vector2.zero;

        if (SpatialGrid.Instance == null) return;

        float radius = Radius;
        SpatialGrid.Instance.GetNearbyUnits(myTransform.position, radius, nearbyUnitsCache);

        for (int i = 0; i < nearbyUnitsCache.Count; i++)
        {
            Transform other = nearbyUnitsCache[i];
            if (other == null || other == myTransform) continue;

            Vector2 diff = (Vector2)myTransform.position - (Vector2)other.position;
            float sqrDist = diff.sqrMagnitude;

            if (sqrDist < 0.0001f) 
            {
                // To avoid getting stuck when units overlap exactly
                diff = new Vector2(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f));
                if (diff == Vector2.zero) diff = Vector2.right * 0.01f;
                sqrDist = diff.sqrMagnitude;
            }

            // Using diff * (radius / sqrDist) avoids Mathf.Sqrt and .normalized entirely
            currentSeparationForce += diff * (radius / sqrDist);
        }

        currentSeparationForce *= Weight;
    }

    private void ApplyForce()
    {
        if (currentSeparationForce.sqrMagnitude > 0.001f)
        {
            rb.AddForce(currentSeparationForce, ForceMode2D.Force);
        }
    }

    public Vector2 GetCurrentSeparationForce() => currentSeparationForce;
}