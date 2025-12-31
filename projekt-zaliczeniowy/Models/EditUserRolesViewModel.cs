using System.Collections.Generic;

namespace projekt_zaliczeniowy.Models
{
    public class EditUserRolesViewModel
{
    public string UserId { get; set; }
    public string UserEmail { get; set; }

    // Lista ról, które użytkownik posiada obecnie
    public IList<string> CurrentRoles { get; set; }

    // Lista wszystkich dostępnych ról w systemie (do wyświetlenia w Dropdown/Checkboxach)
    public List<string> AllRoles { get; set; }
}
}
