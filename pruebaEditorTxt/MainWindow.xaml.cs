using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace pruebaEditorTxt
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string Ruta { get; set; }
        public bool VistaPrevia { get; set; }  // Si está activada o no
        public string Extension { get; set; }  // Extensión del archivo con el que estamos trabajando
        List<MenuItem> ObjetosMenu = new List<MenuItem>();  // Lista de submenús, útil para cambiarles el color a todos de una vez

        // Creamos la ventana web
        WebBrowser vistaWeb = new WebBrowser();

        public MainWindow()
        {
            InitializeComponent();
            txtNuevo.Focus();
            VistaPrevia = false;

            // Añadimos los submenús a la lista
            // Actualizar cada vez que creemos un nuevo submenú
            ObjetosMenu.Add(menuArchivo);
            ObjetosMenu.Add(menuNuevo);
            ObjetosMenu.Add(menuAbrir);
            ObjetosMenu.Add(menuGuardar);
            ObjetosMenu.Add(menuGuardarComo);
            ObjetosMenu.Add(menuCerrar);
            ObjetosMenu.Add(menuSalir);
            ObjetosMenu.Add(menuEdicion);
            ObjetosMenu.Add(menuDeshacer);
            ObjetosMenu.Add(menuRehacer);
            ObjetosMenu.Add(menuCopiar);
            ObjetosMenu.Add(menuCortar);
            ObjetosMenu.Add(menuPegar);
            ObjetosMenu.Add(menuVer);
            ObjetosMenu.Add(menuFuente);
            ObjetosMenu.Add(menuTema);
            ObjetosMenu.Add(menuClaro);
            ObjetosMenu.Add(menuOscuro);
        }

        private string ObtenerExtension(string nombre)
        {
            int punto = nombre.LastIndexOf(".");
            string extension = String.Empty;

            if (punto > 0) extension = nombre.Substring(punto);

            return extension;
        }

        private void GuardarNuevo()
        {
            SaveFileDialog dialogoGuardar = new SaveFileDialog();
            dialogoGuardar.Filter = "Texto plano (*.txt)|*.txt|HTML (*.html)|*.html|Markdown (*.md)|*.md";
            dialogoGuardar.Title = "Guardar un nuevo archivo...";

            if (dialogoGuardar.ShowDialog() == true)
            {
                string ruta = dialogoGuardar.FileName;
                string contenido = txtNuevo.Text;
                string nombre = dialogoGuardar.SafeFileName;

                File.WriteAllText(ruta, contenido);
                Ruta = ruta;
                Extension = ObtenerExtension(nombre);
                this.Title = $"{nombre} - YATE (Yet Another Text Editor)";
            }
        }

        private void MenuAbrir()
        {
            if (Ruta != null)  // Si hay un archivo abierto
            {
                MessageBoxResult respuesta = MessageBox.Show("Hay un archivo abierto\n¿Desea guardar los cambios en el archivo actual?", "Aviso", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (respuesta == MessageBoxResult.Yes)
                {
                    ActualizarArchivo();
                }
            }
            else if (Ruta == null && txtNuevo.Text != String.Empty)  // Si hay texto pero no se ha guardado en ningún archivo
            {
                MessageBoxResult respuesta = MessageBox.Show("Hay texto sin guardar\n¿Desea guardarlo?", "Aviso", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                if (respuesta == MessageBoxResult.Yes)
                {
                    ActualizarArchivo();
                }
            }

            try
            {
                AbrirArchivo();
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ActualizarArchivo()
        {
            string contenido = txtNuevo.Text;
            File.WriteAllText(Ruta, contenido);
        }

        private void AbrirArchivo()
        {
            OpenFileDialog dialogoAbrir = new OpenFileDialog();
            dialogoAbrir.Filter = "Markdown (*.md)|*.md|Texto plano (*.txt)|*.txt|HTML (*.html)|*.html|Todos los archivos (*.*)|*.*";

            if (dialogoAbrir.ShowDialog() == true)
            {
                string ruta = dialogoAbrir.FileName;
                string nombre = dialogoAbrir.SafeFileName;

                txtNuevo.Visibility = Visibility.Visible;
                txtNuevo.Text = File.ReadAllText(ruta);
                Ruta = ruta;
                Extension = ObtenerExtension(nombre);
                this.Title = $"{nombre} - YATE (Yet Another Text Editor)";
            }
        }

        private void CerrarArchivo()
        {
            if (Ruta != null)  // Si hay un archivo abierto
            {
                MessageBoxResult respuesta = MessageBox.Show("Hay un archivo abierto\n¿Desea guardar los cambios?", "Aviso", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                if (respuesta == MessageBoxResult.Yes)
                {
                    ActualizarArchivo();
                    LimpiarVentana();
                }
                else if (respuesta == MessageBoxResult.No)
                {
                    LimpiarVentana();
                }
            }
            else if (Ruta == null && txtNuevo.Text != String.Empty)  // Si hay texto pero no se ha guardado en ningún archivo
            {
                MessageBoxResult respuesta = MessageBox.Show("Hay texto sin guardar\n¿Desea guardarlo?", "Aviso", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                if (respuesta == MessageBoxResult.Yes)
                {
                    GuardarNuevo();
                    LimpiarVentana();
                }
                else if (respuesta == MessageBoxResult.No)
                {
                    LimpiarVentana();
                }
            }

            OcultarWebBrowser();
        }

        private void LimpiarVentana()
        {
            txtNuevo.Text = String.Empty;
            Ruta = null;
            Extension = null;
            txtNuevo.Visibility = Visibility.Hidden;
            this.Title = "YATE (Yet Another Text Editor)";
        }

        private SolidColorBrush CrearColor(string hex)
        {
            SolidColorBrush color = new SolidColorBrush();
            color = (SolidColorBrush)new BrushConverter().ConvertFrom(hex);

            return color;
        }

        private void MostrarWebBrowser()
        {
            // Encogemos la ventana de texto 
            txtNuevo.SetValue(Grid.ColumnSpanProperty, 1);
            txtNuevo.Margin = new Thickness(10, 10, 5, 10);
            // Añadimos la ventana web
            grdContenido.Children.Add(vistaWeb);
            // Le damos anchura
            vistaWeb.SetValue(Grid.ColumnProperty, 1);
            vistaWeb.Margin = new Thickness(5, 10, 10, 10);
            // Establecemos el contenido de la ventana web
            if (Extension != ".md")
            {
                // Obtenemos el texto del textbox directamente si no es Markdown. Si es HTML el WebBrowser sabe interpretar las tags
                if (txtNuevo.Text != String.Empty) vistaWeb.NavigateToString(txtNuevo.Text);
            }
            else
            {
                // Si es Markdown hay que reemplazar las tags de Markdown por las de HTML
                if (txtNuevo.Text != String.Empty) vistaWeb.NavigateToString(GenerarDesdeMarkdown(txtNuevo.Text));
            }
        }

        private void OcultarWebBrowser()
        {
            grdContenido.Children.Remove(vistaWeb);
            txtNuevo.SetValue(Grid.ColumnSpanProperty, 2);
            txtNuevo.Margin = new Thickness(10, 10, 10, 10);
        }

        private string GenerarDesdeMarkdown(string txt)
        {
            StringBuilder txtMostrar = new StringBuilder(txt);

            // Listas ordenadas
            txtMostrar = Reemplazos.CrearListasOrdenadas(txtMostrar.ToString());

            //// Líneas horizontales
            //txtMostrar = Reemplazos.BuscarReemplazar(txtMostrar, "***", "<hr>");
            //txtMostrar = Reemplazos.BuscarReemplazar(txtMostrar, "---", "<hr>");

            //// Imágenes
            //txtMostrar = Reemplazos.ReemplazarImagenes(txtMostrar.ToString());

            //// Enlaces
            //txtMostrar = Reemplazos.ReemplazarEnlaces(txtMostrar.ToString());

            //// Negrita
            //txtMostrar = Reemplazos.ReemplazarBloque(txtMostrar.ToString(), "**", "<strong>", "</strong>");
            //txtMostrar = Reemplazos.ReemplazarBloque(txtMostrar.ToString(), "__", "<strong>", "</strong>");

            //// Cursiva
            //txtMostrar = Reemplazos.ReemplazarBloque(txtMostrar.ToString(), "*", "<em>", "</em>");
            //txtMostrar = Reemplazos.ReemplazarBloque(txtMostrar.ToString(), "_", "<em>", "</em>");

            //// Texto monoespacio
            //txtMostrar = Reemplazos.ReemplazarBloque(txtMostrar.ToString(), "`", "<code>", "</code>");

            //// Encabezados
            //txtMostrar = Reemplazos.BuscarReemplazar(txtMostrar, "###### ", "</p><h6>", "</h6><p>");
            //txtMostrar = Reemplazos.BuscarReemplazar(txtMostrar, "##### ", "</p><h5>", "</h5><p>");
            //txtMostrar = Reemplazos.BuscarReemplazar(txtMostrar, "#### ", "</p><h4>", "</h4><p>");
            //txtMostrar = Reemplazos.BuscarReemplazar(txtMostrar, "### ", "</p><h3>", "</h3><p>");
            //txtMostrar = Reemplazos.BuscarReemplazar(txtMostrar, "## ", "</p><h2>", "</h2><p>");
            //txtMostrar = Reemplazos.BuscarReemplazar(txtMostrar, "# ", "</p><h1>", "</h1><p>");

            //// Citas
            //txtMostrar = Reemplazos.BuscarReemplazar(txtMostrar, "> ", "<blockquote>", "</blockquote>");

            // Párrafos
            txtMostrar = Reemplazos.ReemplazarParrafos(txtMostrar);

            txtMostrar.Insert(0, "<html><head><meta charset = 'UTF-8'></head><body>");
            txtMostrar.AppendLine("</body></html>");

            return txtMostrar.ToString();
        }
        
        #region Eventos

        // MENÚ ARCHIVO
        private void menuNuevo_Click(object sender, RoutedEventArgs e)
        {
            CerrarArchivo();
            txtNuevo.Visibility = Visibility.Visible;
            txtNuevo.Focus();
        }

        // Abrir, Guardar y Guardar Como van con comandos, no hay eventos en el click

        private void menuCerrar_Click(object sender, RoutedEventArgs e)
        {
            CerrarArchivo();
        }

        private void menuSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();  //  Llamamos al evento Window_Closing
        }

        // MENÚ VER
        private void menuFuente_Click(object sender, RoutedEventArgs e)
        {
            // FontDialog de WindowsForms, no hay en WPF y tenemos que convertir
            System.Windows.Forms.FontDialog dialogoFuente = new System.Windows.Forms.FontDialog();
            dialogoFuente.Font = new System.Drawing.Font("Consolas", 12);  // Aquí el tamaño se define en em; en XAML en ¿px?
            System.Windows.Forms.DialogResult resultado = dialogoFuente.ShowDialog();


            if (resultado == System.Windows.Forms.DialogResult.OK)
            {
                txtNuevo.FontFamily = new FontFamily(dialogoFuente.Font.Name);
                txtNuevo.FontSize = dialogoFuente.Font.Size * 96.0 / 72.0;
                txtNuevo.FontWeight = dialogoFuente.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                txtNuevo.FontStyle = dialogoFuente.Font.Italic ? FontStyles.Italic : FontStyles.Normal;
            }
        }

        private void menuClaro_Click(object sender, RoutedEventArgs e)
        {
            menuMenuSup.Background = CrearColor("#FFF0F0F0");
            menuMenuSup.Foreground = CrearColor("#FF212121");

            foreach (MenuItem menu in ObjetosMenu)
            {
                menu.Background = CrearColor("#FFF0F0F0");
                menu.Foreground = CrearColor("#FF212121");
                menu.BorderBrush = CrearColor("#FFF0F0F0");
            }

            btnVistaPrevia.Background = CrearColor("#FFF0F0F0");
            btnVistaPrevia.Foreground = CrearColor("#FF212121");

            txtNuevo.Background = Brushes.White;
            txtNuevo.Foreground = CrearColor("#FF000000");
        }

        private void menuOscuro_Click(object sender, RoutedEventArgs e)
        {
            menuMenuSup.Background = CrearColor("#FF2D2D30");

            foreach (MenuItem menu in ObjetosMenu)
            {
                menu.Background = CrearColor("#FF2D2D30");
                menu.Foreground = Brushes.DodgerBlue;
                menu.BorderBrush = CrearColor("#FF2D2D30");
            }

            btnVistaPrevia.Background = CrearColor("#FF2D2D30");
            btnVistaPrevia.Foreground = Brushes.DodgerBlue;

            txtNuevo.Background = CrearColor("#FF1E1E1E");
            txtNuevo.Foreground = CrearColor("#FFE6E6E6");
        }

        private void btnVistaPrevia_Click(object sender, RoutedEventArgs e)
        {
            if (VistaPrevia == false)
            {
                VistaPrevia = true;
                btnVistaPrevia.Content = "Vista previa <<";
                MostrarWebBrowser();
            }
            else
            {
                VistaPrevia = false;
                btnVistaPrevia.Content = "Vista previa >>";
                OcultarWebBrowser();
            }
        }

        private void txtNuevo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (VistaPrevia == true)
            {
                if (Extension != ".md")
                {
                    // Obtenemos el texto del textbox directamente si no es Markdown. Si es HTML el WebBrowser sabe interpretar las tags
                    if (txtNuevo.Text != String.Empty) vistaWeb.NavigateToString(txtNuevo.Text);
                }
                else
                {
                    // Si es Markdown hay que reemplazar las tags de Markdown por las de HTML
                    if (txtNuevo.Text != String.Empty) vistaWeb.NavigateToString(GenerarDesdeMarkdown(txtNuevo.Text));
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Ruta != null)  // Si hay un archivo abierto
            {
                MessageBoxResult respuesta = MessageBox.Show("Hay un archivo abierto\n¿Desea guardar los cambios?", "Aviso", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                if (respuesta == MessageBoxResult.Yes)
                {
                    ActualizarArchivo();
                    Application.Current.Shutdown();
                }
                else if (respuesta == MessageBoxResult.No)
                {
                    Application.Current.Shutdown();
                }
                else e.Cancel = true;
            }
            else if (Ruta == null && txtNuevo.Text != String.Empty)  // Si hay texto pero no se ha guardado en ningún archivo
            {
                MessageBoxResult respuesta = MessageBox.Show("Hay texto sin guardar\n¿Desea guardarlo?", "Aviso", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                if (respuesta == MessageBoxResult.Yes)
                {
                    GuardarNuevo();
                    e.Cancel = true;
                }
                else if (respuesta == MessageBoxResult.No)
                {
                    Application.Current.Shutdown();
                }
                else e.Cancel = true;
            }
        }

        // COMANDOS
        // Comando Abrir
        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MenuAbrir();
        }

        // Comando Guardar
        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (Ruta == null)
            {
                GuardarNuevo();
            }
            else
            {
                ActualizarArchivo();
            }
        }

        // Comando Guardar como
        private void SaveAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GuardarNuevo();
        }
        #endregion
    }
}

// TODO Listas ordenadas y sin ordenar
// TODO Bloques de código