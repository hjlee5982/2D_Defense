
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
    public int BtnIdx = -1;

    public BeginSpawnAllyEvent(int btnIdx) 
    {
        BtnIdx = btnIdx;
    }
}




// JGameManager -> MonsterSpawner
public class BeginSpawnMonsterEvent { }



// JGameManager ->UI_UnitStatus, JUIManager
public class UnitSelectEvent 
{
    public JUnit SelectedUnit;

    public UnitSelectEvent(JUnit selectedUnit)
    {
        SelectedUnit = selectedUnit;
    }
}
public class UnitDeselectEvent { }



// JGameManager -> UI_UnitStatus
public class EnhanceCompleteEvent 
{
    public JUnit SelectedUnit;

    public EnhanceCompleteEvent(JUnit selectedUnit)
    {
        SelectedUnit = selectedUnit;
    }
}