using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class SimpleProceduralMap : MonoBehaviour
{
    [Header("Tilemap")]
    public Tilemap tilemap;

    [Header("Rule Tile")]
    public RuleTile grassRuleTile;

    [Header("Settings")]
    public Camera mainCamera;
    public int extraPadding = 5;           // Padding nhỏ, tự động + size camera

    [Header("Noise")]
    public float noiseScale = 0.085f;
    public int seed = 12345;

    private HashSet<Vector3Int> activeTiles = new HashSet<Vector3Int>();

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera == null) return;
        UpdateMapToCameraView();
    }

    void UpdateMapToCameraView()
    {
        // Tính kích thước camera thực tế
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = halfHeight * mainCamera.aspect;

        Vector3 minWorld = mainCamera.transform.position - new Vector3(halfWidth, halfHeight, 0);
        Vector3 maxWorld = mainCamera.transform.position + new Vector3(halfWidth, halfHeight, 0);

        // Chuyển sang tile coordinate + padding
        Vector3Int minTile = tilemap.WorldToCell(minWorld) - new Vector3Int(extraPadding, extraPadding, 0);
        Vector3Int maxTile = tilemap.WorldToCell(maxWorld) + new Vector3Int(extraPadding, extraPadding, 0);

        HashSet<Vector3Int> currentVisible = new HashSet<Vector3Int>();

        // 1. Generate tile trong vùng camera
        for (int x = minTile.x; x <= maxTile.x; x++)
        {
            for (int y = minTile.y; y <= maxTile.y; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                currentVisible.Add(pos);

                if (!activeTiles.Contains(pos))
                {
                    float noise = Mathf.PerlinNoise((x + seed) * noiseScale, (y + seed) * noiseScale);
                    tilemap.SetTile(pos, grassRuleTile);
                    activeTiles.Add(pos);
                }
            }
        }

        // 2. Xóa tile ngoài vùng camera
        List<Vector3Int> toRemove = new List<Vector3Int>();
        foreach (var pos in activeTiles)
        {
            if (!currentVisible.Contains(pos))
            {
                tilemap.SetTile(pos, null);
                toRemove.Add(pos);
            }
        }

        foreach (var pos in toRemove)
            activeTiles.Remove(pos);
    }
}