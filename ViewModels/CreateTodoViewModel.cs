using System.ComponentModel.DataAnnotations;

namespace meuToDo.ViewModels
{
    public class CreateTodoViewModel
    {
        [Required]
        public string Title { get; set; }
        public bool Done { get; set; }
    }
}
