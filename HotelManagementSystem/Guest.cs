public class Guest(string name)
{
    public string Name { get; } = name;
    public Room? AssignedRoom { get; private set; } = null;

    public bool AssignRoom(Room room)
    {
        AssignedRoom = room;
        return true;
    }

    public bool FreeRoom()
    {
        AssignedRoom = null;
        return true;
    }

    public void CheckIn()
    {
        AssignedRoom?.CheckIn();
    }

    public void CheckOut()
    {
        AssignedRoom?.CheckOut();
    }

    public override string ToString()
    {
        return Name;
    }
}
