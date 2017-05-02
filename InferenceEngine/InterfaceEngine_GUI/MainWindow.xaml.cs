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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InterfaceEngine_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String attributesString = "";

        public MainWindow()
        {
            InitializeComponent();


            KnowledgeBase KB = new KnowledgeBase();
            KB.Interpret("p2=> p3; p3 => p1; c => e; b&e => f; f&g => h; p1=>d; p1&p3 => c; a; b; p2;");

            /*foreach (Statement o in KB.World)
            {
                Console.WriteLine(o.Identifier);
            }

            Console.WriteLine("Is Consictent: " + KB.CheckConsistency());      */

            /*TruthTable tt = new TruthTable(KB.World);
            tt.WriteTable();
            Console.WriteLine(tt.Query("c=>e"));

            Console.WriteLine("\n\n"); */

            //tt.clean();
            //tt.writetable();
            //console.writeline(tt.query("c=>e"));

            //Console.ReadKey();
        }

        private void submitAttribute()
        {
            if ((addAttribute.Text != "") && !(addAttribute.Text.StartsWith(" ")))
            {
                attributesString += addAttribute.Text + ";\n";
                addAttribute.Text = "";
                attributes.Text = attributesString;
                attributes.SelectionStart = attributesString.Length;
                //attributes.ScrollToCaret();
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                submitAttribute();
            }
        }

        private void InitialiseUI()
        {
            
        }

        private void Button_Click_Gen(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_Clear(object sender, RoutedEventArgs e)
        {
            attributesString = "";
            attributes.Text = attributesString;
        }

     
        private void Button_Click_Attribute(object sender, RoutedEventArgs e)
        {
            submitAttribute();
        }
    }
}
