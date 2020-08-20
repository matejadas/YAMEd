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

namespace pruebaEditorTxt
{
    public class TextBoxCustom : TextBox
    {
        //TextBox custom para poder modificar los 8 espacios del tabulador a 4
        public int TabSize { get; set; }        

        public TextBoxCustom()
        {
            TabSize = 4;
        }

        protected override void OnPreviewKeyDown( KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                String tab = new String(' ', TabSize);
                int caretPosition = base.CaretIndex;
                base.Text = base.Text.Insert(caretPosition, tab);
                base.CaretIndex = caretPosition + TabSize + 1;
                e.Handled = true;
            }
        }
    }
}
