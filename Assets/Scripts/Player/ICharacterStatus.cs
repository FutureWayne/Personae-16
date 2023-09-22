using System.Collections.Generic;



public interface ICharacterStatus
{
    Dictionary<ECharacterStatus, int> DictStatus { get; set; }
    
    void SetStatus(ECharacterStatus status, int value);
    int GetStatus(ECharacterStatus status);
    void InitStatus();
}