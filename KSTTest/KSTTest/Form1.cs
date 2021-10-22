using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Collections;

namespace KSTTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        class nameSorting
        {
            public string lastName { get; set; }
            public string givenName { get; set; }
           public string restOfName { get; set; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            string filename="";
            var loadDialog = new OpenFileDialog { Filter = "Text File|*.txt", InitialDirectory = @"C:\Your\Start\Directory\" };
            if (loadDialog.ShowDialog() == DialogResult.OK)
            {
                filename = loadDialog.FileName;
                lblFilePath.Text = filename.ToString();
            }
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            string[] lines = System.IO.File.ReadAllLines
            (@""+lblFilePath.Text);

            System.Diagnostics.Trace.WriteLine("Contents of unsorted-names-list.txt =:");
            nameSorting[] allNames = new nameSorting[10000];
            ArrayList arrList = new ArrayList();

            int count = 0;
            string theRestOfName;

            //get all the names that are in text file and start the loop.
            foreach (string line in lines)
            {
                string[] nameSplit = line.Split(' ');
                if (nameSplit.Count() > 2)
                {
                    if (nameSplit.Count() == 4)
                    {
                        theRestOfName = nameSplit[0] + " " + nameSplit[1]; //handle if the name has 3 given name.
                    }
                    else
                    {
                        theRestOfName = nameSplit[0]; //handle if the name has 2 given name
                    }
                    allNames[count] = new nameSorting
                    {
                        lastName = nameSplit[nameSplit.Count() - 1],
                        givenName = nameSplit[nameSplit.Count() - 2],
                        restOfName = theRestOfName
                    };

                }
                else
                {
                    //handle if the name only consist of LastName and GivenName.
                    allNames[count] = new nameSorting
                    {
                        lastName = nameSplit[nameSplit.Count() - 1],
                        givenName = nameSplit[nameSplit.Count() - 2],
                        restOfName = ""
                    };
                }
                count++;
            }

            //Sort all the names in array using LINQ, sort by Last name, then the given name.
            allNames = allNames.Where(x => x != null).ToArray();
            allNames = allNames.OrderBy(x => x.lastName).ThenBy(x => x.givenName).ToArray();

            //Write all the sorted names in array into a text file in current executable path, before writing the values of array, write an empty string as a way to "Rewrite" everything
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            File.WriteAllText(path + @"\" + "sorted-names-list.txt", string.Empty);
            foreach (var x in allNames)
            {
                if (x.restOfName != "")
                {
                    File.AppendAllText(path + @"\" + "sorted-names-list.txt", x.restOfName + " " + x.givenName + " " + x.lastName + "\r\n");

                    rtb1.AppendText(x.restOfName + " " + x.givenName + " " + x.lastName + "\n");
                    rtb1.ScrollToCaret();
                }
                else
                {
                    File.AppendAllText(path + @"\" + "sorted-names-list.txt", x.givenName + " " + x.lastName + "\r\n");

                    rtb1.AppendText(x.givenName + " " + x.lastName + "\n");
                    rtb1.ScrollToCaret();
                }
            }

            // Keep the console window open in debug mode.
            MessageBox.Show("Sorted Successfully !");
            System.Diagnostics.Trace.WriteLine("Press any key to exit.");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
