using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

[Serializable]
public class CutsceneScriptControlBehaviour : PlayableBehaviour
{
    public bool playerInputEnabled;
    public bool useRootMotion;
    [FormerlySerializedAs("playerInputController")] [FormerlySerializedAs("playerInput")] public PlayerInput playerInputControllerController;

    public override void OnGraphStart (Playable playable)
    {
        
    }
}
