   M S B A M L   `   `   `  ����J  FpruebaEditorTxt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:pruebaEditorTxtpruebaEditorTxt  5   	   1clr-namespace:pruebaEditorTxtpruebaEditorTxt  5   	        pruebaEditorTxt.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:pruebaEditorTxt     5   	   3	CustomTxtclr-namespace:pruebaEditorTxt     5   	   -   5          � Title$$  YATE (Yet Another Text Editor)��5	   	   $	��768��60   $
��1366��6=   
������� J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command$ ApplicationCommands.Open��5      6Y     5   
   -   5        $ ApplicationCommands.Save��5      6Y     5   
   -   5        $  ApplicationCommands.SaveAs��5      6]    J� InputBindings 5      �� 5   
    �� Command$ ApplicationCommands.Open��6    �� Gesture$ CTRL+O��68   �� 5   
   $ ApplicationCommands.Save��6   $ CTRL+S��68   �� 5   
   $  ApplicationCommands.SaveAs��6   $ CTRL+SHIFT+S��6:   .��5      � -   5         �Name$ grdPrincipal��5       � ColumnDefinitions 5   
   �� 5      $��* �6   6+    � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      $��* �6   6)   .+�5   
   o� -   5        $��menuMenuSup��5   ,   $��0��6   $��0��6   $
��Left=�6?   $
��Auto��6Z   $	��Top=�6h   -  'System.Windows.Input.KeyboardNavigation	   TabNavigation$
	 None=�6�   ��6�   �� 
 �� ElementName$
 grdPrincipal�� �� Path$ ActualWidth�.@�5      m� -   5        $��menuArchivo��5   )   $��_Archivo��6   .@�5!      m� -   5         m�Name$ 	menuNuevo��5!   *   $��Nuevo��6    m� InputGestureText$ Ctrl+N��6=   6W   m� 5"      -	   5        $ 	menuAbrir��5"   *   $��Abrir��6   $ Ctrl+O��6=    m� Command$ ApplicationCommands.Open��6W   m� 5#      -
   5        $ menuGuardar��5#   ,   $��Guardar��6   $ Ctrl+S��6A   $ ApplicationCommands.Save��6[   m� 5$      -   5        $ menuGuardarComo��5$   4   $��Guardar como...��6   $ Ctrl+Shift+S��6M   $  ApplicationCommands.SaveAs��6m   m� 5%      -   5        $ 
menuCerrar��5%   ,   $��_Cerrar��6   6@   �� 5&      m� 5'      -   5        $ 	menuSalir��5'   +   $��_Salir��6   $ Alt+F4��6>   6X   5(      m� 5)      -   5        $��menuEdicion��5)   )   $��	_Edición��6   .@�5*      m� -   5        $��menuDeshacer��5*   *   $
 Undo��6   6*   m� 5+      -   5        $��menuRehacer��5+   *   $
 Redo��6   6*   �� 5,      m� 5-      -   5        $��
menuCopiar��5-   *   $
 Copy��6   6*   m� 5.      -   5        $��
menuCortar��5.   )   $	 Cut��6   6)   m� 5/      -   5        $��	menuPegar��5/   +   $ Paste��6   6+   50      m� 51      -   5        $��menuVer��51   &   $
��_Ver��6   .@�52      m� -   5        $ 
menuFuente��52   /   $��
_Fuente...��6   6C   m� 53      -   5        $��menuTema��53   *   $��_Tema��6   .@�54      m� -   5        $��	menuClaro��54   .   $��Claro��6   6?   m� 55      -   5        $��
menuOscuro��55   /   $��Oscuro��6   6A   56      57      �� 58      -   5         ��Name$ btnVistaPrevia��58      $��Vista previa��6-   ���	#FFF0F0F06[   59      � 5:   
   -   5        $ grdContenido��5:      $��0��6%   $��1��65    5;      �� 5<      $��* �6#   6/   �� 5=      $��* �6#   6/   .+�5@      #   pruebaEditorTxt.TextBoxCustom  -   5          Name$ txtNuevo��5@   &   #   pruebaEditorTxt.TextBoxCustom   TextChanged$ txtNuevo_TextChanged��6I  $��0��68   $��2��6H   $��Consolas)�6\   $��14(�6r   $��Stretch=�6�    �� AcceptsReturn .  �� 
AcceptsTab . ��.  " �� VerticalScrollBarVisibility$
 Auto=�6�   $��10, 10, 10, 10q�6�    �� TextWrapping$
 Wrap=�6  $ �� HorizontalScrollBarVisibility$
 Auto=�6$  .��6l   J� Effect 5A      3  -System.Windows.Media.Effects.DropShadowEffect  5B         	Direction$	 135Q�6'      Color$ 	#FFBBBBBB��67   .��5C   "   5D      5E      5F      5G      