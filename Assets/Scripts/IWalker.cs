using UnityEngine;

public interface IWalker
{
    float MoveSpeed { get; }       
    bool CanWalk { get; }          
    Vector2 Direction { get; set; } 

    void Walk(Vector2 dir);  
}
