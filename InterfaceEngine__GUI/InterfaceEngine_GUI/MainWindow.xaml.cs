using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace InterfaceEngine_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static String attributesString = "";
        private static KnowledgeBase KB;
        private static TruthTable tt;
      

        public MainWindow()
        {
            InitializeComponent();
            InitialiseUI();   
        }

        public void InitialiseUI()
        {
            this.Height = 420;
        }

        //prints the GUI table (sorry for the mess)
        private void printTable()
        {
            List<Statement> statements = tt.Statements;
            List<List<bool>> values = tt.Assertions;

            var table = new DataTable();
            var data = new DataView();

            //data.Columns.Add

            table.Columns.Add(new DataColumn("TruthTable"));

            //range the size of the number of statements
            int rowRange;

            if ((statements.Count - 1) < 0)
                rowRange = 0;
            else
                rowRange = (statements.Count - 1);

            table.Columns.AddRange(Enumerable.Range(1, rowRange).Select(i => new DataColumn(i.ToString())).ToArray());
            DataRow row = table.NewRow();

         



            int x = 0;
            foreach (Statement s in statements)
            {
                row[x] = s.Identifier;
                x++;
            }


            table.Rows.Add(row);
            
          
        
            row = table.NewRow();
            if (tt.Assertions.Count > 0 )
            {
                for (int l = 0; l < values[0].Count; l++)
                {
                    row = table.NewRow();
                    for (int k = 0; k < values.Count; k++)
                    {
                        bool temp = values[k][l];

                        if (temp)
                            row[k] = "1";
                        else
                            row[k] = "0";
                    }
                    table.Rows.Add(row);
                }
            }

            DataContext = table;
        }

        //generates knowledge base and truth table
        private void generateData()
        {
                KB = new KnowledgeBase();
                //KB.Interpret("p2=> p3; p3 => p1; c => e; b&e => f; f&g => h; p1=>d; p1&p3 => c; a; b; p2;");
                
                KB.Interpret(attributesString, AssertionEnum.Assertion);

                tt = new TruthTable(KB);
                printTable();
        }

        //button event handlers 
        private void submitAttribute()
        {
            
            foreach (String statement in new Interpreter(addAttribute.Text).ProcessInput())
            {
                if (statement.Contains("#ERROR"))
                {
                    MessageBox.Show(statement);
                    break;
                }

                else if (statement != "")
                {               
                    attributesString += statement;
                    attributes.Text += statement + "\n";
                    
                }
            }
            addAttribute.Text = "";
            attributes.SelectionStart = attributesString.Length;


          
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TT.IsSelected)
                this.Height = 600;
            else 
                this.Height = 420;

        }

        //event checking for enter key
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                submitAttribute();
            }
        }
     
        //event for generate button
        private void Button_Click_Gen(object sender, RoutedEventArgs e)
        {
            generateData(); 
        }

        //event that clears the data from the entry fields
        private void Button_Click_Clear(object sender, RoutedEventArgs e)
        {
            attributesString = "";
            attributes.Text = attributesString;
            generateData();
        }
     
        private void Button_Click_Attribute(object sender, RoutedEventArgs e)
        {
            submitAttribute();
        }

        //required as part of using a datagrif object
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { }
    }
}
