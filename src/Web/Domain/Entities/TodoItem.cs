namespace Web.Domain.Entities;

public class TodoItem
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Note { get; set; }

    private bool _done;
    public bool Done 
    { 
        get => _done; 
        set
        {
            _done = value;
        }
    }
}
