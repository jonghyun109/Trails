using UnityEngine;

public interface IWalker
{
    float MoveSpeed { get; }       
    bool CanWalk { get; }          
    Vector3 Direction { get; set; } 

    void Walk(Vector3 dir);  
}
