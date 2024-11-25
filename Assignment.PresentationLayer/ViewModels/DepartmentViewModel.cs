namespace PresentationLayer.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Range(0, 500)]
        public int Code { get; set; }
        [Required(ErrorMessage = "Name Is Required")]
        public string Name { get; set; }
        [Display(Name = "Created At")]
        public DateTime CDate { get; set; }
    }
}
