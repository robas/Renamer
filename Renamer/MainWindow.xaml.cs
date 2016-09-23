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
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;

namespace Renamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Collections.ArrayList asIsArrayList;
        System.Collections.ArrayList toBeArrayList;
        System.IO.SearchOption isRecursive = System.IO.SearchOption.TopDirectoryOnly;

        public MainWindow()
        {
            InitializeComponent();
            // Initializing ArrayLists
            asIsArrayList = new System.Collections.ArrayList();
            toBeArrayList = new System.Collections.ArrayList();
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            noDirSelectedLabel.Visibility = Visibility.Hidden;
            var dialog = new CommonOpenFileDialog();
            
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            
            if (result == CommonFileDialogResult.Ok)
            {
                dirTextBox.Text = dialog.FileName;
                getDirContent();
                asIsListBox.ItemsSource = asIsArrayList;
            }
        }

        private System.Collections.ArrayList getDirContent()
        {
            asIsArrayList = new System.Collections.ArrayList();
            //String[] allFiles = System.IO.Directory.GetFiles(dirTextBox.Text, "*.*", System.IO.SearchOption.AllDirectories);

            //String[] allFiles = GetFiles(dirTextBox.Text, "*.*", isRecursive);
            String[] allFiles = GetFiles(dirTextBox.Text, fileTypeComboBox.Text, isRecursive);
            foreach (var fileName in allFiles)
            {
                //FileInfo info = new FileInfo(file);
                asIsArrayList.Add(fileName);
                //fileName.
                
            }

            return asIsArrayList;
        }

        private void previewButton_Click(object sender, RoutedEventArgs e)
        {
            System.Collections.ArrayList conflictingRows = new System.Collections.ArrayList();
            foreach (String asIsFileName in asIsArrayList)
            {
                string onlyPath = System.IO.Path.GetDirectoryName(asIsFileName);
                string fileNameWithoutPath = System.IO.Path.GetFileNameWithoutExtension(asIsFileName);
                string extension = System.IO.Path.GetExtension(asIsFileName);

                string updatedFileName = fileNameWithoutPath.Replace(findTextBox.Text, replaceTextBox.Text);
                string toBeFileName = onlyPath + "\\" + updatedFileName + extension;
                
                // If there's already an existing filename, mark it as a problem.
                //if (toBeArrayList.Contains(toBeFileName))
                //{
                //    conflictingRows.Add(toBeArrayList.IndexOf(toBeFileName));
                //    conflictingRows.Add(toBeArrayList.Count);
                //}
                toBeArrayList.Add(toBeFileName);
            }
            toBeListBox.ItemsSource = toBeArrayList;
            //foreach (int nameConflictIndex in conflictingRows)
            //{
            //    toBeListBox.Items.GetItemAt(nameConflictIndex);
            //    toBeListBox.ItemTemplate = ;
            //}
            
        }

        private void asIsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            toBeListBox.SelectedIndex = asIsListBox.SelectedIndex;
        }

        private void toBeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            asIsListBox.SelectedIndex = toBeListBox.SelectedIndex;
        }

        private void replaceButton_Click(object sender, RoutedEventArgs e)
        {
            int listIndex = 0;
            foreach (String asIsFileName in asIsArrayList)
            {
                File.Move(asIsFileName, toBeArrayList[listIndex].ToString());
                listIndex++;
            }
        }

        private void recursiveBoxChecked(object sender, RoutedEventArgs e)
        {
            isRecursive = System.IO.SearchOption.AllDirectories;
        }

        private void recursiveBoxUnchecked(object sender, RoutedEventArgs e)
        {
            isRecursive = System.IO.SearchOption.TopDirectoryOnly;
        }

        public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            string[] searchPatterns = searchPattern.Split(',');
            List<string> files = new List<string>();
            foreach (string sp in searchPatterns)
                files.AddRange(System.IO.Directory.GetFiles(path, sp.Trim(), searchOption));
            files.Sort();
            return files.ToArray();
        }

        private void reloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (dirTextBox.Text != "")
            {
                getDirContent();
                asIsListBox.ItemsSource = asIsArrayList;
            } else
            {
                noDirSelectedLabel.Visibility = Visibility.Visible;
            }
        }
    }
}
