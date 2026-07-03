using System.Collections.Generic;
using UnityEngine;
public class SpatialGrid : MonoBehaviour
{
    public static SpatialGrid Instance;

    public float cellSize = 2.5f;

    private Dictionary<Vector2Int, List<Transform>> grid = new Dictionary<Vector2Int, List<Transform>>();
    private Dictionary<Transform, Vector2Int> unitCells = new Dictionary<Transform, Vector2Int>();

    private void Awake()
    {
        Instance = this;
    }

    public void Register(Transform unit)
    {
        if (unit == null) return;
        Vector2Int cell = GetCell(unit.position);
        if (!grid.ContainsKey(cell))
            grid[cell] = new List<Transform>(32); // Giảm allocation

        grid[cell].Add(unit);
        unitCells[unit] = cell;
    }

    public void Unregister(Transform unit)
    {
        if (unit == null) return;
        if (unitCells.TryGetValue(unit, out Vector2Int cell))
        {
            if (grid.ContainsKey(cell))
                grid[cell].Remove(unit);
            unitCells.Remove(unit);
        }
    }

    public void UpdateUnitPosition(Transform unit)
    {
        if (unit == null) return;
        
        if (!unitCells.TryGetValue(unit, out Vector2Int oldCell))
        {
            Register(unit);
            return;
        }

        Vector2Int newCell = GetCell(unit.position);
        if (oldCell != newCell)
        {
            if (grid.ContainsKey(oldCell))
            {
                grid[oldCell].Remove(unit);
            }

            if (!grid.ContainsKey(newCell))
            {
                grid[newCell] = new List<Transform>(32);
            }

            grid[newCell].Add(unit);
            unitCells[unit] = newCell;
        }
    }

    public Vector2Int GetCell(Vector2 pos)
    {
        return new Vector2Int(
            Mathf.FloorToInt(pos.x / cellSize),
            Mathf.FloorToInt(pos.y / cellSize)
        );
    }

    public void GetNearbyUnits(Vector2 position, float radius, List<Transform> result)
    {
        result.Clear();
        Vector2Int center = GetCell(position);
        int range = Mathf.CeilToInt(radius / cellSize) + 1;

        float sqrRadius = radius * radius;

        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                Vector2Int cell = center + new Vector2Int(x, y);
                if (grid.TryGetValue(cell, out var list))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        Transform t = list[i];
                        if (t != null && (position - (Vector2)t.position).sqrMagnitude <= sqrRadius)
                        {
                            result.Add(t);
                        }
                    }
                }
            }
        }
    }
}