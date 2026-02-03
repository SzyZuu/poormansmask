using Godot;

namespace poormansmask.scripts.singletons;

public partial class LevelManager : Node
{
    private int _enemyCount;
    
    [Signal]
    public delegate void StageClearedEventHandler();
    
    public void RegisterEnemy(Enemy enemy)
    {
        enemy.Dead += () =>
        {
            _enemyCount--;
            if (_enemyCount == 0)
                EmitSignal(SignalName.StageCleared);
        };
        
        _enemyCount++;
    }
}