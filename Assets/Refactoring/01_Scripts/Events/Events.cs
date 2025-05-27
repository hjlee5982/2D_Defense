
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



// JGameManager -> UI_GameStatus
public class GameStatusChangeEvent
{
    public enum GameStatusType
    {
        Life,
        NumOfMonster,
        Gold
    }

    public GameStatusType Type;
    public int Value;

    public GameStatusChangeEvent(GameStatusType type, int value)
    {
        Type  = type;
        Value = value;
    }
}



// MonsterUnit -> JGameManager
public class MonsterStateChangeEvent
{
    public enum MonsterStateType
    {
        Die,
        Finish
    }

    public MonsterStateType Type;
    
    public MonsterStateChangeEvent(MonsterStateType type)
    {
        Type = type;
    }
}



// JGameManager -> UI_SpawnAlly
public class SummonRestrictionEvent
{
    public int Gold;
    public int Level_1;
    public int Level_2;
    public int Level_3;

    public SummonRestrictionEvent(int gold, int level_1, int level_2, int level_3)
    {
        Gold = gold;
        Level_1 = level_1;
        Level_2 = level_2;
        Level_3 = level_3;
    }
}



// AllySpawner -> JGameManager
public class SummonCompleteEvent
{
    public int Gold;

    public SummonCompleteEvent(int gold)
    {
        Gold = gold;
    }
}