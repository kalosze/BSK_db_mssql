   M S B A M L   `   `   `  ����D  @interfejs, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:interfejs	interfejs  5   	       interfejs.AdminMenu   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:interfejs     5   	   -   5          � Title$!  Zarządzanie użytkownikami��5   	   $��238.154��6+   $��324.762��6<    � 
ResizeMode$ NoResize=�6L   .��5
      � .+�5   
   �� -   5         ��Name$ listaUzytkownikow��5      $
��Left=�6.   $��	10,29,0,0q�6I   $	��Top=�6\   $	��120��6t   5      �� 5   
   -   5         ��Name$ label��5      $��Użytkownik:��6   $
��Left=�65   $��10,3,0,0q�6P   $	��Top=�6b   �� 5   
   -   5         ��Name$ okBtn��5      $��OK��6    $
��Left=�6-   $��	31,0,0,19q�6H   $��74��6[   $��27��6g   $��Bottom=�6s   6�   �� 5   
   -   5        $ 	anulujBtn��5      $��Anuluj��6$   $��	0,0,31,19q�65   $��27��6H   $��Bottom=�6T   $��Right=�6o   $��75��6�   6�   �� 5   
   -   5        $ usunBtn��5      {�.  $��Usuń użytkownika��64   $
��Left=�6O   $��	10,83,0,0q�6j   $	��Top=�6}   $	��106��6�   $��22��6�   6�   �� 5   
   -   5        $ dodajUsrBtn��5      $��Dodaj użytkownika��6&   $
��Left=�6B   $��	10,56,0,0q�6]   $	��Top=�6p   $	��106��6�   $��22��6�   6�   � 5   
   $��83��6   $��
135,49,9,0q�6   $	��Top=�6/    � ColumnDefinitions 5      �� 5      $	��52* �6#   �� 5      .+�5      �� -   5         ��Name$ sliderEtykiet��5      $��	13,48,0,5q�6,    � Maximum$ 3Q�6?    � SmallChange$ 1Q�6K   	 �� TickPlacement$	 TopLeft=�6[   
 �� IsSnapToTickEnabled
 .  J� IsHitTestVisible . $
��Left=�6�   $	��140��6�   6�   �� 5      -	   5        $ label1��5      $��Poziom uprawnień:��6$   $��
15,0,31,57q�6@   �� 5      -
   5        $ label3��5      $��3��6$   $��140,26,0,31q�60   $
��Left=�6E   $��16��6`   �� 5      -   5        $ label4��5      $��2��6$   $��
97,26,0,31q�60   $
��Left=�6D   $��16��6_   �� 5      -   5        $ label5��5      $��1��6$   $��
53,26,0,31q�60   $
��Left=�6D   $��16��6_   �� 5      -   5        $ label6��5      $��0��6$   $��
10,26,0,31q�60    J� RenderTransformOrigin$ 0.554,0.356&�6D   $
��Left=�6h   $��16��6�   5      �� 5   
   -   5        $ 	passReset��5      {�.  $��Resetuj hasło��66   $
��Left=�6N   $��
10,110,0,0q�6i   $	��Top=�6}   $	��106��6�   $��22��6�   6�   �� 5   
   -   5        $ showPass��5      $y�Hidden=�6#   $��Pokaż hasło��67   {�.  $
��Left=�6_   $��
135,29,0,0q�6z   $	��Top=�6�   $��75��6�   6�   5!      5"      