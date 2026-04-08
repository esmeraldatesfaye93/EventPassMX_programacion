using System.Configuration;
using System.Data;
using System.Windows;

namespace EventPassMX_programacion
{
  
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var login = new SimpleLoginWindow();
            login.Show();
        }
    }

}
