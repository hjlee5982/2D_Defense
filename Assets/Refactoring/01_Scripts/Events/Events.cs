
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




// JUIManager¿¡ ÀÖ´Â StartButton <-> JGameManager
public class StartRoundEvent { }
public class EndRoundEvent { }



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
        Gold,
        Round
    }

    public GameStatusType Type;
    public int Value;
    public int MaxRound;

    public GameStatusChangeEvent(GameStatusType type, int value, int maxRound = 0)
    {
        Type  = type;
        Value = value;
        MaxRound = maxRound;
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



// JGameManager -> UI_Enhancement, UI_SpawnAlly
public class GoldRestrictionEvent
{
    public int CurrentGold;

    public GoldRestrictionEvent(int currentGold)
    {
        CurrentGold = currentGold;
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



// JTitleManager -> JSettingManager
public class SettingMenuActivationEvent { }