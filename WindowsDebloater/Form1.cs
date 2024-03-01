using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsDebloater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string folderPath = "Assets"; // The folder path to search.
        private string result;
        private long maxGB_Size = 0;
        private string maxGB_SizeString;
        private List<string> searchResults = new List<string>();
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void SearchInFolder(string currentFolder)
        {
            try
            {
                if (Directory.Exists(currentFolder))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(currentFolder);
                    long folderSize = GetDirectorySize(directoryInfo);
                    if (folderSize >= maxGB_Size * 1024 * 1024 * 1024)
                    {
                        result = $"Folder size exceeds {maxGB_Size} GB: {currentFolder} - Size: {folderSize / (1024 * 1024 * 1024)} GB";
                        searchResults.Add(result);
                    }

                    string[] subDirectories = Directory.GetDirectories(currentFolder);

                    foreach (string subDirectory in subDirectories)
                    {
                        SearchInFolder(subDirectory); // Recursive call to search subdirectories
                    }
                }
                else
                {
                    result = $"Could not find folder: {currentFolder}";
                    searchResults.Add(result);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Handle unauthorized access to directories
                result = $"Access to folder is denied: {currentFolder}";
                searchResults.Add(result);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                result = $"An error occurred while accessing folder {currentFolder}: {ex.Message}";
                searchResults.Add(result);
            }
        }

        private long GetDirectorySize(DirectoryInfo directoryInfo)
        {
            long size = 0;

            // Add file sizes.
            FileInfo[] files = directoryInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                size += file.Length;
            }

            // Add subdirectory sizes.
            DirectoryInfo[] subDirectories = directoryInfo.GetDirectories();
            foreach (DirectoryInfo subDirectory in subDirectories)
            {
                size += GetDirectorySize(subDirectory);
            }

            return size;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            searchResults.Clear();
            SearchInFolder(folderPath);
            richTextBox1.Text = string.Join(Environment.NewLine, searchResults);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            folderPath = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Assuming maxGB_Size is a string variable
            maxGB_SizeString = textBox2.Text;

            // Using int.Parse() - This will throw an exception if the string is not a valid integer
            int GB_Int = int.Parse(maxGB_SizeString);
            maxGB_Size = GB_Int;

        }
    }
}