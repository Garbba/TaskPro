   M S B A M L   `   `   `  ����H  DWPFTaskPro_WS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:WPFTaskPro_WSWPFTaskPro_WS  5   	       WPFTaskPro_WS.ListEdit   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:WPFTaskPro_WS     5   	     � Title$  ListEdit��5   	   $	��570��6   $	��800��6'    � WindowState$ 	Maximized=�63    � WindowStartupLocation$ CenterScreen=�6K    � 	Resources 5
       �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius��%�  '��       �� 5   
   " ��6   .�5      ��  � �� �#B824876+   �� 5       � �� �#B824876,   �� 5       � ��$ 2q�60   �� 5       � ��
 �����6+   �� 5       � ��$ 10q�6(   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   #��z��5      #��z��5      $ 3��5      ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5      5      5      .��5!      � ���#FFC729	 � ColumnDefinitions	 5"   
   �� 5#      $��* �6   �� 5$      $	��300 �6   �� 5%      $��* �6   .+�5(   
   �� -   5        
 ��Name$
 lblLista��5(      $��Lista��6"   $��1��62   $��0,35,0,0q�6B   $��25(�6T   $
��Bold"�6b   �� 5*   
   $��1��6   $��0,90,0,0q�6%   .+�5.      �� -   5        $
 lblAgregarMiembro��5.      $!��Agregar miembro por usuario��6/   �� 5/      -   5         ��Name$ txtActualizarNombreLista��5/      
������ 68   � 51      $��2��6   $��Right=�6    	 52      �� 53      $
��Auto �6'   �� 54      $
��Auto �6'   .+�57      �� -   5         ��Name$ 	btndelete��57   )   $��0��6   $
��5,10q�6T   $��90��6b   .��58      �� Eliminar Lista5:      5;      �� 5=      -   5        $ 	btnupdate��5=   )   $��1��6   $
��5,10q�6T   $��90��6b   .��5>      �� Actualizar Lista5@      5A      5B      �� 5C      $��Usuario��6   �� 5D      -   5        $ 
txtMiembro��5D      
������ 6*   �� 5E      $��Tipo de acceso��6   �� 5G      -   5         ��Name$ cmbTipoAcceso��5G       �� SelectedIndex$ 2��6.   .@�5H      �� .��6   OWNER6&   �� 5I      .��6   ADMIN6&   �� 5J      .��6   MEMBER6'   5K      �� 5L      -   5        $ btnaddmember��5L   )   $��
230,10,5,0q�6   .��5M      �� 	Agregar5O      5P      �� 5Q      $��Eliminar miembro��6   �� 5R      $��0,0,0,10q�6   .+�5S      &   System.Windows.Controls.DataGrid  -	   5        $��
DGMiembros��5S   E   $��Center=�6   $��1��68      AutoGenerateColumns .     
IsReadOnly . $��	Quicksand)�6�      GridLinesVisibility$
 None=�6�      Columns 5T      4  .System.Windows.Controls.DataGridTemplateColumn  5U      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$	 290 61      CellTemplate 5V      �� 5W   "   .R�5X   &   � .+�5Y   *   �� $ 	btn_lista��6r   -
   5        $
��Left=�5Y   �   $��Center=�6�   $	��140��6�   $��	Quicksand)�6�   $��15��6�   $
��Left=�6�   $	��Top=�6  $��10(�6+  
�������$��0q�6L  
���   ���61   �� *user+.��5Z   .   ��  �� TextAlignment$
 Left=�6T   ��68   �� *
useraccess+6T   5[   +   5]   '   5^   #   5`      5b      5c      �� 5d      -   5        $ 	btncancel��5d   )   $��
230,10,5,0q�6   .��5e      �� 
Cancelar5g      5h      5j      5m      5n      