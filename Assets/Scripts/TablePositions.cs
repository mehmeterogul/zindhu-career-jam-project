using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TablePositions : MonoBehaviour
{
    [SerializeField] List<Transform> tablePositions;

    public List<Transform> GetTablePositions()
    {
        return tablePositions;
    }
}
