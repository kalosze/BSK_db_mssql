   M S B A M L   `   `   `  ����D  @interfejs, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:interfejs	interfejs  5   	       interfejs.newUser   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:interfejs     5   	   -   5          � Title$  Dodawanie użytkownika��5   	   $��297.504��6'   $��308.136��68    � 
ResizeMode$ NoResize=�6H   .��5
      � .+�5   
   �� -   5         ��Name$ button��5      $��OK��6!   $
��Left=�6.   $��	10,0,0,10q�6I   $��75��6\   $��20��6|   $��Bottom=�6�   �� 5   
   -   5        $ button1��5      $��Anuluj��6"   $��217,0,10,10q�63   $��20��6^   $��Bottom=�6j   � 5   
   $
��Left=�6   $	��205��6*   $��	10,10,0,0q�67   $	��Top=�6J   $	��272��6b   .+�5      �� -   5         ��Name$
 imie��5      $
��Left=�6$   $��23��6?   $��	111,0,0,0q�6K    �� TextWrapping$
 Wrap=�6^   $	��Top=�6r   $	��161��6�   �� 5      -   5        $ nazwisko��5      $
��Left=�6(   $��23��6C   $��
111,28,0,0q�6O   $
 Wrap=�6c   $	��Top=�6w   $	��161��6�   �� 5      -   5        $ login��5      $
��Left=�6%   $��23��6@   $��
111,56,0,0q�6L   $
 Wrap=�6`   $	��Top=�6t   $	��161��6�   G� 5      -   5         G�Name$ pass1��5      $
��Left=�6)   $��111,114,0,0q�6D   $	��Top=�6Y   $	��161��6q   $��23��6}   G� 5      -   5        $ pass2��5      $
��Left=�6)   $��111,142,0,0q�6D   $	��Top=�6Y   $	��161��6q   $��23��6}   �� 5      -	   5         ��Name$ label��5      $��Imię:��6#   $
��Left=�63   $��0,-4,0,0q�6N   $	��Top=�6`   �� 5      -
   5        $ label1��5      $��	Nazwisko:��6$   $
��Left=�68   $��0,25,0,0q�6S   $	��Top=�6e   �� 5      -   5        $ label2��5      $��Login:��6$   $
��Left=�65   $��0,51,0,0q�6P   $	��Top=�6b   �� 5      -   5        $ label4��5      $��Hasło:��6$   $
��Left=�65   $��	0,111,0,0q�6P   $	��Top=�6c   �� 5      -   5        $ label5��5      $��Potwierdź hasło:��6$   $
��Left=�6?   $��	0,139,0,0q�6Z   $	��Top=�6m   �� 5      -   5        $ label6��5      $��Stanowisko:��6$   $
��Left=�6:   $��0,82,0,0q�6U   $	��Top=�6g   �� 5      -   5         ��Name$ listaStanowisk��5      $
��Left=�6/   $��
111,84,0,0q�6J   $	��Top=�6^   $	��161��6v   .@�5      �� $��
Nauczyciel��6   �� 5      $��
Sekretarka��6   �� 5      $��Dyrektor��6   �� 5      $��Administrator��6   5      �� 5      -   5        $ 
hasloLabel��5   %   
���  ��$�� ��69   $��10,165,10,0q�6D   $	��Top=�6Y   $y�Visible=�6q   5       5"      5#      