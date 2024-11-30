using TodoListAPI.Models;

namespace TodoListAPI.Services
{
    public class TodosService
    {
        static List<Todos> Todos {get;}

        static int nextId;

        static TodosService()
        {
            Todos = new List<Todos>
            {
                new Todos{Id = 1, Title = "Buy groceries", Description= " Buy milk, eggs, and bread "},  
                new Todos{Id = 2, Title = "Buy clothes", Description= " Buy Tshirt and jeans "},  
                new Todos{Id = 3, Title = "Mengerjakan Tugas", Description= " Terdapat PR pada mata pelajar matematika hal 70 no 1-10 "}  
            };
        }

        public static List<Todos> GetAll() => Todos;
        public static Todos GetTodos(int id) => Todos.FirstOrDefault(p => p.Id == id);

        public static void CreateTodos(Todos todo)
        {
            nextId = Todos.Count + 1;

            todo.Id = nextId++;

            Todos.Add(todo);
        }

        public static void DeleteTodo(int id)
        {
            Todos todos = GetTodos(id);

            if(todos is null) return;

            Todos.Remove(todos);
        }

        public static void UpdateTodo(Todos updatedTodo)
        {
            var index = Todos.FindIndex(p => p.Id == updatedTodo.Id);

            if(index == -1) return;

            Todos[index] = updatedTodo;

        }




    }
}