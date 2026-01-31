namespace poormansmask.scripts.interfaces;

public interface IPickUp
{
    public void ItemInRange(IAmItem item);
    public void ItemOutOfRange();           // for now alwas aat most 1 item in range
}