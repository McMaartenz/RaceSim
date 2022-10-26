using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Grafische
{
    /// <summary>
    /// Interaction logic for SchermA.xaml
    /// </summary>
    public partial class SchermA : Window
    {
        bool keepAlive = true;

        public SchermA()
        {
            InitializeComponent();

            MainWindow.WPFExit += (sender, e) => { keepAlive = false; Close(); };
            Closing += (sender, e) => { e.Cancel = keepAlive; Hide(); };
        }
    }
}
