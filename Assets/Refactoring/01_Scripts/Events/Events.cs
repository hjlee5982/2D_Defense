
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




// JUIManager에 있는 StartButton <-> JGameManager
public class StartRoundEvent { }
public class EndRoundEvent { }
public class UnitRecallPhase1Event { }
public class UnitRecallPhase2Event { }


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

    public SummonCompleteEvent(int gold = 0)
    {
        Gold = gold;
    }
}




// JGameManager -> UIManager
public class GameStartEvent { }


// JGameManager -> UI_ResultUI
public class GameEndEvent
{
    public int Round;
    public int Life;
    public int Gold;

    public GameEndEvent(int round, int life, int gold)
    {
        Round = round;
        Life  = life;
        Gold  = gold;
    }   
}



// JSettingManager -> 텍스트들
public class LanguageChangeEvent { }




// UI_SettingPanel -> JSettingManager
public class SettingValueChangeEvent
{
    public enum SettingOption
    {
        BGM_Slider, BGM_Toggle, SFX_Slider, SFX_Toggle, Language_Dropdown
    }
    public struct SettingValue
    {
        public float         BGM_Slider_Value;
        public float         SFX_Slider_Value;
        public bool          BGM_Toggle_Value;
        public bool          SFX_Toggle_Value;
        public int           LanguageIndex;
        public SettingOption Option;
    }

    public SettingValue Values;

    public SettingValueChangeEvent(SettingValue values)
    {
        Values = values;
    }
}



// JTitleManager -> UI_SettingPanel
public class OpenSettingPanelEvent { }



// UI_GameStatus -> JGameManager;
public class GameSpeedChangeEvent 
{
    public float Speed;

    public GameSpeedChangeEvent(float speed)
    {
        Speed = speed;
    }
}