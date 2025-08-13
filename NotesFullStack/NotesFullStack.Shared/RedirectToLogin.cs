using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace NotesFullStack.Shared
{
    public class RedirectToLogin : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager {  get; set; }
        protected override void OnInitialized()
        {
            NavigationManager.NavigateTo("login", replace:true);
        }
    }
}
