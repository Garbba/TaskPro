   M S B A M L   `   `   `  ����H  DWPFTaskPro_WS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:WPFTaskPro_WSWPFTaskPro_WS  5   	       WPFTaskPro_WS.Register   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:WPFTaskPro_WS     5   	     � Title$  Register��5   	   $	��570��6   $	��800��6'    � WindowState$ 	Maximized=�63    � WindowStartupLocation$ CenterScreen=�6K   .��5      �  J� RenderTransformOrigin$ 0.806,0.495&�6   ���#FFC729 � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��1��6   $��Center=�6"   .+�5      �� $��Registro��6   $��Center=�6'   $	��Top=�6D   $��24(�6\   
������ $
��Bold"�6�   $��0,0,0,10q�6�   �� 5      $	��0,3q�6    �� Orientation$ 
Horizontal=�6&   $��Center=�6?   .+�5      �� $��	Nickname:��6   $	��100��6,   �� 5      -   5         ��Name$ txtNickname��5      $	��200��6/   
������ 6;   5      �� 5      $	��0,3q�6   $ 
Horizontal=�6&   $��Center=�6?   .+�5      �� $��	Username:��6   $	��100��6,   �� 5      -   5        $ txtUsername��5      $	��200��6/   
������ 6;   5      �� 5       $	��0,3q�6   $ 
Horizontal=�6&   $��Center=�6?   .+�5!      �� $��	Lastname:��6   $	��100��6,   �� 5"      -   5        $ txtLastname��5"      $	��200��6/   
������ 6;   5#      �� 5$      $	��0,3q�6   $ 
Horizontal=�6&   $��Center=�6?   .+�5%      �� $��Email:��6   $	��100��6)   �� 5&      -   5        $ txtEmail��5&      $	��200��6,   
������ 68   5'      �� 5(      $	��0,3q�6   $ 
Horizontal=�6&   $��Center=�6?   .+�5)      �� $��	Password:��6   $	��100��6,   G� 5*      -   5         G�Name$ txtPassword��5*      $	��200��63   
������ 6?   5+      �� 5,      $��50,10q�6   .+�5-      �� -   5         ��Name$ RegisterBtn��5-   %   $��30��6   $��Registrarse��6:   ���#B82487
���������5.      �� 5/      	 �� 
TargetType"	 ��6    .�50      �� 
 �� Property
 � �� �� Value 51   "   �� 52   &    �� 
TargetType" ��66   .R�53   *   ��  ��Name$ LoginBorder��61   #��z��6F    �� CornerRadius$ 16��6q   ._�54   .   �� $��Center=�6?   $��Center=�6\   55   +    �� Triggers 56   *   P� 57   .    P� Property �@J�IsMouseOver P� Value . . �58   2   ��  �� 
TargetName$ LoginBorder��6a   
 � ��
 ������� 59   2   $ LoginBorder��6h   
 � ��
 �   �5:   /   5<   '   5>      5?      5A      5B      �� 5D      $��¿Ya tienes una cuenta?��6   $��Center=�65   $	��Top=�6R   $��29��6j   �� 5E      -   5        $��Inicia Sesión��5E      $��Center=�6-   $	��Top=�6J   ���#FFC729���#FFC729
����  �6�   5F      5G      5H      