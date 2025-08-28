using TaskMicroservice.Entities;

namespace TaskMicroservice.Repositories;

public class TaskRepository:IRepository<Tasks>
{

    private readonly List<Tasks> _tasks;

    public TaskRepository()
    {
        _tasks = new List<Tasks>();
    }
    
    public IEnumerable<Tasks> GetAll()
    {
        return _tasks;
    }

    public void Add(Tasks entity)
    {
        if (_tasks.Any(t => t.Id == entity.Id))
        {
            Console.WriteLine($"Task with Id: {entity.Id} already exists");
            return;
        }
        _tasks.Add(entity);
        Console.WriteLine($"Task with Id: {entity.Id} added successfully");
    }

    public int Update(Tasks entity)
    {
        throw new NotImplementedException();
    }

    public int Delete(Tasks entity)
    {
        throw new NotImplementedException();
    }
}