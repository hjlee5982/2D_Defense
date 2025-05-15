
// UI_SpawnAlly -> JGameManager 
public class StartSpawnAllyEvent
{
    public int BtnIdx = -1;

    public StartSpawnAllyEvent(int btnIdx) 
    {
        BtnIdx = btnIdx;
    }
}



// UI_Enhancement -> JGameManager 
public class StartEnhancementEvent
{
    public int BtnIdx = -1;

    public StartEnhancementEvent(int btnIdx)
    {
        BtnIdx = btnIdx;
    }
}




// JUIManager¿¡ ÀÖ´Â StartButton -> JGameManager 
public class StartRoundEvent { }



// JGameManager -> AllySpawner
public class BeginSpawnAllyEvent
{
    public AllyUnitData AllyUnitData;

    public BeginSpawnAllyEvent(AllyUnitData allyUnitData) 
    {
        AllyUnitData = allyUnitData;
    }
}




// JGameManager -> MonsterSpawner
public class BeginSpawnMonsterEvent
{
    public StageData   StageData;
    public MonsterUnitData MonsterUnitData;

    public BeginSpawnMonsterEvent(StageData stageData, MonsterUnitData monsterUnitData)
    {
        StageData       = stageData;
        MonsterUnitData = monsterUnitData;
    }
}



// JGameManager ->UI_UnitStatus, JUIManager
public class UnitSelectEvent 
{
    public AllyUnit SelectedUnit;

    public UnitSelectEvent(AllyUnit selectedUnit)
    {
        SelectedUnit = selectedUnit;
    }
}
public class UnitDeselectEvent { }



// JGameManager -> UI_UnitStatus
public class EnhanceCompleteEvent 
{
    public AllyUnit SelectedUnit;

    public EnhanceCompleteEvent(AllyUnit selectedUnit)
    {
        SelectedUnit = selectedUnit;
    }
}