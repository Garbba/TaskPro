   M S B A M L   `   `   `  ����H  DWPFTaskPro_WS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:WPFTaskPro_WSWPFTaskPro_WS  5   	       WPFTaskPro_WS.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:WPFTaskPro_WS     5   	     � Title$  
MainWindow��5   	   $	��570��6   $	��800��6)    � WindowState$ 	Maximized=�65    � WindowStartupLocation$ CenterScreen=�6M   .��5
      � ���#FFC729.+�5   
   �� $	��300��6   $	��600��5       �� CornerRadius$ 6��5      ._�5      ��  �� Orientation$ Vertical=�6   $��20q�5      .+�5      �� $��Imgs/logo.png��6   $	��120��6/   $	��120��6;   $��Center=�6H   $
��0,10q�6e   �� 5      $��Inicio de sesión��6   $��25(�5      $��Center=�5      $	��Top=�5      $
��Bold"�5       �� Padding$ 10q�5      � 5      $��Center=�6    � RowDefinitions 5      �� 5      $��* �6(   �� 5      $��* �6(   �� 5      $��* �6(   �� 5      $��* �6(   .+�5      �� $#��Usuario o correo electrónico��6    $��0��6D   $
��0,15q�6Q   $
��Left=�6_   �� 5       -   5         ��Name$ txtuser��5       $��1��60   $��1��6@   $	��0,5q�6M   $	��255��6[   $��0,0,0,1q�6g   $
��Left=�6�   $��Usuario��6�   
������ 6�   �� 5!      $��Contraseña��6    $��2��62   $
��0,15q�6?   $
��Left=�6M   G� 5"      -   5         G�Name$ txtpass��5"   "   $��3��63   	 G� Password$	 Contraseña��6@   $	��0,5q�6V   $	��255��6d   $��0,0,0,1q�6p   
������ 6�   5#      �� 5%      -   5        
 ��Name$
 LoginBtn��5%      $��Iniciar sesión��5&      
����������#B82487$
��0,25q�5)      $��35��5*      $	��250��5+      ��5,      �� 5-       �� 
TargetType" ��6    .�5.      ��  �� Property � �� �� Value 5/   "   �� 50   &    �� 
TargetType" ��66   .R�51   *   ��  ��Name$ LoginBorder��61   #��z��6F   $ 16��52   -   ._�53   .   �� $��Center=�6?   $��Center=�54   ;   55   +    �� Triggers 56   *   P� 57   .    P� Property �@J�IsMouseOver P� Value . . �58   2   ��  �� 
TargetName$ LoginBorder��6a    � ��
 ������� 59   2   $ LoginBorder��6h    � ��
 �   �5:   /   5<   '   5>      5?      5A      �� 5B      $��No tienes cuenta?��6   $	��0,5q�65   $��Bottom=�6B   $��Center=�6]   �� 5D      -   5        $��
Registrate��5D      $
��0,10q�6.   $��Bottom=�6<   $��Center=�6W   ���#FFC729���#FFC729
����  �6�   5F      5G      5I      5L      