using SnailPass_Desktop.View.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.ViewModel.Services
{
    internal class DialogService : IDialogService
    {
        IEnumerable<Type> dialogTypes;

        public DialogService()
        {
            dialogTypes = from t in Assembly.GetExecutingAssembly().GetTypes()
                          where t.GetTypeInfo().GetCustomAttribute<DialogContentAttribute>() != null
                          select t;
        }

        public void ShowDialog(string name)
        {
            DialogWindow dialog = new DialogWindow();

            Type dialogContentType = dialogTypes.First(t => t.Name == $"{name}");
            dialog.Content = Activator.CreateInstance(dialogContentType);
        }
    }
}
