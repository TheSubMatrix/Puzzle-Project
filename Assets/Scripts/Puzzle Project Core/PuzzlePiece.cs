using System;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour, IIdentifiable
{
    [SerializeField] SerializableGuid m_guid = SerializableGuid.NewGuid();
    public Guid ID => m_guid;
}
