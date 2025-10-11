namespace InstallmentPlanner.DTOs;

public abstract class BaseActionDto
{
    public int ContractId { get; set; }
    public DateTime EffectiveChangeDate { get; set; }
}
