   M S B A M L   `   `   `  ����D  @interfejs, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:interfejs	interfejs  5   	       interfejs.tableControl   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:interfejs     5   	   -   5          � Title$  Zarządzanie tabelami��5   	   $��184.158��6&   $��324.505��67    � 
ResizeMode$ NoResize=�6G   .��5
      � .+�5   
   �� -   5         ��Name$ okBtn��5      $��OK��6    $
��Left=�6-   $��	41,0,0,10q�6H   $��74��6[   $��27��6g   $��Bottom=�6s   6�   �� 5   
   -   5        $ 	anulujBtn��5      $��Anuluj��6$   $��	0,0,21,10q�65   $��27��6H   $��Bottom=�6T   $��Right=�6o   $��75��6�   6�   � 5   
   $
��Left=�6   $��51��6*   $��	10,13,0,0q�66   $	��Top=�6I   $	��140��6a   .+�5      �� -   5         ��Name$ wyborTabeli��5      $
��Left=�6,   $��0,29,0,0q�6G   $	��120��6Y   .@�5      �� -   5         ��Name$ 
textBlock2��5      $
��Wrap=�60   $��Tabela1��6D   5      �� 5      -   5         ��Name$ label2��5      $��Tabela:��6$   $
��Left=�66   $��0,3,0,0q�6Q   $	��Top=�6b   5      � 5   
   $��83��6   $��135,13,-18,0q�6   $	��Top=�61    � ColumnDefinitions 5      �� 5      $	��52* �6#   �� 5      .+�5      �� -   5         ��Name$ slider��5      $��	13,48,0,5q�6%    � Maximum$ 3Q�68   	 � SmallChange$	 1Q�6D   
 �� TickPlacement$
 TopLeft=�6T    �� IsSnapToTickEnabled .  J� IsHitTestVisible . $
��Left=�6�   $	��140��6�   6�   �� 5      -   5        $ label7��5      $��Poziom dostępu:��6$   $��
15,0,31,57q�6>   �� 5      -	   5        $ label8��5      $��3��6$   $��140,26,0,31q�60   $
��Left=�6E   $��16��6`   �� 5      -
   5        $ label9��5      $��2��6$   $��
97,26,0,31q�60   $
��Left=�6D   $��16��6_   �� 5      -   5        $ label10��5      $��1��6%   $��
53,26,0,31q�61   $
��Left=�6E   $��16��6`   �� 5      -   5        $ label11��5      $��0��6%   $��
10,26,0,31q�61    J� RenderTransformOrigin$ 0.554,0.356&�6E   $
��Left=�6i   $��16��6�   5      5       5!      