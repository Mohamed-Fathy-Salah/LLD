namespace InstallmentPlanner.Enums;

[Flags]
public enum ContractStatusEnum : short
{
    Init = 1 << 0,
    Booked = 1 << 1,
    Terminated = 1 << 2,
    Securitized = 1 << 3,
}
