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
using System.IO;
using Microsoft.Win32;
namespace MovingFolderFiles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void ClearTexboxes()
        {
            txtBoxFrom.Text = string.Empty;
            txtBoxTo.Clear();
        }

        private void SavingNames()
        {
            try
            {
                txtBoxFrom.Text = Properties.Settings.Default.FullPathFrom;
                txtBoxTo.Text = Properties.Settings.Default.FullPathTo;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void CloseTextBoxes()
        {
            txtBoxFrom.IsReadOnly = true;
            txtBoxTo.IsReadOnly = true;
        }

       
        OpenFileDialog openFileDialog = null;

        public MainWindow()
        {
            InitializeComponent();
            SavingNames();
            //       CloseTextBoxes();
            openFileDialog = new OpenFileDialog();
        }
        // SourcePath is this 'Properties.Settings.Default.FullPathFrom'
        // targetPath is this 'Properties.Settings.Default.FullPathTo'
        

        private void btnFrom_Click(object sender, RoutedEventArgs e)
        {
            

            try
            {
                lblMessage.Content = "From";
                openFileDialog.ValidateNames = false;
                openFileDialog.CheckFileExists = false;
                openFileDialog.CheckPathExists = true;
                openFileDialog.FileName = "Select All files from here";
                openFileDialog.ShowDialog(); // will allow both files and folders to be selected
                //Properties.Settings.Default.FullPathFrom is a string which holds '0' as a value as default
                Properties.Settings.Default.FullPathFrom = openFileDialog.FileName;
                FileInfo src = new FileInfo(Properties.Settings.Default.FullPathFrom);
                txtBoxFrom.Text = src.DirectoryName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            }

        private void btnTo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblMessage.Content = "To";
                openFileDialog.ValidateNames = false;
                openFileDialog.CheckFileExists = false;
                openFileDialog.CheckPathExists = true;
                openFileDialog.FileName = "Select All files from here";
                openFileDialog.ShowDialog(); // will allow both files and folders to be selected
                // Properties.Settings.Default.FullPathFrom is a string which holds '0' as a value as default
                Properties.Settings.Default.FullPathTo = openFileDialog.FileName;
                FileInfo src = new FileInfo(Properties.Settings.Default.FullPathTo);
                txtBoxTo.Text = src.DirectoryName;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
            }


        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            lblMessage.Content = "Working on it";
            try
            {
                if (Directory.Exists(txtBoxFrom.Text))
                {
                    string destFile = System.IO.Path.Combine(txtBoxFrom.Text, Properties.Settings.Default.FullPathFrom);

                    string[] files = Directory.GetFiles(txtBoxFrom.Text);

                    foreach (string s in files)
                    {
                        lblMessage.Content = "Moving";
                        Properties.Settings.Default.FullPathFrom = System.IO.Path.GetFileName(s);
                        destFile = System.IO.Path.Combine(txtBoxTo.Text, Properties.Settings.Default.FullPathFrom);
                        System.IO.File.Copy(s, destFile, true);

                    }
                    if (System.IO.Directory.Exists(txtBoxFrom.Text))
                    {
                        if (ck.IsChecked == false)
                        {
                            try
                            {
                                System.IO.Directory.Delete($@"{txtBoxFrom.Text}", true);
                                {
                                    lblMessage.Content = "Completed";
                                    MessageBox.Show("Finished");
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Just Copied");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error with finding folder to delete");
                    }

                    //System.IO.Directory.Move($@"{txtBoxFrom.Text}", $@"{txtBoxTo.Text}");
                }
                else
                {
                    if (txtBoxFrom.Text != string.Empty)
                        MessageBox.Show("You have already moved files or there are no files there");
                    else
                        MessageBox.Show("No folder selected");
                }
                }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
            }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Properties.Settings.Default.FullPathFrom = txtBoxFrom.Text;
                Properties.Settings.Default.FullPathTo = txtBoxTo.Text;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SavingNames();
        }

    }
}
