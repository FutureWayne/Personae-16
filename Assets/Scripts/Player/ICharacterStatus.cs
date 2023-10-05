using System.Collections.Generic;



public interface ICharacterStatus
{
    Dictionary<ECharacterStatusType, int> DictStatus { get; set; }
    
    void SetStatus(ECharacterStatusType statusType, int value);
    int GetStatus(ECharacterStatusType statusType);
    void InitStatus();
}